using System;
using System.IO;
using Cysharp.Threading.Tasks;
using FishNet.Managing.Server;
using FishNet.Transporting;
using FishNet.Transporting.Bayou;
using UnityEngine;

namespace CrashyChasy.Connection
{
    public sealed class ServerConnectionService : INetworkConnectionService
    {
        [System.Serializable]
        private class ServerConfig
        {
            public int Port = 7777;
            public bool UseSsl;
            public string SslCertPath;
            public string SslCertPassword;
        }
        
        private readonly Transport _transport;
        private readonly ServerManager _serverManager;

        public ServerConnectionService(Transport transport, ServerManager serverManager)
        {
            _transport = transport;
            _serverManager = serverManager;
        }

        public UniTask Connect(IProgress<float> progress = null)
        {
            progress?.Report(0f);
            
            var config = LoadServerConfig();
            progress?.Report(0.3f);
            
            _transport.SetPort((ushort)config.Port);
            
            if (_transport is Bayou bayou && config.UseSsl)
            {
                ConfigureServerSsl(bayou, config);
                progress?.Report(0.6f);
            }
            
            _serverManager.StartConnection();
            progress?.Report(1f);
            
            return UniTask.CompletedTask;
        }

        private static void ConfigureServerSsl(Bayou bayou, ServerConfig config)
        {
            if (!File.Exists(config.SslCertPath))
            {
                Debug.LogError($"SSL certificate not found at: {config.SslCertPath}");
                return;
            }
            
            bayou.SetUseWSS(config.UseSsl);
        
            var sslConfig = new SslConfiguration(
                enabled: true,
                sslProtocols: System.Security.Authentication.SslProtocols.Tls12,
                certPath: config.SslCertPath,
                certPassword: config.SslCertPassword
            );

            var field = typeof(Bayou).GetField("_sslConfiguration", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                field.SetValue(bayou, sslConfig);
                Debug.Log("SSL configuration applied successfully");
            }
            else
            {
                Debug.LogError("Failed to set SSL configuration - field not found");
            }
        }

        private static ServerConfig LoadServerConfig()
        {
            var path = Path.Combine(Application.dataPath, "../server_config.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonUtility.FromJson<ServerConfig>(json);
            }

            Debug.LogWarning("Server config not found. Using defaults.");
            return new ServerConfig();
        }
    }
}