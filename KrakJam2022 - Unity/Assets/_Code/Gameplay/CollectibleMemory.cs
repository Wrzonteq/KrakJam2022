using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CollectibleMemory : Collectable {
        [SerializeField] MemoryData memoryData;

        protected override void OnInteract() {
            GameSystems.GetSystem<UISystem>().GetScreen<MemoryScreen>().DisplayMemory(memoryData).Forget();
        }
    }
}
