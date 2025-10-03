using CrashyChasy.Game.Cars.Components;
using UnityEngine;

namespace CrashyChasy.Game.Bots.Components
{
    public abstract class BotComponent : MonoBehaviour
    {
        [field: SerializeField] public EnemyCarComponent CarComponent { get; private set; }
    }
}