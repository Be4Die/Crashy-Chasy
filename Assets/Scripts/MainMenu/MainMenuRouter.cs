using CrashyChasy.Game;
using CrashyChasy.LoadingScreen;
using CrashyChasy.Scenes;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuRouter
    {
        private readonly SceneCollection _sceneCollection;
        private readonly LoadingScreenController _loadingScreenController;

        public MainMenuRouter(SceneCollection sceneCollection, LoadingScreenController loadingScreenController)
        {
            _sceneCollection = sceneCollection;
            _loadingScreenController = loadingScreenController;
        }

        public void LoadGameWithParameters(GameBootParameters gameBootParameters)
        {
            var extraInstallerScope = new ExtraInstallerScope(container =>
            {
                container.AddSingleton(gameBootParameters);
            });
            
            var loadSceneOperation = new LoadingOperation
            {
                Name = "Загрузка сцены",
                Action = async progressReporter =>
                {
                    var operation = SceneManager.LoadSceneAsync(_sceneCollection.GameSceneName);
                    if (operation != null)
                    {
                        operation.allowSceneActivation = false;

                        while (!operation.isDone)
                        {
                            var progress = Mathf.Clamp01(operation.progress / 0.9f);
                            progressReporter.Report(progress);

                            if (operation.progress >= 0.9f)
                            {
                                operation.allowSceneActivation = true;
                            }

                            await UniTask.Yield();
                        }
                    }

                    progressReporter.Report(1f);
                    
                    extraInstallerScope.Dispose();
                }
            };
            
            _loadingScreenController.Show();
            _loadingScreenController.ResetProgress();

            _loadingScreenController.LoadOperations(
                groupWeight: 0.5f,
                loadSceneOperation
            ).Forget();
        }
    }
}