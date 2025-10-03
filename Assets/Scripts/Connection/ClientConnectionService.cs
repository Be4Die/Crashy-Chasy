using System;
using System.IO;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Client;
using FishNet.Transporting;
using FishNet.Transporting.Bayou;
using UnityEngine;
using UnityEngine.Networking;

namespace CrashyChasy.Connection
{
    public sealed class ClientConnectionService : INetworkConnectionService
    {
        [Serializable]
        private class ClientConfig
        {
            public string Address = "localhost";
            public int Port = 7777;
            public bool UseWss;
        }
        
        private readonly ClientManager _clientManager;
        private readonly Transport _transport;
        private static ClientConfig _cachedConfig;
        private UniTaskCompletionSource _connectionTcs;

        public ClientConnectionService(ClientManager clientManager, Transport transport)
        {
            _clientManager = clientManager;
            _transport = transport;
        }

        public async UniTask Connect(IProgress<float> progress = null)
        {
            progress?.Report(0f);

            if (_cachedConfig == null)
            {
                _cachedConfig = await LoadClientConfig();
                progress?.Report(0.1f);
                
                if (_transport is Bayou bayou)
                {
                    bayou.SetUseWSS(_cachedConfig.UseWss);
                }
            }
            else
            {
                progress?.Report(0.1f);
            }

            if (_connectionTcs != null && !_connectionTcs.Task.Status.IsCompleted())
            {
                _connectionTcs.TrySetCanceled();
            }

            _connectionTcs = new UniTaskCompletionSource();
            _clientManager.OnClientConnectionState += HandleClientConnectionState;

            try
            {
                _clientManager.StartConnection(_cachedConfig.Address, (ushort)_cachedConfig.Port);
                progress?.Report(0.2f);

                for (var p = 0.2f; p < 0.9f; p += 0.05f)
                {
                    if (_connectionTcs.Task.Status.IsCompleted()) break;
                    progress?.Report(p);
                    await UniTask.Delay(100);
                }

                await _connectionTcs.Task.Timeout(TimeSpan.FromSeconds(10));
                progress?.Report(1f);
            }
            catch
            {
                progress?.Report(1f);
                throw;
            }
            finally
            {
                _clientManager.OnClientConnectionState -= HandleClientConnectionState;
            }
        }


        private void HandleClientConnectionState(ClientConnectionStateArgs args)
        {
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Started:
                    _connectionTcs.TrySetResult();
                    break;
                case LocalConnectionState.Stopped:
                    _connectionTcs.TrySetException(
                        new Exception($"Connection failed")
                    );
                    break;
                case LocalConnectionState.Stopping:
                case LocalConnectionState.Starting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static UniTask<ClientConfig> LoadClientConfig()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "client_config.json");
            return Application.platform == RuntimePlatform.WebGLPlayer ? 
                LoadWebConfig(path) : 
                UniTask.FromResult(LoadLocalConfig(path));
        }

        private static async UniTask<ClientConfig> LoadWebConfig(string url)
        {
            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
                return JsonUtility.FromJson<ClientConfig>(request.downloadHandler.text);
            
            Debug.LogWarning($"Client config load error: {request.error}");
            return new ClientConfig();
        }

        private static ClientConfig LoadLocalConfig(string path)
        {
            if (File.Exists(path))
                return JsonUtility.FromJson<ClientConfig>(File.ReadAllText(path));
            
            Debug.LogWarning("Client config not found. Using defaults.");
            return new ClientConfig();
        }
    }
}