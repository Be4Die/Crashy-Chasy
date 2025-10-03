using CrashyChasy.Game.Cars.Components;
using CrashyChasy.Game.Player;
using Reflex.Extensions;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Controllers.Offline
{
    public sealed class OfflinePlayerCarController : OfflineCarController
    {
        protected override CarComponent BaseComponent => _component;
        protected override SpawnPointsCollection RespawnPoints => _pointsCollection;
        
        [SerializeField] private PlayerCarComponent _component;
        
        private PlayerInput _playerInput;
        private int _currentTurnDirection;
        private Game.Camera.CameraController _cameraController;
        private PlayerSpawnPointsCollection _pointsCollection;
        
        private void OnValidate()
        {
            _component ??= GetComponent<PlayerCarComponent>();
        }

        protected override void Awake()
        {
            base.Awake();
            
            _cameraController = gameObject.scene.GetSceneContainer().Resolve<Game.Camera.CameraController>();
            _pointsCollection = gameObject.scene.GetSceneContainer().Resolve<PlayerSpawnPointsCollection>();
            _playerInput = new PlayerInput(_cameraController.Camera);
            _cameraController.SetTarget(transform);
        }

        private void Update()
        {
            var input = _playerInput.Handle();
            _currentTurnDirection = input.IsTurning() ? input.TurnDirection : 0;
        }

        protected override void ApplyTurn()
        {
            if (_currentTurnDirection != 0)
            {
                _component.Rigidbody.angularVelocity = transform.up * (_currentTurnDirection * _component.TurnSpeed);
                
                var currentEuler = _component.ModelTransform.localEulerAngles;
                var currentZ = NormalizeAngle(currentEuler.z);
                var newTilt = currentZ + _currentTurnDirection * _component.Tilt;
                var clampedTilt = Mathf.Clamp(newTilt, -_component.MaxRightTilt, _component.MaxLeftTilt);
                
                _component.ModelTransform.localEulerAngles = new Vector3(
                    currentEuler.x,
                    currentEuler.y,
                    clampedTilt
                );

                SetTrailEffect(true);
                HandleDriftSound(true);
            }
            else
            {
                _component.Rigidbody.angularVelocity = Vector3.Lerp(
                    _component.Rigidbody.angularVelocity,
                    Vector3.zero,
                    5f * Time.fixedDeltaTime
                );
                
                var currentEuler = _component.ModelTransform.localEulerAngles;
                var currentZ = NormalizeAngle(currentEuler.z);
                var newZ = Mathf.Lerp(currentZ, 0f, 5f * Time.fixedDeltaTime);
                
                _component.ModelTransform.localEulerAngles = new Vector3(
                    currentEuler.x,
                    currentEuler.y,
                    newZ
                );

                SetTrailEffect(false);
                HandleDriftSound(false);
            }
        }

        private static float NormalizeAngle(float angle) => angle > 180f ? angle - 360f : angle;

        private void HandleDriftSound(bool isDrifting)
        {
            if (isDrifting) PlayDriftSound();
            else StopDriftSound();
        }

        private void PlayDriftSound()
        {
            if (!_component.DriftAudioSource.isPlaying)
                _component.DriftAudioSource.Play();
        }

        private void StopDriftSound()
        {
            _component.DriftAudioSource.Stop();
        }

        protected override void OnDestroyStarted()
        {
            base.OnDestroyStarted();
            StopDriftSound();
        }

        protected override void OnDestroyCar()
        {
            base.OnDestroyCar();
            _cameraController.enabled = false;
        }
    }
}