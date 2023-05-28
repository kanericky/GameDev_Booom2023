using Runtime.Menu;
using UnityEngine.EventSystems;

namespace Runtime.DropItemSystemFramework
{
    public class MapDropItemUIModel : DropItemUIModel
    {

        public override void OnPointerClick(PointerEventData eventData)
        {
            PawnInventorySystem playerInventory = new PawnInventorySystem();
            HealthSystem playerHealthSystem = new HealthSystem(100f);

            GameElementColor ammoColor = dropItemConfig.ammoColor;
            int ammoAmountNum = dropItemConfig.ammoAmount;
            
            int sloIndex = GameManager.GetReloadSlotIndexBasedOnAmmoColor(ammoColor);
            Ammo ammoToAdd = AmmoFactory.GetAmmoFromFactory(ammoColor);
            
            for(int i = 0; i < ammoAmountNum; i++) playerInventory.AddItemToSlot(slotIndex: sloIndex, ammoToAdd);

            GameManager.instance.SaveCurrentStatusInfo(playerInventory, playerHealthSystem);

            MenuUIManager.instance.isTriggerOnBegin = true;
            MenuUIManager.instance.TransitionIntro();
            MenuUIManager.instance.isTriggerOnBegin = false;
            
            MapLevelManager.instance.CloseDropItemMenu();
            
            
        }
    }
}
