using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CrosshairController : MonoBehaviour {
        Transform cachedTransform;
        Transform followPointTransform;


        void Awake() {
            cachedTransform = transform;
        }

        public void SetupFollow(Transform follow) {
            followPointTransform = follow;
        }

        void Update() {
            if (!followPointTransform)
                return;
            Vector2 mousePosition = GameSystems.GetSystem<CameraSystem>().MainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Vector2.Distance(cachedTransform.position, mousePosition) > 0.01f) {
                cachedTransform.position = mousePosition;
                followPointTransform.localPosition = cachedTransform.localPosition / 4;
            }
        }
    }
}
