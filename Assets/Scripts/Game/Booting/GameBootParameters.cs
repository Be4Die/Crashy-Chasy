using System;
using UnityEngine;

namespace CrashyChasy.Game
{
    [Serializable]
    public class GameBootParameters
    {
        [field: SerializeField] public GameMode Mode { get; set; }

        public GameBootParameters(GameMode mode)
        {
            Mode = mode;
        }
    }
}