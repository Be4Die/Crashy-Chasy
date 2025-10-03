using CrashyChasy.LoadingScreen;
using CrashyChasy.Scenes;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.Booting
{
    public sealed class BootLoaderInstaller : MonoBehaviour, IInstaller
    {
        private static IBootLoader Create(Container container)
        {
            if (Utils.IsServer())
            {
                return new ServerBootLoader(
                    container.Resolve<SceneCollection>()
                );
            }
            else
            {
                return new ClientBootLoader(
                    container.Resolve<SceneCollection>(),
                    container.Resolve<LoadingScreenController>()
                );
            }
        }
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(Create);
        }
    }
}