using Alchemy.Inspector;
using UnityEngine;

namespace CrashyChasy.Game.Cars.Components
{
    public sealed class PlayerCarComponent : CarComponent
    {
        [field: Title("Player Configuration")]
        [field: Header("Tilt")]
        [field: SerializeField] public float Tilt { get; private set; } = 3f;
        [field: SerializeField] public float MaxLeftTilt { get; private set; } = 7f;
        [field: SerializeField] public float MaxRightTilt { get; private set; } = 8f;
        [field:Header("Grenade")]
        [field: SerializeField] public GameObject GrenadePrefab { get; private set; }
        [field: SerializeField] public int GrenadeDetonateCount { get; private set; }
        [field: Title("References")]
        [field: SerializeField] public AudioSource DriftAudioSource { get; private set; }
    }
}