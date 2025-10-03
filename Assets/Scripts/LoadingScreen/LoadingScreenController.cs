// LoadingScreenController.cs (полностью переработан)
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

namespace CrashyChasy.LoadingScreen
{
    public sealed class LoadingScreenController
    {
        private readonly LoadingScreenView _view;
        private float _overallProgress;

        public LoadingScreenController(LoadingScreenView view)
        {
            _view = view;
        }

        public void Show() => _view.SetVisible(true);
        public void Hide() => _view.SetVisible(false);
        public void ResetProgress() => _overallProgress = 0f;

        public async UniTask LoadOperations(
            float groupWeight, 
            params LoadingOperation[] operations)
        {
            if (operations == null || operations.Length == 0)
            {
                _overallProgress += groupWeight;
                return;
            }

            var totalGroupWeight = operations.Sum(op => op.Weight);
            var completedInGroup = 0f;

            foreach (var operation in operations)
            {
                SetStatus(operation.Name);

                var group = completedInGroup;
                await operation.Action(new Progress<float>(progress => 
                {
                    var operationProgress = group + progress * operation.Weight;
                    var groupProgress = operationProgress / totalGroupWeight;
                    SetProgress(_overallProgress + groupProgress * groupWeight);
                }));

                completedInGroup += operation.Weight;
            }

            _overallProgress += groupWeight;
        }

        private void SetStatus(string status) => _view.SetStatus(status);
        private void SetProgress(float progress) => _view.SetProgress(progress);
    }
}