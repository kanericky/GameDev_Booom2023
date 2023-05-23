using DG.Tweening;
using UnityEngine;

namespace Runtime.MapSpecific
{
    public class MovableTrain : MonoBehaviour
    {
        public bool isYoyo = false;
        
        public float delayTime;
        public float movementTime;
        public float moveDistance;
    
        private void Start()
        {
            if(!isYoyo) DOTween.Sequence().SetDelay(delayTime).onComplete = () => { transform.DOMoveX(moveDistance, movementTime); };
            else DOTween.Sequence().SetDelay(delayTime).onComplete = () => { transform.DOMoveX(moveDistance, movementTime).SetLoops(-1, LoopType.Yoyo); };
        }
    }
}
