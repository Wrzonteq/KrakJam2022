using System;
using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CollectibleMemory : Collectable {
        public event Action<MemoryData> MemoryOpenedEvent;
        public event Action<MemoryData> MemoryCollectedEvent;

        [SerializeField] MemoryData memoryData;
        

        protected override void OnInteract() {
            MemoryOpenedEvent?.Invoke(memoryData);
            var memoryScreen = GameSystems.GetSystem<UISystem>().GetScreen<MemoryScreen>();
            memoryScreen.DisplayMemory(memoryData).Forget();
            memoryScreen.ScreenClosedEvent += HandleMemoryScreenClosed;
        }

        void HandleMemoryScreenClosed() {
            GameSystems.GetSystem<UISystem>().GetScreen<MemoryScreen>().ScreenClosedEvent -= HandleMemoryScreenClosed;
            MemoryCollectedEvent?.Invoke(memoryData);
        }

        public void ShowAndFocusCameraOn() {
            gameObject.SetActive(true);
            GameSystems.GetSystem<CameraSystem>().FocusOnMe(transform).Forget();
        }
    }
}
