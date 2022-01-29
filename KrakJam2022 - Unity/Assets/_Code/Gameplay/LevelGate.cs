using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class LevelGate : MonoBehaviour {
        [SerializeField] Collider2D entranceTrigger;
        [SerializeField] GameObject inactive;
        [SerializeField] GameObject insane;
        [SerializeField] GameObject active;

        [SerializeField] EmotionLevelArea assignedArea;
        [SerializeField] Emotion emotion;

        public Emotion Emotion => emotion;


        public void Close() {
            entranceTrigger.enabled = false;
            SetGateState(GateState.Inactive);
        }

        public void Activate() {
            entranceTrigger.enabled = true;
            SetGateState(GateState.Active);
        }

        public void GoInsane() {
            entranceTrigger.enabled = false;
            SetGateState(GateState.Insane);;
        }

        void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player")) {
                PlayerController.Instance.transform.position = assignedArea.PlayerSpawnPoint.position;
            }
        }

        public void InitialiseFromSavedState(EmotionLevelState state) {
            if (state.insanitySurvived)
                Close();
            else if (state.insanityStarted)
                GoInsane();
            else if (state.doorUnlocked)
                Activate();
            else
                Close();
        }

        void SetGateState(GateState state) {
            inactive.SetActive(state == GateState.Inactive);
            active.SetActive(state == GateState.Active);
            insane.SetActive(state == GateState.Insane);
        }

        enum GateState {
            Inactive,
            Active,
            Insane
        }
    }
}
