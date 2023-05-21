using DG.Tweening;
using TMPro;
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

        public TMP_Text currentHealthText;
        public TMP_Text totalHealthText;

        public void InitHealthBar(float healthPercentage, float currentHealth, float totalHealth)
        {
            enemyHealthBar.transform.localScale = new Vector3(healthPercentage, 1, 1);

            currentHealthText.text = currentHealth.ToString();
            totalHealthText.text = totalHealth.ToString();
        }

        public void UpdateHealthBar(float healthPercentage, float currentHealth, float totalHealth)
        {
            enemyHealthBar.transform.DOScaleX(healthPercentage, .2f);
            
            currentHealthText.text = currentHealth.ToString();

            if (healthPercentage == 0) enemyHealthBarCanvas.enabled = false;
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
