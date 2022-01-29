using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Collectable : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            if(col.CompareTag("Player"))
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
        }

        void OnTriggerExit2D(Collider2D col) {
            if(col.CompareTag("Player"))
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
        }

        public void Interact() {
            Debug.Log("Interacting with Collectable");
            
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.PickupCollectable(this);

            OnInteract();

            Destroy(gameObject);
        }

        protected virtual void OnInteract() { }
    }
}
