using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
        public Task SendPasswordResetEmailAsync(string emailAddress, string resetLink);
    }
}
