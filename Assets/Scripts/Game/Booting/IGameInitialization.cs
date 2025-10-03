using Cysharp.Threading.Tasks;

namespace CrashyChasy.Game
{
    public interface IGameInitialization
    {
        public UniTask Initialize(GameBootParameters parameters);
    }
}