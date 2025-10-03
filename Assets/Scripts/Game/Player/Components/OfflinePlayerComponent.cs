using CrashyChasy.Game.Cars.Controllers.Offline;
using UnityEngine;

namespace CrashyChasy.Game.Player.Components
{
    public sealed class OfflinePlayerComponent : PlayerComponent
    {
        [field: SerializeField] public OfflinePlayerCarController CarController { get; private set; }
    }
}