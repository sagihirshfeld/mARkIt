using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public interface IAuthenticationDelegate
    {
        void OnAuthenticationCompleted(Account i_Account);
        void OnAuthenticationFailed(string i_Message, Exception i_Exception);
        void OnAuthenticationCanceled();
    }
}
