using DG.Tweening;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MusicSystem : BaseGameSystem {
        [SerializeField] AudioSource musicSource;
        [SerializeField] float maxVolume = .6f;


        public override void OnCreate() { }

        public override void Initialise() {
            PlayMusic();
        }

        void PlayMusic() {
            musicSource.volume = 0;
            musicSource.Play();
            DOTween.To(() => musicSource.volume, x => musicSource.volume = x, maxVolume, 1);
        }
    }
}
