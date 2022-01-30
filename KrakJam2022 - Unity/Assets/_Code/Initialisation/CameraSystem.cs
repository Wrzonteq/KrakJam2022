using Cinemachine;
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

        public override void Initialise() {
            CrosshairInstance = Instantiate(crosshairPrefab, MainCamera.transform, false);
            GameSystems.GetSystem<GameplaySystem>().PlayerInstantiatedEvent += HandlePlayerInstantiated;
            GameSystems.GetSystem<GameStateSystem>().Stage.ChangedValue += HandleStageChanged;
        }

        void HandleStageChanged(GameStage stage) {
            virtualCamera.m_Lens.OrthographicSize = stage == GameStage.Sanity ? camSizeWhenSane : camSizeWhenInsane;
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
    }
}
