using Alchemy.Inspector;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Components
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class CarComponent : MonoBehaviour
    {
        [field: Title("Base Configuration")]
        [field: SerializeField] public float MoveSpeed { get; private set; } = 25f;
        [field: SerializeField] public float TurnSpeed { get; private set; } = 10f;
        [field: Header("Health")]
        [field: SerializeField] public int HealthPoints { get; private set; } = 2;
        [field: Header("Destroy Settings")]
        [field: SerializeField] public float BlowUpForce { get; private set; } = 100f;
        [field: SerializeField] public float BlowRotateSpeed { get; private set; } = 200f;
        [field: SerializeField] public float DestroyDuration { get; private set; } = 2f;
        [field: SerializeField] public GameObject DestroyEffectPrefab { get; private set; }
        [field: Title("References")]
        [field: SerializeField] public Transform ModelTransform { get; private set; }
        [field: Header("Trails")]
        [field: SerializeField] public TrailRenderer LeftTrail { get; private set; }
        [field: SerializeField] public TrailRenderer RightTrail { get; private set; }
        [field: Header("Components")]
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }

        private void OnValidate()
        {
            Rigidbody ??= GetComponent<Rigidbody>();
            Collider ??= GetComponent<Collider>();
        }
    }
}