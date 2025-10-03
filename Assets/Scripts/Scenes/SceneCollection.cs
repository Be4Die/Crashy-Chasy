using System;
using UnityEngine;

namespace CrashyChasy.Scenes
{
    [Serializable]
    public class SceneCollection
    {
        [field: SerializeField] public string MainMenuSceneName { get; private set; } = "MainMenu";
        [field: SerializeField] public string GameSceneName { get; private set; } = "Game";
    }
}