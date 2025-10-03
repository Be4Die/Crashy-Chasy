using System;
using System.Collections.Generic;
using CrashyChasy.Game.Player.Components;
using CrashyChasy.Game.Player.Factory;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;
using UnityEngine;

namespace CrashyChasy.Game.Player
{
    public sealed class ServerPlayerLifeTime : MonoBehaviour
    {
        public event Action<int> OnPlayerCountChanged;
        
        private ServerManager _serverManager;
        private NetworkPlayerFactory _networkPlayerFactory;
        private PlayerSpawnPointsCollection _spawnPoints;
        private MapBorder _mapBorder;
        private NetworkPlayersContainer _networkPlayersContainer;
        
        private readonly Dictionary<NetworkConnection, NetworkPlayerComponent> _players = new ();
        
        public void Construct(ServerManager serverManager, 
            NetworkPlayerFactory networkPlayerFactory, 
            PlayerSpawnPointsCollection spawnPoints,
            MapBorder mapBorder,
            NetworkPlayersContainer networkPlayersContainer)
        {
            _serverManager = serverManager;
            _networkPlayerFactory = networkPlayerFactory;
            _spawnPoints = spawnPoints;
            _mapBorder = mapBorder;
            _networkPlayersContainer = networkPlayersContainer;
        }
        
        private void Start()
        {
            _serverManager.OnRemoteConnectionState += OnRemoteConnectionState;
        }

        private void OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs stateArgs)
        {
            switch (stateArgs.ConnectionState)
            {
                case RemoteConnectionState.Started:
                    HandleClientConnected(conn);
                    break;
                case RemoteConnectionState.Stopped:
                    HandleClientDisconnected(conn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleClientConnected(NetworkConnection conn)
        {
            var player = _networkPlayerFactory.Create(_spawnPoints, SpawnType.Next);
            _serverManager.Spawn(player.NetworkObject, conn);
            _mapBorder.Register(player.transform, player.CarController);
            _players.Add(conn, player);
            _networkPlayersContainer.Register(player);
            OnPlayerCountChanged?.Invoke(_players.Count);
        }

        private void HandleClientDisconnected(NetworkConnection conn)
        {
            if (!conn.IsValid)
            {
                Debug.LogError($"[ServerPlayerLifeTime] Error on HandleClientDisconnected: Player {conn.ClientId} not valid");
                return;
            }
            if (!_players.TryGetValue(conn, out var player))
            {
                Debug.LogError($"[ServerPlayerLifeTime] Error on HandleClientDisconnected: Player {conn.ClientId} has not been connected");
                return;
            }
            
            _serverManager.Despawn(player.NetworkObject);
            _players.Remove(conn);
            _mapBorder.Unregister(player.transform, player.CarController);
            _networkPlayersContainer.Unregister(player);
            OnPlayerCountChanged?.Invoke(_players.Count);
        }

        private void OnDestroy()
        {
            if (_serverManager != null) _serverManager.OnRemoteConnectionState -= OnRemoteConnectionState;
        }
    }
}