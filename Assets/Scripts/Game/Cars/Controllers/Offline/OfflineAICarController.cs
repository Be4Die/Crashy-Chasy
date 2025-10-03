using System.Collections;
using CrashyChasy.Game.Bots;
using CrashyChasy.Game.Cars.Components;
using Reflex.Extensions;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Controllers.Offline
{
    public sealed class OfflineAICarController : OfflineCarController
    {
        protected override CarComponent BaseComponent => _component;
        protected override SpawnPointsCollection RespawnPoints => _pointsCollection;

        [SerializeField] private EnemyCarComponent _component;
        private BotsSpawnPointsCollection _pointsCollection;

        
        private Transform _currentTarget;
        private float _lastTargetSearchTime;
        private float _nextWanderTime;
        private float _currentWanderAngle;
        private float _halfWidth;
        private float _halfLength;
        private MapBorder _mapBorder;
        private bool _isAvoidingBorder;

        protected override void ApplyTurn()
        {
            if (_mapBorder == null) return;
            if (Time.time < _lastTargetSearchTime + _component.TargetSearchCooldown) return;
            
            FindTarget();
            _lastTargetSearchTime = Time.time;

            var direction = Vector3.zero;
            var isChasing = _currentTarget != null;

            var avoidanceDir = Vector3.zero;
            var shouldAvoid = ShouldAvoidBorder(out avoidanceDir);

            switch (shouldAvoid)
            {
                case true when !_isAvoidingBorder:
                    _isAvoidingBorder = true;
                    _nextWanderTime = Time.time + _component.WanderDirectionChangeInterval;
                    break;
                case false when _isAvoidingBorder:
                    _isAvoidingBorder = false;
                    _nextWanderTime = Time.time;
                    break;
            }

            if (isChasing)
            {
                direction = (_currentTarget.position - transform.position).normalized;
                direction.y = 0;
                
                if (shouldAvoid && Random.value < _component.AvoidBorderChanceWhenChasing)
                {
                    direction = (direction + avoidanceDir * _component.AvoidBorderForce).normalized;
                }
            }
            else
            {
                if (_isAvoidingBorder)
                {
                    direction = avoidanceDir;
                }
                else
                {
                    if (Time.time > _nextWanderTime)
                    {
                        _currentWanderAngle = Random.Range(-_component.MaxWanderAngle, _component.MaxWanderAngle);
                        _nextWanderTime = Time.time + _component.WanderDirectionChangeInterval;
                    }
                    direction = Quaternion.Euler(0, _currentWanderAngle, 0) * transform.forward;
                }
            }

            if (direction == Vector3.zero) return;
            
            var angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            var turnDirection = angle > 0 ? 1 : -1;
            
            _component.Rigidbody.angularVelocity = transform.up * (turnDirection * _component.TurnSpeed);
        }

        private bool ShouldAvoidBorder(out Vector3 avoidanceDir)
        {
            avoidanceDir = Vector3.zero;
            var pos = transform.position;

            var nearLeft = pos.x < -_halfWidth + _component.BorderAvoidanceRadius;
            var nearRight = pos.x > _halfWidth - _component.BorderAvoidanceRadius;
            var nearBottom = pos.z < -_halfLength + _component.BorderAvoidanceRadius;
            var nearTop = pos.z > _halfLength - _component.BorderAvoidanceRadius;

            var safeLeft = pos.x > -_halfWidth + _component.BorderAvoidanceExitRadius;
            var safeRight = pos.x < _halfWidth - _component.BorderAvoidanceExitRadius;
            var safeBottom = pos.z > -_halfLength + _component.BorderAvoidanceExitRadius;
            var safeTop = pos.z < _halfLength - _component.BorderAvoidanceExitRadius;

            var shouldAvoid = false;

            if (nearLeft || !safeLeft) 
            {
                avoidanceDir += Vector3.right;
                shouldAvoid = true;
            }
            if (nearRight || !safeRight) 
            {
                avoidanceDir += Vector3.left;
                shouldAvoid = true;
            }
            if (nearBottom || !safeBottom) 
            {
                avoidanceDir += Vector3.forward;
                shouldAvoid = true;
            }
            if (nearTop || !safeTop) 
            {
                avoidanceDir += Vector3.back;
                shouldAvoid = true;
            }

            avoidanceDir.Normalize();
            return shouldAvoid;
        }

        private void FindTarget()
        {
            if (_currentTarget != null && !_currentTarget.gameObject.activeSelf)
            {
                _currentTarget = null;
            }

            if (_currentTarget != null) return;
            var player = FindObjectOfType<OfflinePlayerCarController>();
            if (player != null) _currentTarget = player.transform;
        }
        
        protected override IEnumerator DestroyRoutine()
        {
            yield return base.DestroyRoutine();
            yield return new WaitForSeconds(_component.RespawnCooldown);
            Respawn();
        }

        protected override void Awake()
        {
            base.Awake();
            _lastTargetSearchTime = Time.time;
            _mapBorder = gameObject.scene.GetSceneContainer().Resolve<MapBorder>();
            _pointsCollection = gameObject.scene.GetSceneContainer().Resolve<BotsSpawnPointsCollection>();
            _halfWidth = _mapBorder.MapSize.x / 2f;
            _halfLength = _mapBorder.MapSize.y / 2f;
            _nextWanderTime = Time.time;
            _currentWanderAngle = Random.Range(-_component.MaxWanderAngle, _component.MaxWanderAngle);
            _isAvoidingBorder = false;
        }
    }
}