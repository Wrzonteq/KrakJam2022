using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Collectable : MonoBehaviour, IInteractable {
        [SerializeField] MessagePopup message;

        void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
                message.Show();
            }
        }

        void OnTriggerExit2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
                message.Hide();
            }
        }

        public void Interact() {
            Debug.Log("Interacting with Collectable");
            
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);

            OnInteract();
            gameObject.SetActive(false);
        }

        protected virtual void OnInteract() { }
    }
}
