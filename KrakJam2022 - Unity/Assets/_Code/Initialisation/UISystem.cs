using PartTimeKamikaze.KrakJam2022.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class UISystem : BaseGameSystem {
        [SerializeField] Canvas mainCanvasPrefab;
        [SerializeField] LoadingScreen loadingScreenPrefab;

        public Canvas MainCanvas { get; private set; }
        public LoadingScreen LoadingScreen { get; private set; }


        protected override void OnInitialise() {
            MainCanvas = Instantiate(mainCanvasPrefab);
            LoadingScreen = Instantiate(loadingScreenPrefab, MainCanvas.transform, false);
            LoadingScreen.Hide();
        }
    }
}
