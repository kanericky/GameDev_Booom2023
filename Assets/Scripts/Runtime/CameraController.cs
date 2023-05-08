using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Runtime
{

    public class CameraController : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform mainCamera;

        [Header("Camera Positions")] 
        [SerializeField] private Transform cameraPosIdlePhase;
        [SerializeField] private Transform cameraPosReloadingPhase;
        [SerializeField] private Transform cameraPosAimingPhase;
        [SerializeField] private Transform cameraPosFiringPhase;

        [Header("Camera Animation Data")] 
        [SerializeField] private float cameraBreathRange = 0.4f;

        [Header("Camera Animation Speed")] 
        [SerializeField] private float cameraMovementTime = 1f;
        [SerializeField] private float cameraDeadZoneSmoothTime = 0.2f;
        [SerializeField] private float cameraBreathTime = 1f;

        [Header("Post Processing")] 
        [SerializeField] private Volume volume;

        [Header("Debug")] 
        [SerializeField] private Vector2 mouseInput;

        private void Start()
        {
            ChangeCameraPosToIdle();
        }

        public void ChangeCameraPosToIdle()
        {
            transform.DOComplete();
            
            transform.DOMove(cameraPosIdlePhase.localPosition, cameraMovementTime).SetEase(Ease.InQuad);
            transform.DORotate(cameraPosIdlePhase.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.InQuad);

            GetComponent<Camera>().DOFieldOfView(50, cameraMovementTime);
            
            ChangeCameraFocalLength(1f);
        }

        public void ChangeCameraPosToReload()
        {
            transform.DOComplete();

            transform.DOMove(cameraPosReloadingPhase.localPosition, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraPosReloadingPhase.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
            GetComponent<Camera>().DOFieldOfView(40, cameraMovementTime);

            ChangeCameraFocalLength(80f);
        }

        public void ChangeCameraPosToAiming()
        {
            transform.DOComplete();
            
            transform.DOMove(cameraPosAimingPhase.localPosition, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraPosAimingPhase.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
            
            GetComponent<Camera>().DOFieldOfView(50, cameraMovementTime);
            ChangeCameraFocalLength(1f);

        }

        public void ChangeCameraFocalLength(float value)
        {
            DepthOfField dof;
            volume.profile.TryGet<DepthOfField>(out dof);
            dof.focalLength.value = value;
        }

        public Vector3 GetCameraRotIdle()
        {
            return cameraPosIdlePhase.rotation.eulerAngles;
        }

        public Vector3 GetCameraPosIdle()
        {
            return cameraPosIdlePhase.position;
        }

        public void HandleCameraDeadZoneMovement(Vector3 cameraCurrentRotation)
        {

            Vector3 currentMousePos = Input.mousePosition;
            Vector3 mousePivot = new Vector3(960, 540, 0);

            float x = (currentMousePos.x - mousePivot.x) * 0.005f;
            float y = (mousePivot.y - currentMousePos.y) * 0.005f;

            float targetCameraRotationX = Mathf.Clamp(
                cameraCurrentRotation.x + y,
                cameraCurrentRotation.x -1,
                cameraCurrentRotation.x + 1);
            
            float targetCameraRotationY = Mathf.Clamp(
                cameraCurrentRotation.y + x, 
                cameraCurrentRotation.y - 2, 
                cameraCurrentRotation.y + 2);

            Vector3 targetCameraRotation = new Vector3(targetCameraRotationX, targetCameraRotationY, cameraCurrentRotation.z);

            transform.DORotate(targetCameraRotation, cameraDeadZoneSmoothTime).SetEase(Ease.OutQuad);
            
            
            //transform.eulerAngles = new Vector3(
                //Mathf.Lerp(cameraCurrentRotation.x, targetCameraRotationX, cameraDeadZoneSmoothTime), 
            //Mathf.Lerp(cameraCurrentRotation.y, targetCameraRotationY, cameraDeadZoneSmoothTime),
              //  GetCameraRotIdle().z);

        }
        
        public void HandleCameraBreath(Vector3 cameraCurrentPosition)
        {
            transform.DOComplete();
            
            transform.DOMoveY(cameraCurrentPosition.y + cameraBreathRange, cameraBreathTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
      
    }
}
