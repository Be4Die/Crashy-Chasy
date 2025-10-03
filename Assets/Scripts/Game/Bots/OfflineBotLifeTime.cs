using System.Collections;
using CrashyChasy.Game.Bots.Components;
using CrashyChasy.Game.Bots.Factory;
using CrashyChasy.Game.Player;
using UnityEngine;

namespace CrashyChasy.Game.Bots
{
    public sealed class OfflineBotLifeTime : MonoBehaviour
    {
        private OfflineBotFactory _factory;
        private BotsContainer<OfflineBotComponent> _bots;
        private MapBorder _border;
        private BotsSpawnPointsCollection _spawnPoints;
        private int _maxBots;
        
        private readonly System.Collections.Generic.List<OfflineBotComponent> _spawnedBots = new();

        public void Construct(
            OfflineBotFactory factory,
            BotsContainer<OfflineBotComponent> bots,
            MapBorder border,
            BotsSpawnPointsCollection spawnPoints,
            int maxBots)
        {
            _factory = factory;
            _bots = bots;
            _border = border;
            _spawnPoints = spawnPoints;
            _maxBots = maxBots;
        }

        private void Start()
        {
            StartCoroutine(SpawnBotsWithDelay());
        }

        private IEnumerator SpawnBotsWithDelay()
        {
            yield return null;
            
            var botsToSpawn = _maxBots - _spawnedBots.Count;
            for (var i = 0; i < botsToSpawn; i++)
            {
                SpawnBot();
                yield return new WaitForSeconds(1f);
            }
        }

        private void SpawnBot()
        {
            var bot = _factory.Create(_spawnPoints, SpawnType.Random);
            _spawnedBots.Add(bot);
            _bots.Register(bot);
            _border.Register(bot.transform, bot.CarController);
        }

        private void OnDestroy()
        {
            foreach (var bot in _spawnedBots)
            {
                if (bot == null) continue;
                _bots.Unregister(bot);
                _border.Unregister(bot.transform, bot.CarController);
                Destroy(bot.gameObject);
            }
            _spawnedBots.Clear();
        }
    }
}