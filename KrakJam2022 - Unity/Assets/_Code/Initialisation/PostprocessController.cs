using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PostprocessController : MonoBehaviour {
        [SerializeField] Volume insanityVolume;
        [SerializeField] Volume sanityVolume;


        public void SetStage(GameStage stage) {
            var insanityValue = stage == GameStage.Insanity ? 1 : 0;
            var sanityValue = stage == GameStage.Sanity ? 1 : 0;
            DOTween.To(() => insanityVolume.weight, x => insanityVolume.weight = x, insanityValue, 1);
            DOTween.To(() => sanityVolume.weight, x => sanityVolume.weight = x, sanityValue, 1);
        }
    }
}
