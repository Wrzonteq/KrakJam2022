using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class LevelExitGate : MonoBehaviour {
        public event Action GateEnteredEvent;

        [SerializeField] GameObject openedGate;
        [SerializeField] Collider2D gateCollider;

        public bool IsOpened { get; private set; }
        public bool IsUsed { get; private set; }


        public void Close() {
            IsOpened = false;
            openedGate.SetActive(false);
            gateCollider.enabled = false;
        }

        public void Open() {
            IsOpened = true;
            openedGate.SetActive(true);
            gateCollider.enabled = true;
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                GateEnteredEvent?.Invoke();
                IsUsed = true;
            }
        }
    }
}
