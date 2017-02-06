using Common.Domain;
using Common.Domain.Helper;
using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Base
{
    public abstract class ConfigDomainCore : DomainBase
    {

        private CurrentUser _user;

        public void Config(ICache cache)
        {

        }

        public CurrentUser ValidateAuth(string token, ICache cache)
        {
            if (_user.IsNull())
                _user =  HelperCurrentUser.ValidateAuth(token, cache);

            return _user;
        }

        public override void Dispose()
        {

        }
    }
}
