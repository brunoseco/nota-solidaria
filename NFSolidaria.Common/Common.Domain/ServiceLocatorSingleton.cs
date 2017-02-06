using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ServiceLocatorSingleton
    {

        private static IServiceLocator _instance;

        private ServiceLocatorSingleton() { }

        public static IServiceLocator GetInstance
        {
            get
            {
                return _instance;
            }
        }

        public static void SetInstance(IServiceLocator instance)
        {
            if (instance.IsNull())
                return;

            if (_instance.IsNull())
            {
                _instance = instance;
                return;
            }

            lock (_instance)
            {
                _instance = instance;
            }

        }

    }
}
