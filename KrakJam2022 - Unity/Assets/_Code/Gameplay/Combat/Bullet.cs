using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.Combat {
    public class Bullet : MonoBehaviour {
        [SerializeField] Rigidbody2D selfRigidbody2d;
        [SerializeField] float aliveTime = 2.5f;

        public void FixedUpdate() {
            aliveTime -= Time.deltaTime;
            
            if (aliveTime < 0)
                Destroy(gameObject);
        }

        public void Fire(Vector2 direction, float power) {
            selfRigidbody2d.AddForce(direction.normalized * power, ForceMode2D.Impulse);
        }

        public void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Enemy")) {
                col.GetComponent<Enemy>().TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
