using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;
        [SerializeField] CrosshairController crosshairPrefab;

        CrosshairController crosshairInstance;

        public Camera MainCamera { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
            crosshairInstance = Instantiate(crosshairPrefab, MainCamera.transform, false);
        }

        public override void Initialise() {
            
        }

        public void SetupCrosshairFollowTarget(Transform followTarget) {
            crosshairInstance.SetupFollow(followTarget);
        }
    }
}
