using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PathManager : MonoBehaviour {

        /*public Waypoint SelectNextWaypoint(Waypoint currentWaypoint) {
            int sum = currentWaypoint.nextWaypoints.Sum(w => w.chanceToTakePath);
            if (sum != 100) {
                Debug.LogWarning("Warning: total sum for next waypoints in not 100");
            }
            int chanceValue = Random.Range(0, sum);

            int currentSum = 0;
            foreach(var waypoint in currentWaypoint.nextWaypoints) {
                currentSum += waypoint.chanceToTakePath;
                if (currentSum > chanceValue)
                    return waypoint;
            }
            return currentWaypoint.nextWaypoints.Last();
        }*/
    }
}
