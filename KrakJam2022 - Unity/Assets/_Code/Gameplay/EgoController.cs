using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {

    public class EgoController : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            PlayerController.Instance.RegisterInteractable(this);
        }

        void OnTriggerExit2D(Collider2D col) {
            PlayerController.Instance.UnregisterInteractable(this);
        }

        public void Interact() {
            Debug.Log("Interacting with EGO");
            
            if (PlayerController.Instance.MemoriesCollected.Value >= PlayerController.MEMORIES_TO_COLLECT) {
                PlayerController.Instance.ClearCollectables();
                // TODO: Start night phase
            }
        }
    }
}
