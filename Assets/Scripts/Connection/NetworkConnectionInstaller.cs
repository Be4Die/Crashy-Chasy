using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Transporting;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.Connection
{
    public sealed class NetworkConnectionInstaller : MonoBehaviour, IInstaller
    {
        private static INetworkConnectionService Create(Container container)
        {
            if (Utils.IsServer())
            {
                return new ServerConnectionService(
                    container.Resolve<Transport>(), 
                    container.Resolve<ServerManager>()
                    );
            }
            else
            {
                return new ClientConnectionService(
                    container.Resolve<ClientManager>(),
                    container.Resolve<Transport>()
                    );
            }
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(Create);
        }
    }
}