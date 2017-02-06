using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IEmail : IDisposable
    {
        string Subject { get; set; }
        string Message { get; set; }

        bool Send();
        void EmailRecipientsAdd(string EmailRecipient);
        void EmailRecipientsClear();
    }
}
