using Common.Domain;
using Common.Domain.Helper;
using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Helpers
{
    public static class HelperValidateAuth
    {
        private static string _simple { get { return "{0}-S"; } }

        public static CurrentUser ValidateAuthSimple(string token, ICache cache)
        {
            return ValidateAuth(TokenSimple(token), cache);
        }

        private static CurrentUser ValidateAuth(string token, ICache cache)
        {
            return HelperCurrentUser.ValidateAuth(token, cache);
        }

        public static string TokenSimple(string token)
        {
            return string.Format(_simple, token);
        }


    }
}
