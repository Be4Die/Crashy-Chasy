using Alchemy.Inspector;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.LoadingScreen
{
    public sealed class LoadingScreenInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField, AssetsOnly] private LoadingScreenView _loadingScreenPrefab;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            if (Utils.IsServer()) return;
            var screen = Instantiate(_loadingScreenPrefab);
            DontDestroyOnLoad(screen.gameObject);
            screen.SetVisible(false);

            containerBuilder.AddSingleton(new LoadingScreenController(screen));
        }
    }
}