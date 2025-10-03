using System;
using UnityEngine;
using UnityEngine.UI;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuView : MonoBehaviour
    {
        public event Action PlayOnlineClicked;
        public event Action PlayOfflineClicked;
        
        [SerializeField] private Button _playOnlineButton;
        [SerializeField] private Button _playOfflineButton;

        private void OnEnable()
        {
            _playOnlineButton.onClick.AddListener(OnPlayOnlineClicked);
            _playOfflineButton.onClick.AddListener(OnPlayOfflineClicked);
        }

        private void OnDisable()
        {
            _playOnlineButton.onClick.RemoveListener(OnPlayOnlineClicked);
            _playOfflineButton.onClick.RemoveListener(OnPlayOfflineClicked);
        }

        private void OnPlayOnlineClicked() => PlayOnlineClicked?.Invoke();
        private void OnPlayOfflineClicked() => PlayOfflineClicked?.Invoke();
    }
}