namespace HCAMiniEHR.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
