using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CameraController : MonoBehaviour {
        public float followSharpness = 0.1f;
        
        void LateUpdate() {
            float blend = 1f - Mathf.Pow(1f - followSharpness, Time.deltaTime * 30f);

            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y, transform.position.z),
                blend);
        }
    }
}
