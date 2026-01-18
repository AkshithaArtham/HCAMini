using System;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }

        [Required]
        [StringLength(100)]
        public string TableName { get; set; }

        [Required]
        [StringLength(10)]
        public string Action { get; set; }  // INSERT, UPDATE, DELETE

        public int RecordId { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string ChangedBy { get; set; }
    }
}