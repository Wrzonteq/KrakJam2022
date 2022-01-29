using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Waypoint : MonoBehaviour {

        [System.Serializable]
        public class WaypointWithChance {
            public WaypointWithChance(Waypoint waypoint, int chance) {
                this.waypoint = waypoint;
                this.chance = chance;
            }
            [SerializeField]
            public Waypoint waypoint;
            [SerializeField]
            public int chance;
        }

        [SerializeField]
        public List<WaypointWithChance> nextWaypoints = new List<WaypointWithChance>();

        [SerializeField]
        public bool spawnPoint = false;

        public Waypoint SelectNextWaypoint() {
            if (nextWaypoints.Count == 0) {
                Debug.Log("waypoint: finished enemy move");
                return null;
            }
            Debug.Log("waypoint: selecting next enemy waypoint");

            int sum = nextWaypoints.Sum(w => w.chance);
            if (sum != 100) {
                Debug.LogWarning("Warning: total sum for next waypoints in not 100");
            }
            int chanceValue = Random.Range(0, sum);

            int currentSum = 0;
            foreach (var waypointData in nextWaypoints) {
                currentSum += waypointData.chance;
                if (currentSum > chanceValue)
                    return waypointData.waypoint;
            }
            return nextWaypoints.Last().waypoint;
        }

        void OnDrawGizmosSelected() {
        //void OnDrawGizmos() {
            Gizmos.DrawSphere(transform.position, 0.3f);
            GUIStyle style = new GUIStyle();
            Vector3 labelPosition = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z);
            UnityEditor.Handles.Label(labelPosition, gameObject.name, style);

            if (nextWaypoints.Count == 0)
                return;

            int sum = 0;
            foreach (var waypointData in nextWaypoints) {
                if (waypointData != null) {
                    sum += waypointData.chance;
                }
            }
            foreach (var waypointData in nextWaypoints) {
                if (sum != 100) {
                    Gizmos.color = new Color(1f, 0.0f, 0.0f);
                } else
                    Gizmos.color = new Color(0.0f, 0.0f, 0.0f);

                if (waypointData != null && waypointData.waypoint != null) {
                    Gizmos.DrawLine(transform.position, waypointData.waypoint.transform.position);
                }

            }
        }
    }
}
