using System.Diagnostics;

namespace CallGate.Services.Email
{
    public class SandboxConsoleMailService : IMailService
    {
        public void SendMail(string mailTo, string subject, string body)
        {
            Debug.WriteLine($"-------------- Email sent --------------");
            Debug.WriteLine($"To: {@mailTo}");
            Debug.WriteLine($"Subject: {@subject}");
            Debug.WriteLine($"Message: {@body}");
            Debug.WriteLine($"----------------------------------------");
        }
    }
}
