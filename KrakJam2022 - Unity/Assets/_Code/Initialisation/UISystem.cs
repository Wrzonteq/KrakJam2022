using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class UISystem : BaseGameSystem {
        [SerializeField] Canvas mainCanvasPrefab;
        [SerializeField] UiScreenBase[] screensPrefabs;

        Dictionary<Type, UiScreenBase> screensInstances;

        public Canvas MainCanvas { get; private set; }


        public override void OnCreate() {
            MainCanvas = Instantiate(mainCanvasPrefab);
            screensInstances = new Dictionary<Type, UiScreenBase>();
            foreach (var sp in screensPrefabs) {
                var instance = Instantiate(sp, MainCanvas.transform, false);
                instance.Initialise();
                screensInstances[sp.GetType()] = instance;
            }
        }

        public override void Initialise() { }

        public T GetScreen<T>() where T : UiScreenBase {
            return (T)screensInstances[typeof(T)];
        }

        public UniTask ShowScreen <T>(bool instant = false) where T : UiScreenBase {
            var screen = GetScreen<T>();
            screen.transform.SetAsLastSibling();
            return screen.Show(instant);
        }
    }

    public class UiScreenBase : MonoBehaviour {
        [SerializeField] CanvasGroup canvasGroup;

        bool isInitialised;

        protected virtual float FadeInDuration => .5f;
        protected virtual float FadeOutDuration => .5f;

        public bool IsVisible { get; private set; }


        public async UniTask Show(bool instant = false) {
            gameObject.SetActive(true);
            OnShow();
            var animDuration = instant ? 0 : FadeInDuration;
            canvasGroup.DOFade(1, animDuration);
            await UniTask.Delay((int)(animDuration * 1000));
            IsVisible = true;
        }

        public async UniTask Hide(bool instant = false) {
            IsVisible = false;
            var animDuration = instant ? 0 : FadeOutDuration;
            canvasGroup.DOFade(0, animDuration);
            await UniTask.Delay((int) (animDuration * 1000));
            OnHide();
            gameObject.SetActive(false);
        }

        public void Initialise() {
            if (isInitialised)
                return;
            IsVisible = false;
            gameObject.SetActive(false);
            OnInitialise();
            isInitialised = true;
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        protected virtual void OnInitialise() { }
    }
}