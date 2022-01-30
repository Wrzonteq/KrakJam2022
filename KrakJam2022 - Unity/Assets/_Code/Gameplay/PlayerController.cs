using System.Collections.Generic;
using Cinemachine;
using PartTimeKamikaze.KrakJam2022.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int movementSpeed = 10;
        [SerializeField] Transform crosshairFollowTarget;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] float bulletSpeed = 6.66f;
        [SerializeField] float shotsInterval = 0.2f;

        float shakeTimer;
        float nextShotTime;
        bool isShooting;
        Vector3 move;
        List<IInteractable> interactablesInRange;
        GameStateSystem gameStateSystem;
        InputSystem inputSystem;
        Transform cachedTransform;

        public Transform CrosshairFollowTarget => crosshairFollowTarget;


        public void Initialise() {
            gameStateSystem = GameSystems.GetSystem<GameStateSystem>();
            inputSystem = GameSystems.GetSystem<InputSystem>();
            inputSystem.Bindings.Gameplay.Interact.performed += HandleInteraction;
            interactablesInRange = new List<IInteractable>();
            gameStateSystem.Stage.ChangedValue += HandleStageChanged;
            cachedTransform = transform;
        }

        void HandleStageChanged(GameStage stage) {
            //todo swap graphics
        }

        void HandleInteraction(InputAction.CallbackContext _) {
            if(interactablesInRange.Count > 0)
                interactablesInRange[0].Interact();
        }

        void Update() {
            if (!inputSystem.PlayerInputEnabled) {
                selfRigidbody2D.velocity = Vector2.zero;
                return;
            }
            UpdateInputValues();
            UpdateMovement();
            UpdateShooting();
            UpdateCamShake();
        }

        void UpdateInputValues() {
            move = inputSystem.Bindings.Gameplay.Move.ReadValue<Vector2>();
            isShooting = inputSystem.Bindings.Gameplay.Fire.IsPressed();
        }

        void UpdateMovement() {
            selfRigidbody2D.velocity = move * movementSpeed;
        }

        void UpdateShooting() {
            if (gameStateSystem.Stage.Value != GameStage.Insanity)
                return;
            if (!isShooting)
                return;
            if(nextShotTime <= Time.time)
                Shoot();
        }

        void UpdateCamShake() {
            if (shakeTimer > 0) {
                shakeTimer -= Time.deltaTime;

                if (shakeTimer <= 0) {
                    ShakeCamera(0, 0);
                }
            }
        }

        void Shoot() {
            nextShotTime = Time.time + shotsInterval;
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = cachedTransform.position;

            bullet.Fire(GameSystems.GetSystem<CameraSystem>().CrosshairInstance.transform.localPosition, bulletSpeed);
            ShakeCamera(1f, .1f);
        }

        void ShakeCamera(float intensity, float time) {
            var channelPerlin = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            channelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }

        public void RegisterInteractable(IInteractable interactable) {
            interactablesInRange.Insert(0, interactable);
        }

        public void UnregisterInteractable(IInteractable interactable) {
            interactablesInRange.Remove(interactable);
        }

        public void Teleport(Vector3 position) {
            var mainCameraTransform = GameSystems.GetSystem<CameraSystem>().MainCamera.transform;
            var playerToCameraOffset = cachedTransform.position - mainCameraTransform.position;
            cachedTransform.position = position;
            mainCameraTransform.position = position - playerToCameraOffset;
        }
    }
}
    
