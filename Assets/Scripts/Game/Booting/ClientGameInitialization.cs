using System;
using CrashyChasy.Connection;
using CrashyChasy.Game.Bots;
using CrashyChasy.Game.Bots.Components;
using CrashyChasy.Game.Bots.Factory;
using CrashyChasy.Game.Player;
using CrashyChasy.Game.Player.Factory;
using CrashyChasy.LoadingScreen;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrashyChasy.Game
{
    public sealed class ClientGameInitialization : IGameInitialization
    {
        private readonly INetworkConnectionService _connectionService;
        private readonly LoadingScreenController _loadingScreen;
        private readonly OfflinePlayerFactory _offlinePlayerFactory;
        private readonly PlayerSpawnPointsCollection _spawnPointsCollection;
        private readonly MapBorder _mapBorder;
        private readonly OfflineBotFactory _offlineBotFactory;
        private readonly BotsContainer<OfflineBotComponent> _offlineBots;
        private readonly BotsSpawnPointsCollection _botSpawnPointsCollection;
        private readonly GameConfig _gameConfig;

        public ClientGameInitialization(
            INetworkConnectionService connectionService,
            LoadingScreenController loadingScreen, 
            OfflinePlayerFactory offlinePlayerFactory, 
            PlayerSpawnPointsCollection spawnPointsCollection, 
            MapBorder mapBorder,
            OfflineBotFactory offlineBotFactory,
            BotsContainer<OfflineBotComponent> offlineBots,
            BotsSpawnPointsCollection botSpawnPointsCollection,
            GameConfig gameConfig)
        {
            _connectionService = connectionService;
            _loadingScreen = loadingScreen;
            _offlinePlayerFactory = offlinePlayerFactory;
            _spawnPointsCollection = spawnPointsCollection;
            _mapBorder = mapBorder;
            _offlineBotFactory = offlineBotFactory;
            _offlineBots = offlineBots;
            _botSpawnPointsCollection = botSpawnPointsCollection;
            _gameConfig = gameConfig;
        }

        public async UniTask Initialize(GameBootParameters parameters)
        {
            _loadingScreen.Show();
            
            try
            {
                var initOperation = new LoadingOperation
                {
                    Name = parameters.Mode == GameMode.Online 
                        ? "Подключение к серверу" 
                        : "Локальная инициализация",
                    Action = async progressReporter =>
                    {
                        if (parameters.Mode == GameMode.Online)
                        {
                            await _connectionService.Connect(progressReporter);
                        }
                        else
                        {
                            for (float i = 0; i < 1; i += 0.1f)
                            {
                                progressReporter.Report(i);
                                await UniTask.Delay(100);
                            }
                        }
                    }
                };

                await _loadingScreen.LoadOperations(
                    groupWeight: 0.5f,
                    initOperation
                );
            }
            catch
            {
                Debug.LogError("Ошибка подключения. Переход в оффлайн режим");
                parameters.Mode = GameMode.Offline;
            }
            finally
            {
                switch (parameters.Mode)
                {
                    case GameMode.Offline:
                        var player = _offlinePlayerFactory.Create(_spawnPointsCollection, SpawnType.Random);
                        _mapBorder.Register(player.transform, player.CarController);
                        var botLifeTimeObject = new GameObject("OfflineBotLifeTime");
                        var botLifeTime = botLifeTimeObject.AddComponent<OfflineBotLifeTime>();
                        botLifeTime.Construct(
                            _offlineBotFactory,
                            _offlineBots,
                            _mapBorder,
                            _botSpawnPointsCollection,
                            _gameConfig.MaxBots
                        );
                        break;
                    case GameMode.Online:
                        _mapBorder.enabled = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                await UniTask.Delay(500);
                _loadingScreen.Hide();
            }
        }
    }
}