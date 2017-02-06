using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Enums;

namespace Common.Domain.Interfaces
{
    public interface ISMS
    {
        void Reset();

        string User { get; set; }
        string Password { get; set; }
        TipoSMS Tipo { get; set; }
        string PhoneNumberFrom { get; set; }

        void Add(string phoneNumber, string message);

        RetornoSMS Send();

    }
}
