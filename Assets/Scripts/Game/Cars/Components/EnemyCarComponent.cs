using UnityEngine;

namespace CrashyChasy.Game.Cars.Components
{
    public sealed class EnemyCarComponent : CarComponent
    {
        [field: Header("AI Settings")]
        [field: SerializeField] public float DetectionRadius { get; private set; } = 30f;
        [field: SerializeField, Range(0f, 1f)] public float FollowBotChance { get; private set; } = 0.3f;
        [field: SerializeField] public float TargetSearchCooldown { get; private set; } = 1f;
        [field: SerializeField] public float RespawnCooldown { get; private set; } = 5f;
        [field: Header("Border Avoidance")]
        [field: SerializeField] public float BorderAvoidanceRadius { get; private set; } = 20f;
        [field: SerializeField] public float BorderAvoidanceExitRadius { get; private set; } = 30f;
        [field: SerializeField, Range(0f, 1f)] public float AvoidBorderChanceWhenChasing { get; private set; } = 0.7f;
        [field: SerializeField, Range(1f, 5f)] public float AvoidBorderForce { get; private set; } = 2f;
        
        [field: Header("Wandering")]
        [field: SerializeField] public float WanderDirectionChangeInterval { get; private set; } = 3f;
        [field: SerializeField] public float MaxWanderAngle { get; private set; } = 45f;
    }
}