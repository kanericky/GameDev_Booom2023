using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [Serializable]
    public class Ammo
    {
        public AmmoColor ammoColor;
        public float initDamage;
        
        public Ammo()
        {
            ammoColor = AmmoColor.NotDefined;
        }

        public Ammo(bool isRandom)
        {
            ammoColor = GetRandomColor();
        }

        public Ammo(AmmoColor ammoColor)
        {
            this.ammoColor = ammoColor;
        }

        public bool isAmmoValid()
        {
            return ammoColor != AmmoColor.NotDefined;
        }

        private AmmoColor GetRandomColor()
        {
            int color = Random.Range(0, (int)AmmoColor.NotDefined);

            switch (color)
            {
                case 0:
                    return AmmoColor.Red;
                    
                case 1:
                    return AmmoColor.Yellow;
                    
                case 2:
                    return AmmoColor.Blue;
                    
                case 3:
                    return AmmoColor.Black;
                    
            }

            return AmmoColor.NotDefined;
        }
    }


    public enum AmmoColor
    {
        Red,
        Yellow,
        Blue,
        Black,
        NotDefined
    }
}
