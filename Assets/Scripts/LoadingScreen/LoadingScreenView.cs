using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrashyChasy.LoadingScreen
{
    public sealed class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _statusText;

        public void SetVisible(bool visible) => _canvas.enabled = visible;
        public void SetProgress(float progress) => _progressBar.value = progress;
        public void SetStatus(string text) => _statusText.text = text;
    }
}