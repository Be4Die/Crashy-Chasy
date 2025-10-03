using CrashyChasy.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CrashyChasy.Booting
{
    public sealed class ServerBootLoader : IBootLoader
    {
        private readonly string _sceneToLoad;

        public ServerBootLoader(SceneCollection sceneCollection)
        {
            _sceneToLoad = sceneCollection.GameSceneName;
        }

        public async UniTask Load()
        {
            await SceneManager.LoadSceneAsync(_sceneToLoad);
        }
    }
}