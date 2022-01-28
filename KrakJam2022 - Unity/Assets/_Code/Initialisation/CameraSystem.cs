using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;

        public Camera MainCamera { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
        }

        public override void Initialise() {
            
        }
    }
}