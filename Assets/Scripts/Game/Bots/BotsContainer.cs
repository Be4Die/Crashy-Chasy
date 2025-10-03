using System.Collections;
using System.Collections.Generic;
using CrashyChasy.Game.Bots.Components;

namespace CrashyChasy.Game.Bots
{
    public sealed class BotsContainer<T> : IEnumerable<T> where T : BotComponent
    {
        private readonly HashSet<T> _bots = new ();
        
        public void Register(T bot) => _bots.Add(bot);
        public void Unregister(T bot) => _bots.Remove(bot);
        
        public IEnumerator<T> GetEnumerator() => _bots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}