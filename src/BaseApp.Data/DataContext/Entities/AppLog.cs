using System;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Data.DataContext.Entities
{
    public class AppLog
    {
        public int Id { get; set; }
        [StringLength(128)]
        public string LogName { get; set; }
        [StringLength(64)]
        public string LogLevel { get; set; }
        public DateTime? LogDate { get; set; }
        [StringLength(64)]
        public string AppVersion { get; set; }
        [StringLength(128)]
        public string UserName { get; set; } 
        [StringLength(64)]
        public string ClientIp { get; set; }
        [StringLength(32)]
        public string RequestMethod { get; set; }
        [StringLength(256)]
        public string RequestContentType { get; set; }
        [StringLength(1024)]
        public string RequestUrl { get; set; }
        [StringLength(2048)]
        public string QueryString { get; set; }
        [StringLength(1024)]
        public string RefererUrl { get; set; }
        [StringLength(1024)]
        public string UserAgent { get; set; }
        [StringLength(512)]
        public string Callsite { get; set; }
        [MaxLength]
        public string Message { get; set; }
        [MaxLength]
        public string Exception { get; set; }
    }
}
