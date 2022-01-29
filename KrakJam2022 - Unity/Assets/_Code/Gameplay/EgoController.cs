using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {

    public class EgoController : MonoBehaviour, IInteractable {
        void OnTriggerEnter2D(Collider2D col) {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.RegisterInteractable(this);
        }

        void OnTriggerExit2D(Collider2D col) {
            GameSystems.GetSystem<GameplaySystem>().PlayerInstance.UnregisterInteractable(this);
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
