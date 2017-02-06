using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using System;
using System.Configuration;

namespace Common.Domain.Helper
{
    public static class HelperCurrentUser
    {

        public static CurrentUser ValidateAuth(string token, ICache cache)
        {
            var exists = cache.ExistsKey(token);

            if (!exists)
                throw new CustomNotAutorizedException(string.Format("Usuário [{0}] não autenticado", token));

            return (CurrentUser)cache.Get(token);
        }        
    }
}
