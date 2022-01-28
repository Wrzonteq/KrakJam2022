using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class LoadingScreen : MonoBehaviour {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Image loadingBarFill;

        int roundedProgress;


        public void Show() {
            gameObject.SetActive(true);
        }

        public void SetProgress(float progress) {
            var percentage = Mathf.CeilToInt(progress * 100);
            if (percentage == roundedProgress)
                return; // to optimise gui redraws
            roundedProgress = percentage;
            label.text = $"{roundedProgress}%";
            loadingBarFill.fillAmount = progress;
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}