namespace CrashyChasy.Game.DeathScreen
{
    public sealed class DeathScreenInteractor
    {
        private readonly DeathScreenEntity _entity;
        private readonly DeathScreenRouter _router;
        private readonly GameMode _gameMode;

        public DeathScreenInteractor(DeathScreenEntity entity, DeathScreenRouter router)
        {
            _entity = entity;
            _router = router;
        }
    }
}