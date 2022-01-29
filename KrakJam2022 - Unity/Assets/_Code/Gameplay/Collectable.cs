using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Collectable : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            PlayerController.Instance.RegisterInteractable(this);
        }

        void OnTriggerExit2D(Collider2D col) {
            PlayerController.Instance.UnregisterInteractable(this);
        }

        public void Interact() {
            Debug.Log("Interacting with Collectable");
            
            PlayerController.Instance.UnregisterInteractable(this);
            PlayerController.Instance.PickupCollectable(this);
                
            Destroy(gameObject);
            // TODO: UI Show image of this memory
        }
    }
}
