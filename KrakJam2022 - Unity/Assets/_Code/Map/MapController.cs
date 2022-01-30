using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MapController : MonoBehaviour {
        [SerializeField] BrainWallController[] walls;

        public void Init() {
            foreach (var w in walls)
                w.Init();
        }
    }
}