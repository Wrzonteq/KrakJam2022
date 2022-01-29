using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class SanityDisplay : MonoBehaviour {
        [SerializeField] Image fillImage;


        public void SetFill(float value) {
            fillImage.fillAmount = value;
        }
    }
}