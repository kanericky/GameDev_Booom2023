using UnityEngine;

namespace Runtime
{
    public class Armor : MonoBehaviour
    {
        public float armorAmount;
        public ArmorType armorType;
    }

    public enum ArmorType
    {
        ChestArmor,
        Helmet
    }
}
