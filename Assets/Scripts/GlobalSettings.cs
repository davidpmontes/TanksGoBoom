

public static class GlobalSettings
{
    public struct PLAYER
    {
        public static float MASS = 500f;

        public struct WEAPONS
        {
            public static float GRENADE_POWER = 3000f;
            public static float LASER_POWER = 5000f;
            public static float ROCKET_POWER = 1000f;
        }
    }

    public struct ENEMY
    {
        public struct WEAPONS
        {
            public static float MINE_POWER = 3000f;
        }
    }
}
