using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EscapeGameTeleport : MonoBehaviour {

        private Action collisionCallback;

        public void AddCollisionCallback(Action collisionCallback = null) {
            this.collisionCallback = collisionCallback;
        }

        void OnTriggerEnter2D(Collider2D collider) {
            Debug.Log($"collistion {collider.gameObject.tag}");
            if (collider.gameObject.CompareTag("Player")) {
                this.collisionCallback?.Invoke();
            }
        }
    }
}
