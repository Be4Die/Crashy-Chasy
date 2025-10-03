using System;
using Cysharp.Threading.Tasks;

namespace CrashyChasy.LoadingScreen
{
    public sealed class LoadingOperation
    {
        public string Name { get; set; }
        public Func<IProgress<float>, UniTask> Action { get; set; }
        public float Weight { get; set; } = 1f;
    }
}