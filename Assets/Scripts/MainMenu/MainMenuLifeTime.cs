using Reflex.Attributes;
using UnityEngine;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuLifeTime : MonoBehaviour
    {
        [Inject] private readonly MainMenuPresenter _presenter;

        private void Awake()
        {
            _presenter.Subscribe();
        }

        private void OnDestroy()
        {
            _presenter.Unsubscribe();
        }
    }
}