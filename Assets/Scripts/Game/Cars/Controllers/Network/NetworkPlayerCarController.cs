using CrashyChasy.Game.Cars.Components;
using CrashyChasy.Game.Player;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Reflex.Extensions;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Controllers.Network
{
    public sealed class NetworkPlayerCarController : NetworkCarController
    {
        protected override CarComponent BaseComponent => _component;
        protected override SpawnPointsCollection RespawnPoints => _pointsCollection;

        [SerializeField] private PlayerCarComponent _component;
        private PlayerInput _playerInput;
        private Game.Camera.CameraController _cameraController;
        private PlayerSpawnPointsCollection _pointsCollection;
        
        private readonly SyncVar<int> _syncTurnDirection = new (
            new SyncTypeSettings(WritePermission.ClientUnsynchronized, ReadPermission.ExcludeOwner)
        );

        protected override void OnValidate()
        {
            _component ??= GetComponent<PlayerCarComponent>();
        }
        
        [ServerRpc]
        public void RequestRespawnServerRpc()
        {
            if (_isDestroyed) Respawn();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (!IsOwner) return;
            _cameraController = gameObject.scene.GetSceneContainer().Resolve<Game.Camera.CameraController>();
            _playerInput = new PlayerInput(_cameraController.Camera);
            _cameraController.SetTarget(transform);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            _pointsCollection = gameObject.scene.GetSceneContainer().Resolve<PlayerSpawnPointsCollection>();
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (Time.frameCount % 10 != 0) return;
            
            var input = _playerInput.Handle();
            if (input.TurnDirection != _syncTurnDirection.Value)
            {
                SetInput(input.TurnDirection);
            }
        }
        
        [ServerRpc(RunLocally = true)] 
        private void SetInput(int direction) => _syncTurnDirection.Value = direction;

        protected override void ApplyTurn()
        {
            if (_syncTurnDirection.Value != 0)
            {
                _component.Rigidbody.angularVelocity = transform.up * (_syncTurnDirection.Value * _component.TurnSpeed);
                
                var currentEuler = _component.ModelTransform.localEulerAngles;
                var currentZ = NormalizeAngle(currentEuler.z);
                var newTilt = currentZ + _syncTurnDirection.Value * _component.Tilt;
                var clampedTilt = Mathf.Clamp(newTilt, -_component.MaxRightTilt, _component.MaxLeftTilt);
                
                _component.ModelTransform.localEulerAngles = new Vector3(
                    currentEuler.x,
                    currentEuler.y,
                    clampedTilt
                );

                SetClientDrift(true);
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

                SetClientDrift(false);
            }
        }
        
        private static float NormalizeAngle(float angle) => angle > 180f ? angle - 360f : angle;

        [Client]
        protected override void HandleClientDriftSound(bool isDrifting)
        {
            if (isDrifting)
            {
                if (!_component.DriftAudioSource.isPlaying)
                    _component.DriftAudioSource.Play();
            }
            else
            {
                _component.DriftAudioSource.Stop();
            }
        }

        protected override void OnDestroyRpc()
        {
            base.OnDestroyRpc();
            _cameraController.enabled = false;
        }
    }
}