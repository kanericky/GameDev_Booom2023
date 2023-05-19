using UnityEngine;

namespace Runtime.ArmorSystem
{
    public class Armor : MonoBehaviour
    {
        public ArmorData armorData;
        
        public float armorAmount;
        
        public ArmorType armorType;
        public GameElementColor armorColor;
        
        public Armor armorPrefab;
        public Material armorMaterial;      
        
        private MeshRenderer meshRenderer;

        public void SetupArmor(ArmorData armorData)
        {
            if(meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
            
            if (armorData == null) return;

            this.armorData = armorData;

            armorAmount = armorData.armorAmount;
            armorType = armorData.armorType;
            armorColor = armorData.armorColor;
            armorPrefab = armorData.armorPrefab;
            armorMaterial = armorData.armorMaterial;

            meshRenderer.material = armorMaterial;
        }
    }

    public enum ArmorType
    {
        ChestArmor,
        Helmet
    }
}
