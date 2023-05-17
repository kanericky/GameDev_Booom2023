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
    }
}
