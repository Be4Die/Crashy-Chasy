using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrashyChasy.Game.DeathScreen
{
    public sealed class DeathScreenView : MonoBehaviour
    {
        public event Action RespawnClicked;
        public event Action MenuClicked;
        
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _respawnButton;
        [SerializeField] private Button _menuButton;

        private void OnEnable()
        {
            _respawnButton.onClick.AddListener(OnRespawnClicked);
            _menuButton.onClick.AddListener(OnMenuClicked);
        }

        private void OnDisable()
        {
            _respawnButton.onClick.RemoveListener(OnRespawnClicked);
            _menuButton.onClick.RemoveListener(OnMenuClicked);
        }

        public void UpdateTimer(float timeRemaining)
        {
            _timerText.text = $"Respawn in: {timeRemaining:F1}s";
        }

        public void SetRespawnButtonState(bool interactable)
        {
            _respawnButton.interactable = interactable;
        }

        private void OnRespawnClicked() => RespawnClicked?.Invoke();
        private void OnMenuClicked() => MenuClicked?.Invoke();
    }
}