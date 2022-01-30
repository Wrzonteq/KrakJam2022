using System;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class LevelGateMessageTrigger : MonoBehaviour {
        [SerializeField] LevelGate owner;
        [SerializeField] MessagePopup message;


        void Awake() {
            owner.GateStateChanged += HandleGateStateChanged;
            HandleGateStateChanged(owner.State);
            message.Hide();
        }

        void HandleGateStateChanged(LevelGate.GateState state) {
            switch (state) {
                case LevelGate.GateState.Inactive:
                    message.SetText($"{owner.Emotion} - Locked");
                    break;
                case LevelGate.GateState.Active:
                    message.SetText($"{owner.Emotion}");
                    break;
                case LevelGate.GateState.Insane:
                    message.SetText(string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player"))
                message.Show();
        }

        void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player"))
                message.Hide();
        }
    }
}
