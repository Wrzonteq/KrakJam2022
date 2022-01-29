using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;
        [SerializeField] CrosshairController crosshairPrefab;

        public Camera MainCamera { get; private set; }
        public CrosshairController CrosshairInstance { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
        }

        public override void Initialise() {
            CrosshairInstance = Instantiate(crosshairPrefab, MainCamera.transform, false);
        }

        public void SetupCrosshairFollowTarget(Transform followTarget) {
            CrosshairInstance.Setup(MainCamera, followTarget);
        }
    }
}
