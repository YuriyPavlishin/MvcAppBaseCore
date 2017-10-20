/// <binding ProjectOpened='project:open' Clean='clean:all' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require('gulp-rename'),
    gulpFilter = require('gulp-filter'),
    es = require('event-stream'),
    sourcemaps = require('gulp-sourcemaps'),
    sass = require('gulp-sass'),
    runSequence = require('run-sequence');

var paths = {
    webroot: "./wwwroot/",
    node_modules: "./node_modules/"
};


gulp.task("clean:all", function (cb) {
    rimraf(getBundlePath(''), cb);
});

gulp.task('watch:js', function () {
    return gulp.watch([getRootPath("js/**/*.js"), getRootPath("app-out/**/*.js")], ['site:concatOnly:js']);
});

gulp.task('watch:css', function () {
    return gulp.watch(getRootPath("sass/**/*.scss"), ['site:concatOnly:css']);
});


gulp.task("vendor:js", function () {
    var otherVendorStream = gulp.src(getOtherVendorJs(), { base: "." })
        .pipe(concat(getBundlePath("vendor.js")))
        .pipe(gulp.dest("."))
        .pipe(uglify())
        .pipe(rename({ extname: '.min.js' }))
        .pipe(gulp.dest("."));


    return es.concat(
        otherVendorStream,
        copyFilesToBundleFromNodeStream("jquery/dist/jquery", "js", true),
        copyFilesToBundleFromNodeStream("bootstrap/dist/js/bootstrap", "js"),
        copyFilesToBundleFromNodeStream("jquery-ui-dist/jquery-ui", "js")
    );
});

function getOtherVendorJs() {
    return [
        getNodeModulesPath("jquery-validation/dist/jquery.validate.js"),
        getNodeModulesPath("jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"),
        getNodeModulesPath("underscore/underscore.js"),
        getNodeModulesPath("jquery.cookie/jquery.cookie.js"),
        getNodeModulesPath("jquery-json/src/jquery.json.js"),
        getNodeModulesPath("bootstrap-notify/bootstrap-notify.js"),
        getRootPath("lib/non-npm/*.js")
    ];
}


function copyFilesToBundleFromNodeStream(basePath, extension, supportSourceMap, dest) {
    var fullPath = getNodeModulesPath(basePath);
    var files = [
        fullPath + "." + extension,
        fullPath + ".min." + extension
    ];
    if (supportSourceMap) {
        files.push(fullPath + ".min.map");
    }
    var destination = getRootPath("bundle");
    if (dest) {
        destination = dest;
    }

    return gulp.src(files)
        .pipe(gulp.dest(destination));
}

gulp.task("vendor:css", function () {
    var excludeBootstrapJs = "!" + getNodeModulesPath("bootstrap/dist/js{,/**/*}");
    var excludeBootstrapTheme = "!" + getNodeModulesPath("bootstrap/dist/css/bootstrap-theme*");
    var bootstrapStream = gulp.src([getNodeModulesPath("bootstrap/dist/**"), excludeBootstrapJs, excludeBootstrapTheme])
        .pipe(gulp.dest(getRootPath("bundle/bootstrap")));

    var jqueryUiCssStream = copyFilesToBundleFromNodeStream("jquery-ui-dist/jquery-ui", "css", false, getBundlePath("jquery-ui"));
    var jqueryUiImagesStream = gulp.src([getNodeModulesPath("jquery-ui-dist/images/**")])
        .pipe(gulp.dest(getBundlePath("jquery-ui/images")));

    var vendorCssStream = gulp.src(getOtherVendorCss(), { base: "." })
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(concat(getBundlePath('vendor.css')))
        .pipe(gulp.dest("."))
        .pipe(cssmin())
        .pipe(rename({ extname: '.min.css' }))
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest("."));

    return es.concat(bootstrapStream, jqueryUiCssStream, jqueryUiImagesStream, vendorCssStream);
});

function getOtherVendorCss() {
    return [
        getNodeModulesPath("bootstrap/dist/css/bootstrap-theme.css"),
        getNodeModulesPath("animate.css/animate.css")
    ];
}

gulp.task("site:concatOnly:js", function () {
    return createAllJs(false);
});

gulp.task("site:min:js", function () {
    return createAllJs(true);
});

gulp.task("site:concatOnly:css", function () {
    return createAllSass(false);
});

gulp.task("site:min:css", function () {
    return createAllSass(true);
});

gulp.task("project:open", ["site:concatOnly:js", "site:concatOnly:css", "watch:js", "watch:css"]);

gulp.task('allTasks', function (callback) {
    runSequence('clean:all',
        ["vendor:js", "site:min:js", "vendor:css", "site:concatOnly:css", "site:min:css"],
        callback);
});


function createAllJs(performUglify) {
    var concatAll = gulp.src(getSiteScripts(), { base: "." })
        .pipe(sourcemaps.init())
        .pipe(concat(getBundlePath('all.js')))
        .pipe(gulp.dest("."));

    if (!performUglify)
        return concatAll;

    return concatAll.pipe(uglify())
        .pipe(rename({ extname: '.min.js' }))
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest("."));
}

function createAllSass(performUglify) {
    var sassStream = gulp.src(getSiteSass())
        .pipe(sourcemaps.init())
        .pipe(concat(getBundlePath('all.css')))
        .pipe(sass.sync().on('error', sass.logError));

    if (performUglify) {
        sassStream = sassStream
            .pipe(cssmin())
            .pipe(rename({ extname: '.min.css' }))
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest('.'));
    }
    else {
        sassStream = sassStream
            .pipe(sourcemaps.write('./'))
            .pipe(gulp.dest('.'));
    }

    var imagesStream = gulp.src([getRootPath("sass/images/**")])
        .pipe(gulp.dest(getBundlePath("images")));

    return es.concat(sassStream, imagesStream);
}

function getSiteScripts() {
    return [
        getRootPath("js/defaults.js"),
        getRootPath("app-out/app.js"),
        getRootPath("js/common/**/*.js"),
        getRootPath("js/app-site.js")
    ];
}

function getSiteSass() {
    return [
        getRootPath('sass/bootstrap-override.scss'),
        getRootPath('sass/site.scss')
    ];
}

function getBundlePath(relativePath) {
    return getRootPath('bundle/' + relativePath);
}

function getRootPath(relativePath) {
    return paths.webroot + relativePath;
}

function getNodeModulesPath(relativePath) {
    return paths.node_modules + relativePath;
}