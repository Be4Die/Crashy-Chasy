using Alchemy.Inspector;
using FishNet.Managing;
using FishNet.Transporting;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy
{
    public sealed class FishnetInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField, AssetsOnly] private NetworkManager _networkPrefab;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            var network = Instantiate(_networkPrefab);
            DontDestroyOnLoad(network.gameObject);
            
            containerBuilder.AddSingleton(network);
            containerBuilder.AddSingleton(network.ServerManager);
            containerBuilder.AddSingleton(network.ClientManager);
            containerBuilder.AddSingleton<Transport>(_ => network.TransportManager.Transport);
        }
    }
}