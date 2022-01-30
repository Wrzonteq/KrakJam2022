using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CrosshairController : MonoBehaviour {
        [SerializeField] Transform followPointTransform;
        Camera _camera;
        Transform cachedTransform;


        void Awake() {
            cachedTransform = transform;
            if (!GameSystems.IsInitialised) {
                _camera = FindObjectOfType<Camera>();
                followPointTransform = FindObjectOfType<PlayerController>().CrosshairFollowTarget;
            }
        }

        public void Setup(Camera cam, Transform follow) {
            _camera = cam;
            followPointTransform = follow;
        }

        void Update() {
            if (!followPointTransform)
                return;
            if(!GameSystems.GetSystem<InputSystem>().MouseLookEnabled)
                return;
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Vector2.Distance(cachedTransform.position, mousePosition) > 0.01f) {
                cachedTransform.position = mousePosition;
                followPointTransform.localPosition = cachedTransform.localPosition / 4;
            }
        }
    }
}
