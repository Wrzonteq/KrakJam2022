using System.Collections.Generic;
using PartTimeKamikaze.KrakJam2022.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int movementSpeed = 10;
        [SerializeField] Transform crosshairFollowTarget;
        [SerializeField] Bullet bulletPrefab;

        List<IInteractable> InteractablesInRange { get; } = new();

        
        [SerializeField] float bulletSpeed = 6.66f;
        [SerializeField] float shootEveryInSec = 0.5f;

        float nextShotIn = 0;
        
        Vector3 move;

        bool isShooting = false; // Range?

        public Transform CrosshairFollowTarget => crosshairFollowTarget;

        void Update() {
            selfRigidbody2D.velocity = move * movementSpeed;

            if (isShooting && nextShotIn <= 0) { // && GameSystems.GetSystem<GameStateSystem>().runtimeGameState.stage == GameStage.Insanity) {
                Shoot();
                nextShotIn = shootEveryInSec;
            }

            if (nextShotIn > 0) {
                nextShotIn -= Time.deltaTime;
            }
        }

        public void OnMove(InputValue value) {
            move = value.Get<Vector2>();
        }

        public void OnInteract(InputValue value) {
            if (value.isPressed && InteractablesInRange.Count > 0) {
                InteractablesInRange[0].Interact();
            }
        }

        public void OnFire(InputValue value) {
            isShooting = value.isPressed;
        }

        public void Shoot() {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;

            bullet.Fire(GameSystems.GetSystem<CameraSystem>().CrosshairInstance.transform.localPosition, bulletSpeed);
        }

        public void RegisterInteractable(IInteractable interactable) {
            InteractablesInRange.Insert(0, interactable);
        }

        public void UnregisterInteractable(IInteractable interactable) {
            InteractablesInRange.Remove(interactable);
        }

        public void PickupCollectable(Collectable collectable) {
            GameSystems.GetSystem<GameStateSystem>().CollectedMemoriesCount.Value += 1;
            
        }

        public void ClearCollectables() {
            GameSystems.GetSystem<GameStateSystem>().CollectedMemoriesCount.Value = 0;
        }
    }
}
    
