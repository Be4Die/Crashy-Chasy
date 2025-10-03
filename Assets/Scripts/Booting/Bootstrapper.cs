using CrashyChasy.Booting;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

namespace CrashyChasy
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [Inject] private IBootLoader _bootLoader;

        private void Start()
        {
            _bootLoader.Load().Forget();
        }
    }
}
