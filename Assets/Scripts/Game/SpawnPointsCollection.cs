using System;
using Alchemy.Inspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashyChasy.Game
{
    [Serializable]
    public abstract class SpawnPointsCollection
    {
        [SerializeField, InlineEditor] private Transform[] _points;
        private int _index;

        public Transform GetSpawnPoint(SpawnType spawnType) => spawnType switch
        {
            SpawnType.Next => GetNext(),
            SpawnType.Random => GetRandom(),
            _ => throw new NotImplementedException()
        };

        private Transform GetRandom() => _points[Random.Range(0, _points.Length)];

        private Transform GetNext()
        {
            var point = _points[_index];
            _index = (_index + 1) % _points.Length;
            return point;
        }
    }
}