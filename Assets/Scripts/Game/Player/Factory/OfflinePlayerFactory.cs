using System;
using CrashyChasy.Game.Player.Components;

namespace CrashyChasy.Game.Player.Factory
{
    [Serializable]
    public sealed class OfflinePlayerFactory : InstantiateFactory<OfflinePlayerComponent> { }
}