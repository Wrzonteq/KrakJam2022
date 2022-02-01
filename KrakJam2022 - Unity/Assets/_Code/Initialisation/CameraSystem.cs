using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class CameraSystem : BaseGameSystem {
        [SerializeField] Camera mainCameraPrefab;
        [SerializeField] CrosshairController crosshairPrefab;
        [SerializeField] float camSizeWhenSane = 5;
        [SerializeField] float camSizeWhenInsane = 8;

        CinemachineVirtualCamera virtualCamera;
        PostprocessController postprocesses;

        public Camera MainCamera { get; private set; }
        public CrosshairController CrosshairInstance { get; private set; }


        public override void OnCreate() {
            MainCamera = Instantiate(mainCameraPrefab);
        }

        void OnDestroy() {
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue -= HandleStageChanged;
            GameSystems.GetSystem<GameplaySystem>().PlayerInstantiatedEvent -= HandlePlayerInstantiated;
        }

        public override void Initialise() {
            CrosshairInstance = Instantiate(crosshairPrefab, MainCamera.transform, false);
            GameSystems.GetSystem<GameplaySystem>().PlayerInstantiatedEvent += HandlePlayerInstantiated;
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            var targetOrtho = stage == GameStage.Sanity ? camSizeWhenSane : camSizeWhenInsane;
            DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x, targetOrtho, .6f);
            virtualCamera.m_Lens.OrthographicSize = targetOrtho;
            postprocesses.SetStage(stage);
        }

        void HandlePlayerInstantiated(PlayerController player) {
            virtualCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
            virtualCamera.m_Lens.OrthographicSize = camSizeWhenSane;
            postprocesses = FindObjectOfType<PostprocessController>();
        }

        public void SetupCrosshairFollowTarget(Transform followTarget) {
            CrosshairInstance.Setup(MainCamera, followTarget);
        }

        public async UniTaskVoid FocusOnMe(Transform target) {
            GameSystems.GetSystem<InputSystem>().DisableInput();
            var followBackup = virtualCamera.Follow;
            virtualCamera.Follow = target;
            await UniTask.Delay(2000);
            virtualCamera.Follow = followBackup;
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
        }
    }
}
