using System;

namespace Utils
{
    public static class GameEvents
    {
        public static Action PlayerDied;
        public static Action PlayerWon;
        public static Action RestartLevel;
        public static Action GameOver;
        public static Action<int,int> PlayerHealthChanged;
        public static Action<int> WaveChanged;
    }
}
