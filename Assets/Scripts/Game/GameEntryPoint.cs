using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

namespace CrashyChasy.Game
{
    public sealed class GameEntryPoint : MonoBehaviour
    {
        [Inject] private readonly GameBootParameters _gameBootParameters;
        [Inject] private readonly IGameInitialization _gameInitialization;
    
        public void Start()
        {
            _gameInitialization.Initialize(_gameBootParameters).Forget();
        }
    }
}