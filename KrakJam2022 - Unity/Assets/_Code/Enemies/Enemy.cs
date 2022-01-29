using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class Enemy : MonoBehaviour {
        [SerializeField] int health;
        [SerializeField] int damage;
        [SerializeField] int insanityReduction;
        [SerializeField] float speed = 5f;

        public int HealthPoints => health;
        public int Damage => damage;
        public Waypoint NextWaypoint { get; private set; }
        public Vector3 NextWaypointPosition { get; private set; }


        public void Initialize(Waypoint currentWaypoint) {
            this.NextWaypoint = currentWaypoint;
            UpdateToNextWaypoint();
        }

        public void UpdateToNextWaypoint() { 
            this.NextWaypoint = NextWaypoint.SelectNextWaypoint();
            if (this.NextWaypoint != null) {
                this.NextWaypointPosition = new Vector3(
                    this.NextWaypoint.transform.position.x,
                    this.NextWaypoint.transform.position.y,
                    // TODO : maybe use const here
                    0f); // for some reason I have to reset Z to 0 value - otherwise enemies are disapearring
            }
        }

        void Update() {
            if (health <= 0)
                return;
            Move();
        }

        void Move() {
            if (NextWaypoint == null) {
                return;
            }
            var newPosition = Vector3.MoveTowards(transform.position, NextWaypointPosition, speed * Time.deltaTime);
            if (newPosition == NextWaypointPosition) {
                UpdateToNextWaypoint();
            } else {
                transform.position = newPosition;
            }
        }

        public void TakeDamage() {
            if (health < 1)
                return;
            health--;
            if (health < 1) {
                GameSystems.GetSystem<GameStateSystem>().Insanity.Value -= insanityReduction;
                Kill().Forget();
            }
        }

        public async UniTaskVoid Kill() {
            var animTimeMiliseconds = 0;
            //todo perform animation
            await UniTask.Delay(animTimeMiliseconds);
            Destroy(gameObject);
        }
    }
}
