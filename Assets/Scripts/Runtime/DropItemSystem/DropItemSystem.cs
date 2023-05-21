using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.DropItemSystem
{
    public class DropItemSystem : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private List<DropItemConfigSO> allDropItemsData;
        [SerializeField] private List<DropItemUIModel> allDropItemsUI;

        public DropItemSystem()
        {
            allDropItemsData = new List<DropItemConfigSO>();
            allDropItemsUI = FindObjectsOfType<DropItemUIModel>().ToList();
        }

        public List<DropItemConfigSO> SelectThreeRandomDrops()
        {
            // TODO Implement function
            return null;
        }
    }
}
