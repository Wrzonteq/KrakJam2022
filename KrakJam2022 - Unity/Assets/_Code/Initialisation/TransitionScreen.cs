using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class TransitionScreen : UiScreenBase {
        [SerializeField] Image black;
        int fadeDurationMiliseconds = 200;
        int delayMiliseconds = 500;

        protected override float FadeInDuration => 0;
        protected override float FadeOutDuration => 0;


        protected override void OnInitialise() {
            black.DOFade(0, 0);
        }

        public async UniTask FadeInAndOut(Action callbackAfterFadeOut, Action callbackAfterFadeIn) {
            black.DOFade(1, fadeDurationMiliseconds/1000f);
            await UniTask.Delay(fadeDurationMiliseconds);
            callbackAfterFadeOut?.Invoke();
            await UniTask.Delay(delayMiliseconds);
            black.DOFade(0, fadeDurationMiliseconds);
            await UniTask.Delay(fadeDurationMiliseconds);
            callbackAfterFadeIn?.Invoke();
        }
    }
}
