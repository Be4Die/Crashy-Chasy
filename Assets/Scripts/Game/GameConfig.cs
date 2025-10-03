using System;
using UnityEngine;

namespace CrashyChasy.Game
{
    [Serializable]
    public sealed class GameConfig
    {
        [field: SerializeField] public int MaxBots { get; private set; }
    }
}