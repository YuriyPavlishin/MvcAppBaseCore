/// <binding ProjectOpened='project:open' Clean='clean:all' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cleanCSS = require("gulp-clean-css"),
    uglify = require("gulp-uglify"),
    rename = require('gulp-rename'),    
    merge = require('merge-stream'),
    sourcemaps = require('gulp-sourcemaps'),
    sass = require('gulp-sass')(require('sass')),
    autoprefixer = require('gulp-autoprefixer');

var paths = {
    webroot: "./wwwroot/",
    node_modules: "./node_modules/"
};


gulp.task("clean:all", function (cb) {
    rimraf(getBundlePath(''), cb);
});

gulp.task('watch:js', function () {
    return gulp.watch([getRootPath("js/**/*.js"), getRootPath("app-out/**/*.js")], gulp.series('site:concatOnly:js'));
});

gulp.task('watch:css', function () {
    return gulp.watch(getRootPath("sass/**/*.scss"), gulp.series('site:concatOnly:css'));
});


gulp.task("vendor:js", function () {
    var otherVendorStream = gulp.src(getOtherVendorJs(), { base: "." })
        .pipe(concat(getBundlePath("vendor.js")))
        .pipe(gulp.dest("."))
        .pipe(uglify())
        .pipe(rename({ extname: '.min.js' }))
        .pipe(gulp.dest("."));


    return merge(
        otherVendorStream,
        copyFilesToBundleFromNodeStream("jquery/dist/jquery", "js", true),
        copyFilesToBundleFromNodeStream("bootstrap/dist/js/bootstrap", "js"),
        copyFilesToBundleFromNodeStream("jquery-ui-dist/jquery-ui", "js")
    );
});

function getOtherVendorJs() {
    return [
        getNodeModulesPath("jquery-validation/dist/jquery.validate.js"),
        getNodeModulesPath("jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js"),
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
        .pipe(cleanCSS())
        .pipe(rename({ extname: '.min.css' }))
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest("."));

    return merge(bootstrapStream, jqueryUiCssStream, jqueryUiImagesStream, vendorCssStream);
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

gulp.task('project:open', gulp.series(gulp.parallel("site:concatOnly:js", "site:concatOnly:css", "watch:js", "watch:css")));

gulp.task('allTasks',
    gulp.series('clean:all', gulp.series(gulp.parallel("vendor:js", "site:min:js", "vendor:css", "site:concatOnly:css", "site:min:css")))
);


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
        .pipe(autoprefixer(['last 15 version', '>1%', 'ie 8', 'ie 7'], { cascade: true }))
        .pipe(concat(getBundlePath('all.css')))
        .pipe(sass.sync().on('error', sass.logError));

    if (performUglify) {
        sassStream = sassStream
            .pipe(cleanCSS())
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

    return merge(sassStream, imagesStream);
}

function getSiteScripts() {
    return [
        getRootPath("app-out/app.js")
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
