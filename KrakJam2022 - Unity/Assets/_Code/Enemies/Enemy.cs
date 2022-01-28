using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Enemy : MonoBehaviour {
        [SerializeField]
        public int hp;
        [SerializeField]
        public int damage;

        public float speed = 0.04f;

        public Waypoint NextWaypoint { get; private set; }
        public Vector3 NextWaypointPosition { get; private set; }

        public void Initialize(Waypoint currentWaypoint) {
            this.NextWaypoint = currentWaypoint;
            UpdateToNextWaypoint();
        }

        public void UpdateToNextWaypoint() { 
            this.NextWaypoint = NextWaypoint.SelectNextWaypoint();
            if (this.NextWaypoint != null) {
                this.NextWaypointPosition = new Vector3(
                    this.NextWaypoint.transform.position.x,
                    this.NextWaypoint.transform.position.y,
                    0f); // for some reason I have to reset Z to 0 value
            }
        }

        void Update() {
            Move();
        }

        private void Move() {
            if (NextWaypoint == null) {
                return;
            }

            Vector3 newPosition = Vector3.MoveTowards(transform.position, NextWaypointPosition, speed * Time.deltaTime);
            if (newPosition == NextWaypointPosition) {
                UpdateToNextWaypoint();
            } else {
                transform.position = newPosition;
            }
        }
    }
}
