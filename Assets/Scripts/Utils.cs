using ParrelSync;

namespace CrashyChasy
{
    public static class Utils
    {
        public static bool IsServer()
        {
#if UNITY_EDITOR
            return !ClonesManager.IsClone();
#else
            return Application.platform == RuntimePlatform.LinuxServer || 
                   Application.platform == RuntimePlatform.WindowsServer;
#endif
        }
    }
}