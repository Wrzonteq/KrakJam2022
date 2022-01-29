using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class LevelGate : MonoBehaviour {
        [SerializeField] Collider2D entranceTrigger;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Sprite openedGateSprite;
        [SerializeField] Sprite closedGateSprite;
        [SerializeField] EmotionLevelArea assignedArea;


        public void Close() {
            spriteRenderer.sprite = closedGateSprite;
            entranceTrigger.enabled = false;
        }

        public void Open() {
            spriteRenderer.sprite = openedGateSprite;
            entranceTrigger.enabled = true;
        }

        void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                PlayerController.Instance.transform.position = assignedArea.playerSpawnPoint.position;
            }
        }
    }
}
