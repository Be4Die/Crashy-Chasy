using CrashyChasy.Game.Cars.Controllers.Network;
using FishNet.Object;
using UnityEngine;

namespace CrashyChasy.Game.Player.Components
{
    public sealed class NetworkPlayerComponent : PlayerComponent
    {
        [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
        [field: SerializeField] public NetworkPlayerCarController CarController { get; private set; }
    }
}