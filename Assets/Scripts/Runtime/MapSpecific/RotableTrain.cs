using DG.Tweening;
using UnityEngine;

namespace Runtime.MapSpecific
{
    public class RotableTrain : MonoBehaviour
    {
        public bool isYoyo = false;
        
        public float delayTime;
        public float rotationTime;
        public Vector3 rotationRange;
    
        private void Start()
        {
            if(!isYoyo) DOTween.Sequence().SetDelay(delayTime).onComplete = () => { transform.DORotate(transform.rotation.eulerAngles + rotationRange, rotationTime); };
            else DOTween.Sequence().SetDelay(delayTime).onComplete = () => { transform.DORotate(transform.rotation.eulerAngles + rotationRange, rotationTime).SetDelay(delayTime).SetLoops(-1, LoopType.Yoyo); };
        }
    }
}
