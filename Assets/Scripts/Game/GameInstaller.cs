using Alchemy.Inspector;
using CrashyChasy.Game.Bots;
using CrashyChasy.Game.Bots.Components;
using CrashyChasy.Game.Bots.Factory;
using CrashyChasy.Game.Camera;
using CrashyChasy.Game.Player;
using CrashyChasy.Game.Player.Factory;
using Reflex.Core;
using UnityEngine;

namespace CrashyChasy.Game
{
    public sealed class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField, InlineEditor] private GameBootParameters _gameBootParameters;
        [SerializeField, InlineEditor] private GameConfig _gameConfig;
        [SerializeField] private PlayerSpawnPointsCollection _playerSpawnPointsCollection;
        [SerializeField] private NetworkPlayerFactory _networkPlayerFactory;
        [SerializeField] private OfflinePlayerFactory _offlinePlayerFactory;
        [SerializeField] private BotsSpawnPointsCollection _botSpawnPointsCollection;
        [SerializeField] private NetworkBotFactory _networkBotFactory;
        [SerializeField] private OfflineBotFactory _offlineBotFactory;
        [SerializeField, AssetsOnly] private CameraController _cameraController;
        [SerializeField, AssetsOnly] private MapBorder _mapBorder;

        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            if (!containerBuilder.HasBinding(typeof(GameBootParameters)))
            {
                containerBuilder.AddSingleton(_gameBootParameters);
            }
            
            containerBuilder.AddSingleton(new NetworkPlayersContainer());
            containerBuilder.AddSingleton(_playerSpawnPointsCollection);
            containerBuilder.AddSingleton(_networkPlayerFactory);
            containerBuilder.AddSingleton(_offlinePlayerFactory);
            containerBuilder.AddSingleton(_botSpawnPointsCollection);
            containerBuilder.AddSingleton(_networkBotFactory);
            containerBuilder.AddSingleton(_offlineBotFactory);
            containerBuilder.AddSingleton(Utils.IsServer() ? typeof(ServerGameInitialization) : typeof(ClientGameInitialization), typeof(IGameInitialization));

            containerBuilder.AddSingleton(Instantiate(_cameraController));
            containerBuilder.AddSingleton(new BotsContainer<NetworkBotComponent>());
            containerBuilder.AddSingleton(new BotsContainer<OfflineBotComponent>());
            containerBuilder.AddSingleton(_gameConfig);
            containerBuilder.AddSingleton(Instantiate(_mapBorder));
        }
    }
}