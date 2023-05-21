using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Runtime.DropItemSystemFramework
{
    public class DropItemSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private List<DropItemConfigSO> allDropItemsData;
        [SerializeField] private List<DropItemUIModel> allDropItemsUI;

        private void OnValidate()
        {
            allDropItemsUI = FindObjectsOfType<DropItemUIModel>().ToList();
        }

        public void SetupDataToUI()
        {
            for (int i = 0; i < allDropItemsData.Count; i++)
            {
                DropItemConfigSO data = allDropItemsData[i];
                DropItemUIModel ui = allDropItemsUI[i];
                
                data = PostProcessData(data);
                
                ui.InitDropItemUI(data);
            }
        }

        private DropItemConfigSO PostProcessData(DropItemConfigSO data)
        {
            int minCoin = data.minCoinAmount;
            int maxCoin = data.maxCoinAmount;

            data.coinAmount = Random.Range(minCoin, maxCoin);

            if (data.isRandom)
            {
                data.ammoColor = GameManager.GatRandomAmmoColor();
                data.ammoIconSprite = GameManager.GetSpriteBasedOnAmmoColor(data.ammoColor);
            }

            return data;
        }
    }
}
