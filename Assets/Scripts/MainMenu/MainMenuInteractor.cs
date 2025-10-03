using CrashyChasy.Game;

namespace CrashyChasy.MainMenu
{
    public sealed class MainMenuInteractor
    {
        private readonly MainMenuEntity _entity;
        private readonly MainMenuRouter _router;

        public MainMenuInteractor(MainMenuEntity entity, MainMenuRouter router)
        {
            _entity = entity;
            _router = router;
        }

        public void ExecuteOnline()
        {
            _router.LoadGameWithParameters(new GameBootParameters(GameMode.Online));
        }

        public void ExecuteOffline()
        {
            _router.LoadGameWithParameters(new GameBootParameters(GameMode.Offline));
        }
    }
}