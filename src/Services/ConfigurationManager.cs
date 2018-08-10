namespace CallGate.Services
{
    public class ConfigurationManager 
    {
        public string JwtSiginingKey { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }
        public int MailPort { get; set; }
        public string MailHost { get; set; }
        public string MailSender { get; set; }
    }
}
