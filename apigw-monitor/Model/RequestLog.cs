namespace apigw_monitor.Model
{
    public class RequestLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Headers { get; set; }
        public string Body { get; set; }
    }
}
