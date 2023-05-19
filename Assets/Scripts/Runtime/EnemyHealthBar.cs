using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    [RequireComponent(typeof(EnemyCharacterController))]
    public class EnemyHealthBar : MonoBehaviour
    {
        public Canvas enemyHealthBarCanvas;
        public Image enemyHealthBar;
        public Image enemyArmorBar;

        public void InitHealthBar(float healthPercentage)
        {
            enemyHealthBar.transform.localScale = new Vector3(healthPercentage, 1, 1);
        }

        public void UpdateHealthBar(float healthPercentage)
        {
            enemyHealthBar.transform.DOScaleX(healthPercentage, .2f);
        }

        public void InitArmorBar(float armorPercentage)
        {
            enemyArmorBar.transform.localScale = new Vector3(armorPercentage, 1, 1);
        }

        public void UpdateArmorBar(float armorPercentage)
        {
            enemyArmorBar.transform.DOScaleX(armorPercentage, .2f);
        }
    }
}
