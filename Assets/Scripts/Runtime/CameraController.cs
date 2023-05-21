using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Sequence = DG.Tweening.Sequence;

namespace Runtime
{

    public class CameraController : MonoBehaviour
    {
        [Header("Camera Reference")]
        [SerializeField] private Transform enemyRenderCamera;
        [SerializeField] private Transform uiRenderCamera;

        [Header("Camera Positions Reference")] 
        [SerializeField] private Transform cameraPosIdlePhase;
        [SerializeField] private Transform cameraPosReloadingPhase;
        [SerializeField] private Transform cameraPosAimingPhase;
        [SerializeField] private Transform cameraCurrentTarget;

        [Header("Camera Animation Data")] 
        [SerializeField] private Vector2 cameraDeadZoneRangeGeneral;
        [SerializeField] private Vector2 cameraDeadZoneRangeAiming;
        [SerializeField] private float cameraBreathRange = 0.4f;
        [SerializeField] private float cameraShakeRange = 0.2f;
        [SerializeField] private float cameraIdleFov = 20;
        [SerializeField] private float cameraReloadFov = 40;
        [SerializeField] private float cameraAimFov = 50;

        [Header("Camera Animation Speed")] 
        [SerializeField] private float cameraMovementTime = 1f;
        [SerializeField] private float cameraDeadZoneSmoothTime = 0.2f;
        [SerializeField] private float cameraBreathTime = 1f;
        [SerializeField] private float cameraShakeTime = 0.2f;

        [Header("Post Processing")] 
        [SerializeField] private Volume volume;

        [Header("Debug")] [SerializeField] private LayerMask aimMask = new LayerMask();

        private bool activateCamDeathZoneMovement;

        private void Awake()
        {
            // Get layer cameras
            enemyRenderCamera = transform.GetChild(0);
            uiRenderCamera = transform.GetChild(1);

            activateCamDeathZoneMovement = true;
        }

        private void Start()
        {
            ChangeCameraPosToIdle();
        }

        private void LateUpdate()
        {
            if (!activateCamDeathZoneMovement) return;
            transform.DOMove(cameraCurrentTarget.position, cameraMovementTime);
        }

        public void ChangeCameraPosToIdle()
        {
            //transform.DOComplete();

            // Handle main camera
            Sequence sequence = DOTween.Sequence();

            cameraCurrentTarget = cameraPosIdlePhase;
            
            sequence.SetDelay(0f)
                .Append(transform.DOMove(cameraCurrentTarget.position, cameraMovementTime).SetEase(Ease.OutQuad))
                .Join(transform.DORotate(cameraCurrentTarget.rotation.eulerAngles, cameraMovementTime)
                    .SetEase(Ease.OutQuad));
            
            transform.GetComponent<Camera>().DOFieldOfView(cameraIdleFov, cameraMovementTime);
            
            // Handle enemy render camera
            enemyRenderCamera.GetComponent<Camera>().enabled = true;
            enemyRenderCamera.GetComponent<Camera>().DOFieldOfView(cameraIdleFov, cameraMovementTime);

            ChangeCameraFocalLength(1f);
            ChangeCameraSaturation(0);
        }

        public void ChangeCameraPosToReload()
        {
            //transform.DOComplete();
            
            cameraCurrentTarget = cameraPosReloadingPhase;

            // Handle main camera
            transform.DOMove(cameraCurrentTarget.position, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraCurrentTarget.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.GetComponent<Camera>().DOFieldOfView(cameraReloadFov, cameraMovementTime);
            
            // Handle enemy render camera
            enemyRenderCamera.GetComponent<Camera>().enabled = false;
            enemyRenderCamera.GetComponent<Camera>().DOFieldOfView(cameraReloadFov, cameraMovementTime);
            enemyRenderCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = true;

            // Handle post-process
            ChangeCameraFocalLength(100f);
            ChangeCameraFocusDistance(1.2f);
            ChangeCameraSaturation(0);
        }

        public void ChangeCameraPosToAiming()
        {
            //transform.DOComplete();

            cameraCurrentTarget = cameraPosAimingPhase;
            
            // Handle the main camera
            transform.DOMove(cameraCurrentTarget.position, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraCurrentTarget.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
            
            transform.GetComponent<Camera>().DOFieldOfView(cameraAimFov, cameraMovementTime);
            
            // Handle the enemy render camera
            enemyRenderCamera.GetComponent<Camera>().enabled = true;
            enemyRenderCamera.GetComponent<Camera>().DOFieldOfView(cameraAimFov, cameraMovementTime);
            enemyRenderCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = false;

            // Handle post-process
            ChangeCameraFocalLength(180f);
            ChangeCameraFocusDistance(14f);
            ChangeCameraSaturation(-50f);

        }

        public void ResetCameraPos()
        {
            transform.DOMove(cameraCurrentTarget.position, cameraMovementTime).SetEase(Ease.OutQuad);
            transform.DORotate(cameraCurrentTarget.rotation.eulerAngles, cameraMovementTime).SetEase(Ease.OutQuad);
        }

        private void ChangeCameraFocalLength(float value)
        {
            DepthOfField dof;
            volume.profile.TryGet<DepthOfField>(out dof);
            dof.focalLength.value = value;
        }
        
        private void ChangeCameraFocusDistance(float value)
        {
            DepthOfField dof;
            volume.profile.TryGet<DepthOfField>(out dof);
            dof.focusDistance.value = value;
        }

        private void ChangeCameraSaturation(float value)
        {
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

            float currentValue = colorAdjustments.saturation.value;

            while (currentValue > value)
            {
                currentValue -= Time.deltaTime;
                colorAdjustments.saturation.value = currentValue;
            }
             
            while (currentValue < value)
            {
                currentValue += Time.deltaTime;
                colorAdjustments.saturation.value = currentValue;
            }
        }

        public Vector3 GetCameraRotIdle()
        {
            return cameraPosIdlePhase.rotation.eulerAngles;
        }

        public Vector3 GetCameraRotAim()
        {
            return cameraPosAimingPhase.rotation.eulerAngles;
        }

        public Vector3 GetCameraPosIdle()
        {
            return cameraPosIdlePhase.position;
        }

        public Vector3 GetCameraRotReload()
        {
            return cameraPosReloadingPhase.rotation.eulerAngles;
        }

        public Vector3 GetMousePosInWorld()
        {
            Camera mainCamera = GetComponent<Camera>();

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimMask))
            {
                return hit.point;
            }

            return new Vector3(0, 0, 0);
        }

        public void HandleCameraShake()
        {
            activateCamDeathZoneMovement = false;
            
            float orginZ = transform.position.z;
            //transform.DOComplete();
            DOTween.Sequence().SetDelay(.2f)
                .Append(transform.DOMoveZ(orginZ -.10f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ +.00f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ +.10f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ -.00f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ -.05f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ -.00f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ +.05f * 1.4f, .03f).Play())
                .Append(transform.DOMoveZ(orginZ -.00f * 1.4f, .03f).Play())
                .onComplete = () => { activateCamDeathZoneMovement = true; };

        }

        public void HandleCameraDeadZoneMovement(Vector3 cameraCurrentRotation, bool isAimingPhase = false)
        {
            Vector2 deadZoneRange;

            if (isAimingPhase) deadZoneRange = cameraDeadZoneRangeAiming;
            else deadZoneRange = cameraDeadZoneRangeGeneral;

            Vector3 currentMousePos = Input.mousePosition;
            Vector3 mousePivot = new Vector3(Screen.width/2, Screen.height/2, 0);

            float x = (currentMousePos.x - mousePivot.x) * 0.005f;
            float y = (mousePivot.y - currentMousePos.y) * 0.005f;

            float targetCameraRotationX = Mathf.Clamp(
                cameraCurrentRotation.x + y,
                cameraCurrentRotation.x - deadZoneRange.x,
                cameraCurrentRotation.x + deadZoneRange.x);
            
            float targetCameraRotationY = Mathf.Clamp(
                cameraCurrentRotation.y + x, 
                cameraCurrentRotation.y - deadZoneRange.y, 
                cameraCurrentRotation.y + deadZoneRange.y);

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
