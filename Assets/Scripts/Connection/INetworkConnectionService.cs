using System;
using Cysharp.Threading.Tasks;

namespace CrashyChasy.Connection
{
    public interface INetworkConnectionService
    {
        public UniTask Connect(IProgress<float> progress = null);
    }
}