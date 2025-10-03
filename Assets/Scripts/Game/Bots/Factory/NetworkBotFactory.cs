using System;
using CrashyChasy.Game.Bots.Components;

namespace CrashyChasy.Game.Bots.Factory
{
    [Serializable]
    public sealed class NetworkBotFactory : InstantiateFactory<NetworkBotComponent> {}
}