using System;
using Alchemy.Inspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CrashyChasy.Game
{
    [Serializable]
    public class InstantiateFactory<T> where T : Component
    {
        [SerializeField, AssetsOnly] private T _prefab;

        public T Create(SpawnPointsCollection spawnPoints, SpawnType spawnType)
        {
            var point = spawnPoints.GetSpawnPoint(spawnType);
            return Object.Instantiate(_prefab, point.position, point.rotation);
        }
    }
}