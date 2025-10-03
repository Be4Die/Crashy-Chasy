using CrashyChasy.Connection;
using CrashyChasy.Game.Bots;
using CrashyChasy.Game.Bots.Components;
using CrashyChasy.Game.Bots.Factory;
using CrashyChasy.Game.Player;
using CrashyChasy.Game.Player.Factory;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using UnityEngine;

namespace CrashyChasy.Game
{
    public sealed class ServerGameInitialization : IGameInitialization
    {
        private readonly INetworkConnectionService _connectionService;
        private readonly ServerManager _serverManager;
        private readonly NetworkPlayerFactory _networkPlayerFactory;
        private readonly PlayerSpawnPointsCollection _spawnPoints;
        private readonly MapBorder _mapBorder;
        private readonly NetworkPlayersContainer _networkPlayersContainer;
        private readonly GameConfig _gameConfig;
        private readonly BotsContainer<NetworkBotComponent> _bots;
        private readonly NetworkBotFactory _factory;
        private readonly BotsSpawnPointsCollection _botSpawnPoints;

        public ServerGameInitialization(
            INetworkConnectionService connectionService, 
            ServerManager serverManager, 
            NetworkPlayerFactory networkPlayerFactory, 
            PlayerSpawnPointsCollection spawnPoints, 
            MapBorder mapBorder, 
            NetworkPlayersContainer networkPlayersContainer, 
            GameConfig gameConfig, 
            BotsContainer<NetworkBotComponent> bots, 
            NetworkBotFactory factory, 
            BotsSpawnPointsCollection botSpawnPoints)
        {
            _connectionService = connectionService;
            _serverManager = serverManager;
            _networkPlayerFactory = networkPlayerFactory;
            _spawnPoints = spawnPoints;
            _mapBorder = mapBorder;
            _networkPlayersContainer = networkPlayersContainer;
            _gameConfig = gameConfig;
            _bots = bots;
            _factory = factory;
            _botSpawnPoints = botSpawnPoints;
        }

        public async UniTask Initialize(GameBootParameters parameters)
        {
            await _connectionService.Connect();
            var obj = new GameObject("ServerLifeTime");
            var players = obj.AddComponent<ServerPlayerLifeTime>();
            players.Construct(
                _serverManager, 
                _networkPlayerFactory, 
                _spawnPoints, 
                _mapBorder, 
                _networkPlayersContainer
            );
            obj.AddComponent<ServerBotLifeTime>()
                .Construct(
                    players,
                    _serverManager,
                    _gameConfig.MaxBots,
                    _factory,
                    _bots,
                    _mapBorder,
                    _botSpawnPoints
            );
        }
    }
}