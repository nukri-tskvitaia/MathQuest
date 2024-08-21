using System;
using Business.Interfaces;
using System.Web;

namespace Business.Services
{
    public class ConsoleEmailSenderService : IConsoleEmailSenderService
    {
        public Task SendEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            Console.WriteLine("---New Email----");
            Console.WriteLine($"To: {emailAddress}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine(HttpUtility.HtmlDecode(htmlMessage));
            Console.WriteLine("-------");
            return Task.CompletedTask;
        }
    }
}
