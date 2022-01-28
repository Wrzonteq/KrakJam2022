using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class LoadingScreen : MonoBehaviour {
        [SerializeField] TextMeshProUGUI label;
        [SerializeField] Image loadingBarFill;

        int lastProgress = -1;


        public void Show() {
            gameObject.SetActive(true);
        }

        public void SetProgress(float progress) {
            var percentage = Mathf.CeilToInt(progress * 100);
            if (percentage == lastProgress)
                return; // to optimise gui redraws
            lastProgress = percentage;
            label.text = $"{lastProgress}%";
            loadingBarFill.fillAmount = progress;
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
