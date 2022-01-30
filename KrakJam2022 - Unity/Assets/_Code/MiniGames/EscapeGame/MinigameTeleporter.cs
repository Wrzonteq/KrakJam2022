using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MinigameTeleporter : MonoBehaviour {
        [SerializeField] Transform target;

        bool ignoreNext;


        void OnTriggerEnter2D(Collider2D collider) {
            if (ignoreNext) {
                ignoreNext = false;
                return;
            }
            if (!collider.gameObject.CompareTag("Player"))
                return;
            if(target.CompareTag("MinigameTeleporter"))
                target.GetComponent<MinigameTeleporter>().IgnoreNextTriggerEnter();
            collider.GetComponent<PlayerController>().Teleport(target.position);
        }

        public void IgnoreNextTriggerEnter() {
            ignoreNext = true;
        }
    }
}
