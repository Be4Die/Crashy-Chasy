using CrashyChasy.Game.Cars.Controllers.Network;
using FishNet.Object;
using UnityEngine;

namespace CrashyChasy.Game.Bots.Components
{
    public class NetworkBotComponent : BotComponent
    {
        [field: SerializeField] public NetworkObject NetworkObject { get; private set; }
        [field: SerializeField] public NetworkAICarController CarController { get; private set; }
    }
}