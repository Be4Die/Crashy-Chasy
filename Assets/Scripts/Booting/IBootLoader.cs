using Cysharp.Threading.Tasks;

namespace CrashyChasy.Booting
{
    public interface IBootLoader
    {
        public UniTask Load();
    }
}