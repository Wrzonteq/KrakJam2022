using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EnemiesSystem : BaseGameSystem {

        [System.Serializable]
        public class EnemySettings {
            public EnemySettings(Enemy enemy, int hp, int damage) {
                this.enemy = enemy;
                this.hp = hp;
                this.damage = damage;
            }

            [SerializeField]
            public Enemy enemy;

            [SerializeField]
            public int hp;

            [SerializeField]
            public int damage;
        }

        [SerializeField]
        List<EnemySettings> enemySettings = new List<EnemySettings>();

        public override void OnCreate() { }

        public override void Initialise() {
            CalculateSpawnPoints();
            SpawnEnemy();
        }

        private List<Waypoint> spawningWaypoints;
        private void CalculateSpawnPoints() {
            if (spawningWaypoints == null) {
                spawningWaypoints = FindObjectsOfType<Waypoint>().Where(w => w.spawnPoint).ToList();
            }
        }
        float planeZ = 0f;

        private void SpawnEnemy() {
            Waypoint spawningWaypoint = spawningWaypoints[Random.Range(0, spawningWaypoints.Count)];
            EnemySettings settings = enemySettings[Random.Range(0, enemySettings.Count)];
            Enemy enemy = Instantiate(settings.enemy, Camera.main.transform);
            enemy.Initialize(spawningWaypoint);
            enemy.transform.position = new Vector3(
                spawningWaypoint.transform.position.x,
                spawningWaypoint.transform.position.y,
                planeZ
                );
        }

        public void StartEnemySpawning() {
            //todo 
        }

        public void StopEnemySpawning() {
            //todo stop spawning + kill remaining enemies
        }
    }
}
