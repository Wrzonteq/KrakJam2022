using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
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

        void OnTriggerEnter2D(Collider2D other) {
            Debug.Log($"OnTriggerEnter2D");
            if (other.gameObject.CompareTag("Player"))
                MovePlayerToArea();
        }

        void OnCollisionEnter2D(Collision2D other) {
            Debug.Log($"OnCollisionEnter2D");
            if (other.gameObject.CompareTag("Player"))
                MovePlayerToArea();
        }

        void MovePlayerToArea() {
            var transitionScreen = GameSystems.GetSystem<UISystem>().GetScreen<TransitionScreen>();
            transitionScreen.Show().Forget();
            transitionScreen.FadeInAndOut(TeleportPlayer, OnTransitionDone).Forget();

            void TeleportPlayer() {
                GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform.position = assignedArea.PlayerSpawnPoint.position;
            }

            void OnTransitionDone() {
                transitionScreen.Hide().Forget();
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
