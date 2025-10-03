using System;
using System.Collections;
using CrashyChasy.Game.Cars.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrashyChasy.Game.Cars.Controllers.Offline
{
    public abstract class OfflineCarController : MonoBehaviour, IDamageable, IDestroyable
    {
        protected abstract CarComponent BaseComponent { get; }
        protected abstract SpawnPointsCollection RespawnPoints { get; }

        protected int _currentHealth;
        protected bool _isDestroyed;
        protected Coroutine _destroyCoroutine;
        private bool _isTrailsEmitted;

        protected virtual void Awake()
        {
            _currentHealth = BaseComponent.HealthPoints;
            _isDestroyed = false;
        }
        
        protected virtual void FixedUpdate()
        {
            ApplyMovement();
            ApplyTurn();
        }
        
        public void Respawn()
        {
            if (!_isDestroyed) return;
            _isDestroyed = false;

            if (_destroyCoroutine != null)
            {
                StopCoroutine(_destroyCoroutine);
                _destroyCoroutine = null;
            }
            
            _currentHealth = BaseComponent.HealthPoints;
            BaseComponent.Collider.enabled = true;
            enabled = true;
            gameObject.SetActive(true);
        
            var spawnPoint = RespawnPoints.GetSpawnPoint(SpawnType.Next);
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            BaseComponent.Rigidbody.velocity = Vector3.zero;
            BaseComponent.Rigidbody.angularVelocity = Vector3.zero;
            BaseComponent.ModelTransform.localRotation = Quaternion.identity;

            SetTrailEffect(false);
            OnRespawn();
        }

        protected virtual void OnRespawn() { }

        protected abstract void ApplyTurn();

        protected virtual void ApplyMovement()
        {
            BaseComponent.Rigidbody.velocity = transform.forward * BaseComponent.MoveSpeed;
        }

        public void TakeDamage(int damage)
        {
            if (_isDestroyed) return;
            
            _currentHealth -= damage;
            if (_currentHealth <= 0) Destroy();
        }

        public void Destroy()
        {
            if (_isDestroyed) return;
            _isDestroyed = true;
            
            if (_destroyCoroutine != null) return;
            OnDestroyCar();
            _destroyCoroutine = StartCoroutine(DestroyRoutine());
        }

        protected virtual void OnDestroyCar() { }

        protected virtual IEnumerator DestroyRoutine()
        {
            enabled = false;
            
            BaseComponent.Rigidbody.AddForce(
                Vector3.up * BaseComponent.BlowUpForce, 
                ForceMode.Impulse
            );
            
            BaseComponent.Collider.enabled = false;
            SetTrailEffect(false);
            OnDestroyStarted();

            if (BaseComponent.DestroyEffectPrefab != null)
            {
                Instantiate(
                    BaseComponent.DestroyEffectPrefab, 
                    transform.position, 
                    Quaternion.identity
                );
            }

            var randomRotation = Random.insideUnitSphere;
            var elapsed = 0f;
            
            while (elapsed < BaseComponent.DestroyDuration)
            {
                BaseComponent.ModelTransform.Rotate(
                    randomRotation * (BaseComponent.BlowRotateSpeed * Time.deltaTime), 
                    Space.Self
                );
                
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        protected virtual void OnDestroyStarted() { }
        
        protected void SetTrailEffect(bool emitting)
        {
            if (BaseComponent.LeftTrail != null)
                BaseComponent.LeftTrail.emitting = emitting;
            if (BaseComponent.RightTrail != null)
                BaseComponent.RightTrail.emitting = emitting;
            _isTrailsEmitted = emitting;
        }
    }
}