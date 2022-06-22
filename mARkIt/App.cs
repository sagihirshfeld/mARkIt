using mARkIt.Models;
using System;

namespace mARkIt
{
    public sealed class App
    {
        private static App s_Instance = null;
        private static object s_LockObj = new Object();
        public static event Action<User> UserChanged;

        private App() { }

        public static App Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new App();
                        }
                    }
                }

                return s_Instance;
            }
        }

        private User m_User;

        public static User ConnectedUser
        {
            get
            {
                return Instance.m_User;
            }

            set
            {
                if (Instance.m_User != value && value != null)
                {
                    Instance.m_User = value;
                    UserChanged?.Invoke(Instance.m_User);
                }
            }
        }
    }
}
