using System.Collections;
using System.Collections.Generic;
using CrashyChasy.Game.Player.Components;

namespace CrashyChasy.Game.Player
{
    public sealed class NetworkPlayersContainer : IEnumerable<NetworkPlayerComponent>
    {
        private readonly HashSet<NetworkPlayerComponent> _players = new();
        
        public void Register(NetworkPlayerComponent component) => _players.Add(component);
        public void Unregister(NetworkPlayerComponent component) => _players.Remove(component);
        
        public IEnumerator<NetworkPlayerComponent> GetEnumerator() => _players.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _players.GetEnumerator();
    }
}