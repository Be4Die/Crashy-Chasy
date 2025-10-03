using System.Collections;
using CrashyChasy.Game.Cars.Components;
using FishNet.Object;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Controllers.Network
{
    public abstract class NetworkCarController : NetworkBehaviour, IDamageable, IDestroyable
    {
        protected abstract CarComponent BaseComponent { get; }
        protected abstract SpawnPointsCollection RespawnPoints { get; }
        private bool _isClientTrailsEmitted = false;
        
        protected int _currentHealth;
        protected bool _isDestroyed;
        protected Coroutine _destroyCoroutine;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _currentHealth = BaseComponent.HealthPoints;
        }

        protected virtual void FixedUpdate()
        {
            if (!IsServerStarted) return;
            ApplyMovement();
            ApplyTurn();
        }

        protected abstract void ApplyTurn();

        protected virtual void ApplyMovement()
        {
            BaseComponent.Rigidbody.velocity = transform.forward * BaseComponent.MoveSpeed;
        }

        [Server]
        public void TakeDamage(int damage)
        {
            if (_isDestroyed) return;
            
            _currentHealth -= damage;
            if (_currentHealth <= 0) Destroy();
        }

        [Server]
        public void Destroy()
        {
            if (_isDestroyed) return;
            _isDestroyed = true;
            
            enabled = false;
            BaseComponent.Collider.enabled = false;
            
            DestroyRpc();
            
            StartCoroutine(DestroyRoutine());
        }

        [ObserversRpc]
        private void DestroyRpc()
        {
            OnDestroyRpc();
            HandleClientDriftSound(false);
            SetClientTrailEffect(false);
    
            if (BaseComponent.DestroyEffectPrefab != null)
            {
                Instantiate(
                    BaseComponent.DestroyEffectPrefab, 
                    transform.position, 
                    Quaternion.identity
                );
            }
        }
        
        protected virtual void OnDestroyRpc(){}

        [Server]
        protected virtual IEnumerator DestroyRoutine()
        {
            BaseComponent.Rigidbody.AddForce(
                Vector3.up * BaseComponent.BlowUpForce, 
                ForceMode.Impulse
            );
            
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

        [Client]
        private void SetClientTrailEffect(bool emitting)
        {
            if (BaseComponent.RightTrail != null)
                BaseComponent.RightTrail.emitting = emitting;
            if (BaseComponent.LeftTrail != null)
                BaseComponent.LeftTrail.emitting = emitting;
            
            _isClientTrailsEmitted = emitting;
        }

        [ObserversRpc]
        protected void SetClientDrift(bool isDrifting)
        {
            HandleClientDriftSound(isDrifting);
            SetClientTrailEffect(isDrifting);
        }

        [Client]
        protected virtual void HandleClientDriftSound(bool isDrifting)
        {
        }
        
        [Server]
        public void Respawn()
        {
            if (!_isDestroyed) return;
            _isDestroyed = false;
            _currentHealth = BaseComponent.HealthPoints;
            
            BaseComponent.Rigidbody.velocity = Vector3.zero;
            BaseComponent.Rigidbody.angularVelocity = Vector3.zero;
            
            var spawnPoint = RespawnPoints.GetSpawnPoint(SpawnType.Next);
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            BaseComponent.ModelTransform.localRotation = Quaternion.identity;
            
            BaseComponent.Collider.enabled = true;
            gameObject.SetActive(true);
            enabled = true;
            
            RespawnRpc();
        }

        [ObserversRpc]
        private void RespawnRpc()
        {
            gameObject.SetActive(true);
            SetClientTrailEffect(false);
        }
    }
}