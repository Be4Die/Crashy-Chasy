namespace CrashyChasy.Game.DeathScreen
{
    public sealed class DeathScreenPresenter
    {
        private readonly DeathScreenView _view;
        private readonly DeathScreenInteractor _interactor;
        private readonly DeathScreenEntity _entity;

        public DeathScreenPresenter(DeathScreenView view, DeathScreenInteractor interactor, DeathScreenEntity entity)
        {
            _view = view;
            _interactor = interactor;
            _entity = entity;
        }

        public void Subscribe()
        {
        }

        public void Unsubscribe()
        {
        }
    }
}