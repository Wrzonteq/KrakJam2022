using System;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class LevelGate : MonoBehaviour {
        public event Action<GateState> GateStateChanged;
        
        [SerializeField] Collider2D entranceTrigger;
        [SerializeField] GameObject inactive;
        [SerializeField] GameObject insane;
        [SerializeField] GameObject active;

        [SerializeField] Emotion emotion;

        public Emotion Emotion => emotion;
        public GateState State { get; private set; }


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
            if (other.gameObject.CompareTag("Player"))
                MovePlayerToArea();
        }

        void MovePlayerToArea() {
            var transitionScreen = GameSystems.GetSystem<UISystem>().GetScreen<TransitionScreen>();
            transitionScreen.Show().Forget();
            transitionScreen.FadeInAndOut(TeleportPlayer, OnTransitionDone).Forget();

            void TeleportPlayer() {
                GameSystems.GetSystem<GameplaySystem>().GetLevelArea(emotion).TeleportPlayerToArea();
            }

            void OnTransitionDone() {
                transitionScreen.Hide().Forget();
                Close();
            }
        }

        public void InitialiseFromSavedState(EmotionLevelState state) {
            if (state.insanitySurvived)
                Close();
            else if (state.insanityStarted)
                GoInsane();
            else if (state.gateUnlocked)
                Activate();
            else
                Close();
        }

        void SetGateState(GateState state) {
            State = state;
            inactive.SetActive(state == GateState.Inactive);
            active.SetActive(state == GateState.Active);
            insane.SetActive(state == GateState.Insane);
            GateStateChanged?.Invoke(state);
        }

        public enum GateState {
            Inactive,
            Active,
            Insane
        }
    }
}
