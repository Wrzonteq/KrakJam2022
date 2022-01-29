using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

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

        public override void Initialise() { }

        public void Test() {
            CalculateSpawnPoints();
            StartEnemySpawning();
        }

        private List<Waypoint> spawningWaypoints;
        private void CalculateSpawnPoints() {
            if (spawningWaypoints == null) {
                spawningWaypoints = FindObjectsOfType<Waypoint>().Where(w => w.spawnPoint).ToList();
            }
        }

        private void SpawnEnemy() {
            Waypoint spawningWaypoint = spawningWaypoints[Random.Range(0, spawningWaypoints.Count)];
            EnemySettings settings = enemySettings[Random.Range(0, enemySettings.Count)];
            Enemy enemy = Instantiate(settings.enemy);
            enemy.Initialize(spawningWaypoint);
            enemy.transform.position = new Vector3(
                spawningWaypoint.transform.position.x,
                spawningWaypoint.transform.position.y,
                0f // TODO : maybe use const here
                );
        }

        bool isSpawning = false;
        public async UniTaskVoid StartEnemySpawning() {
            //todo 
            isSpawning = true;
            while (isSpawning) {
                SpawnEnemy();
                await UniTask.Delay(1000);
            }
        }

        public void StopEnemySpawning() {
            isSpawning = false;
            List<Enemy> enemies = FindObjectsOfType<Enemy>().ToList();
            foreach(Enemy enemy in enemies) {
                Destroy(enemy.gameObject);
            }
        }
    }
}
