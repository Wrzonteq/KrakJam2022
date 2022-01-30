using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class BrainWallController : MonoBehaviour {

        public GameObject Sprite;
        public Sprite SanitySprite;
        public Sprite InsanitySprint;

        public void Init() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void OnDestroy() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue -= HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            if (stage == GameStage.Insanity)
                Sprite.GetComponent<SpriteRenderer>().sprite = InsanitySprint;
            else if (stage == GameStage.Sanity)
                Sprite.GetComponent<SpriteRenderer>().sprite = SanitySprite;
        }
    }
}
