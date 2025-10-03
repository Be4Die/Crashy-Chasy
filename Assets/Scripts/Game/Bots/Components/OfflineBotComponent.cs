using CrashyChasy.Game.Cars.Controllers.Offline;
using UnityEngine;

namespace CrashyChasy.Game.Bots.Components
{
    public sealed class OfflineBotComponent : BotComponent
    {
        [field: SerializeField] public OfflineAICarController CarController { get; private set; }
    }
}