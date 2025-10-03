using UnityEngine;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuPresenter
    {
        private readonly MainMenuView _view;
        private readonly MainMenuInteractor _interactor;

        public MainMenuPresenter(MainMenuView view, MainMenuInteractor interactor)
        {
            _view = view;
            _interactor = interactor;
        }

        public void Subscribe()
        {
            _view.PlayOnlineClicked += OnPlayOnlineClicked;
            _view.PlayOfflineClicked += OnPlayOfflineClicked;
        }
        
        public void Unsubscribe()
        {
            _view.PlayOnlineClicked -= OnPlayOnlineClicked;
            _view.PlayOfflineClicked -= OnPlayOfflineClicked;
        }

        private void OnPlayOfflineClicked() => _interactor.ExecuteOffline();
        private void OnPlayOnlineClicked() => _interactor.ExecuteOnline();
    }
}