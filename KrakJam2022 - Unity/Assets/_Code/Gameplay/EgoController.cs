using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EgoController : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
            } else if (col.CompareTag("Enemy")) {
                var enemy = col.GetComponent<Enemy>();
                GameSystems.GetSystem<GameStateSystem>().Insanity.Value += enemy.damage;
                enemy.Kill().Forget();
                // DESTROY enemy - 
                // increase INSANITY
            }
        }

        void OnTriggerExit2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
            }
        }

        public void Interact() {
            Debug.Log("Interacting with EGO");
            
            if (GameSystems.GetSystem<GameStateSystem>().CollectedMemoriesCount.Value >= Consts.MEMORIES_TO_COLLECT) {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.ClearCollectables();
                GameSystems.GetSystem<GameplaySystem>().BeginInsanityStage();
            }
        }
    }
}
