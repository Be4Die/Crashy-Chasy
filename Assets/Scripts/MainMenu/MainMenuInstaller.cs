using Alchemy.Inspector;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField, ReadOnly] private MainMenuView _view;
        
        [Button]
        private void OnValidate()
        {
            _view ??= FindObjectOfType<MainMenuView>();
        }

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_view);
            containerBuilder.AddSingleton(typeof(MainMenuEntity));
            containerBuilder.AddSingleton(typeof(MainMenuRouter));
            containerBuilder.AddSingleton(typeof(MainMenuInteractor));
            containerBuilder.AddSingleton(typeof(MainMenuPresenter));
        }
    }
}