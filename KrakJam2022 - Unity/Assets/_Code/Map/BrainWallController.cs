using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class BrainWallController : MonoBehaviour {

        public GameObject Sprite;
        public Sprite SanitySprite;
        public Sprite InsanitySprint;

        void Init() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        private void HandleStageChanged(GameStage stage) {
            if (stage == GameStage.Insanity)
                Sprite.GetComponent<SpriteRenderer>().sprite = InsanitySprint;
            else if (stage == GameStage.Sanity)
                Sprite.GetComponent<SpriteRenderer>().sprite = SanitySprite;
        }
    }
}
