using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Infrastructure.Email
{
    public interface IEmailSender
    {
        public Task SendEmail(Message message);
    }
}
