using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.Scenes
{
    public sealed class SceneCollectionInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private SceneCollection _sceneCollection;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(_sceneCollection);
        }
    }
}