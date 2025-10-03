using CrashyChasy.LoadingScreen;
using CrashyChasy.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrashyChasy.Booting
{
    public sealed class ClientBootLoader : IBootLoader
    {
        private readonly string _sceneToLoad;
        private readonly LoadingScreenController _loadingScreenController;
        
        public ClientBootLoader(
            SceneCollection sceneCollection,
            LoadingScreenController loadingScreenController
        )
        {
            _sceneToLoad = sceneCollection.MainMenuSceneName;
            _loadingScreenController = loadingScreenController;
        }

        public async UniTask Load()
        {
            _loadingScreenController.Show();
            await _loadingScreenController.LoadOperations(1,
                new LoadingOperation
                {
                    Name = "Loading Scene",
                    Action = async (progress) =>
                    {
                        var asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad);
                        if (asyncOperation != null)
                        {
                            asyncOperation.allowSceneActivation = false;

                            while (!asyncOperation.isDone)
                            {
                                var normalizedProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                                progress.Report(normalizedProgress);

                                if (asyncOperation.progress >= 0.9f)
                                {
                                    asyncOperation.allowSceneActivation = true;
                                }

                                await UniTask.Yield();
                            }
                        }
                    },
                    Weight = 1f
                }
            );
            _loadingScreenController.Hide();
        }
    }
}