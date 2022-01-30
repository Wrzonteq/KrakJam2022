using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EnemiesSystem : BaseGameSystem {
        [SerializeField] List<EnemySettings> enemySettings = new List<EnemySettings>();

        bool waypointsInitialised;
        bool isSpawning;
        MapWaypoints allWaypoints;
        List<Waypoint> spawnPoints;

        public override void OnCreate() { }

        public override void Initialise() { }


        void CalculateSpawnPoints() {
            allWaypoints = FindObjectOfType<MapWaypoints>();
            spawnPoints = allWaypoints.waypoints.Where(x => x.spawnPoint).ToList();
            waypointsInitialised = true;
        }

        void SpawnEnemy() {
            var spawningWaypoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            var settings = enemySettings[Random.Range(0, enemySettings.Count)];
            var enemy = Instantiate(settings.enemy);
            enemy.Initialize(spawningWaypoint);
            enemy.transform.position = new Vector3(
                spawningWaypoint.transform.position.x,
                spawningWaypoint.transform.position.y,
                0f // TODO : maybe use const here
                );
        }

        public async UniTaskVoid StartEnemySpawning() {
            if(!waypointsInitialised)
                CalculateSpawnPoints();
            isSpawning = true;
            while (isSpawning) {
                SpawnEnemy();
                await UniTask.Delay(1000);
            }
        }

        public void StopEnemySpawning() {
            isSpawning = false;
            var enemies = FindObjectsOfType<Enemy>().Where(x => x.HealthPoints > 0).ToList();
            foreach(var enemy in enemies)
                enemy.Kill().Forget();
        }

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

#if UNITY_EDITOR

        public void Test() {
            CalculateSpawnPoints();
            StartEnemySpawning().Forget();
        }

#endif
    }
}
