using System.Collections;
using System.Collections.Generic;
using CrashyChasy.Game.Bots.Components;
using CrashyChasy.Game.Bots.Factory;
using CrashyChasy.Game.Player;
using FishNet.Managing.Server;
using UnityEngine;

namespace CrashyChasy.Game.Bots
{
    public sealed class ServerBotLifeTime : MonoBehaviour
    {
        private ServerPlayerLifeTime _playerLifeTime;
        private ServerManager _serverManager;
        private int _maxPlayers;
        private BotsContainer<NetworkBotComponent> _bots;
        private NetworkBotFactory _factory;
        private MapBorder _border;
        private BotsSpawnPointsCollection _spawnPoints;
        
        private readonly List<NetworkBotComponent> _spawnedBots = new();
        private int _lastPlayerCount;
        
        private int _botsToSpawn;
        private Coroutine _spawningCoroutine;

        public void Construct(
            ServerPlayerLifeTime playerLifeTime, 
            ServerManager serverManager, 
            int maxPlayers,
            NetworkBotFactory factory,
            BotsContainer<NetworkBotComponent> bots,
            MapBorder border,
            BotsSpawnPointsCollection spawnPoints)
        {
            _playerLifeTime = playerLifeTime;
            _serverManager = serverManager;
            _maxPlayers = maxPlayers;
            _factory = factory;
            _bots = bots;
            _border = border;
            _spawnPoints = spawnPoints;
        }

        private void Start()
        {
            _playerLifeTime.OnPlayerCountChanged += OnPlayerCountChanged;
        }

        private void OnPlayerCountChanged(int playerCount)
        {
            var targetBotCount = CalculateTargetBotCount(playerCount);
            
            while (_spawnedBots.Count > targetBotCount)
            {
                DestroyBot(_spawnedBots[0]);
            }
            
            _botsToSpawn = targetBotCount - _spawnedBots.Count;
            
            if (_botsToSpawn > 0 && _spawningCoroutine == null)
            {
                _spawningCoroutine = StartCoroutine(SpawnBotsWithDelay());
            }
        }
        
        private IEnumerator SpawnBotsWithDelay()
        {
            while (_botsToSpawn > 0)
            {
                SpawnBot();
                _botsToSpawn--;
                yield return new WaitForSeconds(1f);
            }
            
            _spawningCoroutine = null;
        }

        private int CalculateTargetBotCount(int playerCount)
        {
            return playerCount == 0 ? 0 : Mathf.Max(0, _maxPlayers - playerCount);
        }

        private void SpawnBot()
        {
            var bot = _factory.Create(_spawnPoints, SpawnType.Random);
            _serverManager.Spawn(bot.NetworkObject);
            
            _spawnedBots.Add(bot);
            _bots.Register(bot);
            _border.Register(bot.transform, bot.CarController);
        }

        private void DestroyBot(NetworkBotComponent bot)
        {
            if (bot == null) return;
            
            _serverManager.Despawn(bot.NetworkObject);
            _spawnedBots.Remove(bot);
            _bots.Unregister(bot);            
            _border.Unregister(bot.transform, bot.CarController);
        }

        private void OnDestroy()
        {
            if (_playerLifeTime != null)
                _playerLifeTime.OnPlayerCountChanged -= OnPlayerCountChanged;

            if (_spawningCoroutine == null) return;
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }
}