using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using ZzaDesktop.Services;

namespace ZzaDesktop
{
    /*
     * The container treats IOC and DI as two phase approach to construct the object graph
     * - The IoC pattern is about delegating responsibility for construction...                *
     * - The Dependency Injection pattern is about providing dependencies to an object that's already been constructed...
     */
    public static class ContainerHelper
    {
        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ICustomersRepository, CustomersRepository>(new ContainerControlledLifetimeManager());
        }
        public static IUnityContainer Container => _container;
    }
}
