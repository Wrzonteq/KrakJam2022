using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Collectable : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
        }

        void OnTriggerExit2D(Collider2D col) {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
        }

        public void Interact() {
            Debug.Log("Interacting with Collectable");
            
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.PickupCollectable(this);
                
            Destroy(gameObject);
            // TODO: UI Show image of this memory
        }
    }
}
