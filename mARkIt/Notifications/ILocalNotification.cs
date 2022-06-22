using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mARkIt.Notifications
{
    public interface ILocalNotification
    {
        void Show(string title, string message);
    }
}
