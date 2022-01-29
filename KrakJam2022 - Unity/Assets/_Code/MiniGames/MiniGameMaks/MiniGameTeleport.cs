using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MiniGameTeleport : MonoBehaviour {

        private Action collisionCallback;

        public void AddCollisionCallback(Action collisionCallback = null) {
            this.collisionCallback = collisionCallback;
        }

        void OnCollisionEnter(Collision collision) {
            // TODO : put variable here
            if (collision.gameObject.name == "Player") {
                this.collisionCallback?.Invoke();
            }
        }
    }
}
