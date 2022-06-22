using Microsoft.WindowsAzure.MobileServices;
using System;


namespace mARkIt.Authentication
{
    public class UnauthorizedException : Exception
    {
        public MobileServiceAuthenticationProvider AuthType { get; }

        public UnauthorizedException(MobileServiceAuthenticationProvider i_AuthType)
        {
            AuthType = i_AuthType;
        }
    }
}
