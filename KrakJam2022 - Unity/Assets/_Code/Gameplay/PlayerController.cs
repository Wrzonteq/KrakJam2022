using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        public static PlayerController Instance { get; private set; }

        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] int movementSpeed = 10;

        List<IInteractable> InteractablesInRange { get; } = new List<IInteractable>();

        Vector3 move;
        
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void Update() {
            selfRigidbody2D.velocity = move * movementSpeed;
        }

        public void OnMove(InputValue value) {
            move = value.Get<Vector2>();
        }

        public void OnInteract(InputValue value) {
            if (value.isPressed && InteractablesInRange.Count > 0) {
                InteractablesInRange[0].Interact();
            }
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
    
