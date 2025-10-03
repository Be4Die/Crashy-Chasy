using System.Collections.Generic;
using UnityEngine;

namespace CrashyChasy.Game
{
    // TODO: Remove manual registration => use container
    public sealed class MapBorder : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MapSize = new(900f, 900f);
        private readonly List<DestroyableObject> _destroyables = new();

        private void FixedUpdate()
        {
            var halfWidth = MapSize.x / 2f;
            var halfLength = MapSize.y / 2f;
        
            for (var i = _destroyables.Count - 1; i >= 0; i--)
            {
                var obj = _destroyables[i];
                
                if (obj.Transform == null)
                {
                    _destroyables.RemoveAt(i);
                    continue;
                }

                var pos = obj.Transform.position;
                var needsDestroy = pos.x < -halfWidth || pos.x > halfWidth || 
                                   pos.z < -halfLength || pos.z > halfLength;

                if (!needsDestroy) continue;
                obj.Destroyable.Destroy();
            }
        }

        public void Register(Transform point, IDestroyable destroyable)
        {
            _destroyables.Add(new DestroyableObject(point, destroyable));
        }

        public void Unregister(Transform point, IDestroyable destroyable)
        {
            _destroyables.RemoveAll(x => 
                x.Transform == point && 
                x.Destroyable == destroyable);
        }

        private struct DestroyableObject
        {
            public readonly Transform Transform;
            public readonly IDestroyable Destroyable;

            public DestroyableObject(Transform transform, IDestroyable destroyable)
            {
                Transform = transform;
                Destroyable = destroyable;
            }
        }
    }
}