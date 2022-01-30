using TMPro;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class MessagePopup : MonoBehaviour {
        [SerializeField] TMP_Text text;

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void SetText(string text) {
            this.text.text = text;
        }
    }
}