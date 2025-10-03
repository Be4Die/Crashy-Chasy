using System;
using UnityEngine;

namespace CrashyChasy.Game.Camera
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public UnityEngine.Camera Camera { get; private set; }
        [SerializeField] private float _smoothing = 5f;
        [SerializeField] private Vector3 _offset;
        private Transform _playerTransform;


        public void SetTarget(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        private void OnValidate()
        {
            Camera ??= GetComponent<UnityEngine.Camera>();
        }

        private void LateUpdate()
        {
            FollowTarget();
        }
        

        private void FollowTarget()
        {
            if (_playerTransform == null) return;

            var targetPosition = _playerTransform.position + _offset;
            transform.position = Vector3.Lerp(
                transform.position, 
                targetPosition, 
                _smoothing * Time.deltaTime
            );
        }
    }
}