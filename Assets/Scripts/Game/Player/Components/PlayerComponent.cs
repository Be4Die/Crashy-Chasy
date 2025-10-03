using CrashyChasy.Game.Cars.Components;
using UnityEngine;

namespace CrashyChasy.Game.Player.Components
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        [field: SerializeField] public PlayerCarComponent Car { get; private set; }
    }
}