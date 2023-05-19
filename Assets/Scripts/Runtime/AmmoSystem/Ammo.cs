using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [Serializable]
    public class Ammo
    {
        public GameElementColor gameElementColor;
        public float initDamage;
        public float ammoProjectileSpeed;

        public Ammo()
        {
            gameElementColor = GameElementColor.NotDefined;
            initDamage = 0;
            ammoProjectileSpeed = 10f;
        }

        public Ammo(GameElementColor gameElementColor, float initDamage, float ammoProjectileSpeed = 20f)
        {
            this.gameElementColor = gameElementColor;
            this.initDamage = initDamage;
            this.ammoProjectileSpeed = ammoProjectileSpeed;
        }

        public bool IsAmmoValid()
        {
            return gameElementColor != GameElementColor.NotDefined;
        }

        private GameElementColor GetRandomColor()
        {
            int color = Random.Range(0, (int)GameElementColor.NotDefined);

            switch (color)
            {
                case 0:
                    return GameElementColor.Red;
                    
                case 1:
                    return GameElementColor.Yellow;
                    
                case 2:
                    return GameElementColor.Blue;
                    
                case 3:
                    return GameElementColor.Black;
                    
            }

            return GameElementColor.NotDefined;
        }
    }
}
