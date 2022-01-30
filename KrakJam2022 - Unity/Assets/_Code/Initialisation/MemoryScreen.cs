using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class MemoryScreen : UiScreenBase {
        public event Action ScreenClosedEvent;

        [SerializeField] Image memoryImage;
        [SerializeField] Image iconImage;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Sprite positiveIcon;
        [SerializeField] Sprite negativeIcon;


        public async UniTaskVoid DisplayMemory(MemoryData memory) {
            var input = GameSystems.GetSystem<InputSystem>();
            input.DisableInput();
            memoryImage.gameObject.SetActive(memory.sprite != null);
            memoryImage.sprite = memory.sprite;
            text.text = memory.description;
            iconImage.sprite = memory.type == MemoryType.Positive ? positiveIcon : negativeIcon;
            uiSystem.GetScreen<HUDScreen>().Hide().Forget();
            await Show();
            input.SwitchToInterfaceInput();
            input.Bindings.Interface.Submit.performed += HandleInput;
            input.Bindings.Interface.Cancel.performed += HandleInput;
            input.Bindings.Interface.Continue.performed += HandleInput;
        }

        void HandleInput(InputAction.CallbackContext _) {
            var input = GameSystems.GetSystem<InputSystem>();
            input.Bindings.Interface.Submit.performed -= HandleInput;
            input.Bindings.Interface.Cancel.performed -= HandleInput;
            input.Bindings.Interface.Continue.performed -= HandleInput;
            input.DisableInput();
            Close().Forget();
        }

        async UniTaskVoid Close() {
            await Hide();
            await uiSystem.GetScreen<HUDScreen>().Show();
            GameSystems.GetSystem<InputSystem>().SwitchToGameplayInput();
            ScreenClosedEvent?.Invoke();
        }
    }
}
