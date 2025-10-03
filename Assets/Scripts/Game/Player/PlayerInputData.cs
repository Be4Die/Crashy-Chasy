namespace CrashyChasy.Game.Player
{
    public struct PlayerInputData
    {
        public int TurnDirection;

        public PlayerInputData(int turnDirection)
        {
            TurnDirection = turnDirection;
        }
        
        public bool IsTurning() => TurnDirection != 0;
    }
}