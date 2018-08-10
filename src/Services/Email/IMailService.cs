namespace CallGate.Services.Email
{
    public interface IMailService
    {
        void SendMail(string mailTo, string subject, string body);
    }
}