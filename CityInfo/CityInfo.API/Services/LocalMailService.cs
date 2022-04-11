namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {

        private readonly string _emailTo = String.Empty;
        private readonly string _emailFrom = String.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            _emailTo = configuration["mailSettings:mailToAdress"];
            _emailFrom = configuration["mailSettings:mailFromAdress"];
        }


        public void Send(string subject, string message)
        {
            // Send mail - output to console window
            Console.WriteLine($"Mail from {_emailFrom} yo {_emailTo}");
            Console.WriteLine($"with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"message {message}");
        }
    }
}
