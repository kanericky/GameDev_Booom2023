namespace Runtime
{
    public static class AmmoFactory
    {
        private static Ammo redAmmo = new(GameElementColor.Red, 30f);
        private static Ammo yellowAmmo = new(GameElementColor.Yellow, 30f);
        private static Ammo blueAmmo = new(GameElementColor.Blue, 30f);
        private static Ammo blackAmmo = new(GameElementColor.Black, 30f);
        
        // Used by enemy
        private static Ammo whiteAmmo = new(GameElementColor.White, 10f);

        public static Ammo GetAmmoFromFactory(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.RedAmmo:
                    return redAmmo;
                
                case AmmoType.YellowAmmo:
                    return yellowAmmo;
                
                case AmmoType.BlueAmmo:
                    return blueAmmo;
                
                case AmmoType.BlackAmmo:
                    return blackAmmo;
                
                case AmmoType.WhiteAmmo:
                    return whiteAmmo;
            }

            return null;
        }

        public static void IncreaseDamage(AmmoType ammoType, float percentage)
        {
            switch (ammoType)
            {
                case AmmoType.RedAmmo:
                    redAmmo.initDamage *= (1 + percentage);
                    break;

                case AmmoType.YellowAmmo:
                    yellowAmmo.initDamage *= (1 + percentage);
                    break;

                case AmmoType.BlueAmmo:
                    blueAmmo.initDamage *= (1 + percentage);
                    break;

                case AmmoType.BlackAmmo:
                    blackAmmo.initDamage *= (1 + percentage);
                    break;

                case AmmoType.WhiteAmmo:
                    whiteAmmo.initDamage *= (1 + percentage);
                    break;
            }
        }
    }
    
    public enum AmmoType{
        RedAmmo,
        YellowAmmo,
        BlueAmmo,
        BlackAmmo,
        WhiteAmmo
    }
}
