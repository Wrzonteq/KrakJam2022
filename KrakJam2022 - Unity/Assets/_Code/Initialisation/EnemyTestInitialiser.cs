using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EnemyTestInitialiser : MonoBehaviour {
        [SerializeField] BaseGameSystem[] systemPrefabs;
        [SerializeField] Transform systemsRoot;


        void Awake() {
            InitialiseGameSystems();
        }

        void InitialiseGameSystems() {
            var systemsInstances = new List<BaseGameSystem>();
            DontDestroyOnLoad(systemsRoot.gameObject);
            foreach (var prefab in systemPrefabs) {
                var instance = Instantiate(prefab, systemsRoot, false);
                instance.OnCreate();
                systemsInstances.Add(instance);
            }

            foreach (var instance in systemsInstances) {
                instance.Initialise();
            }

            GameSystems.Init(systemsInstances);
        }
    }
}

