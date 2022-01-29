using System.Collections.Generic;
using PartTimeKamikaze.KrakJam2022.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        public const int MEMORIES_TO_COLLECT = 2;
        
        public static PlayerController Instance { get; private set; }

        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] private int movementSpeed = 10;

        public Observable<int> MemoriesCollected { get; } = new(0);
        List<IInteractable> InteractablesInRange { get; } = new();

        Vector3 move;
        
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
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
            MemoriesCollected.Set(MemoriesCollected.Value + 1);
        }

        public void ClearCollectables() {
            MemoriesCollected.Set(0);
        }
    }
}
    
