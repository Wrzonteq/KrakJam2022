using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PartTimeKamikaze.KrakJam2022.UI {
    public class AuthorsScreen : UiScreenBase {
        [SerializeField] Button returnButton;

        protected override float FadeInDuration => 0;
        protected override float FadeOutDuration => 0;


        protected override void OnInitialise() {
            returnButton.onClick.AddListener(HandleReturnButton);
        }

        void HandleReturnButton() {
            Hide().Forget();
        }

        protected override void OnShow() {
            GameSystems.GetSystem<InputSystem>().Bindings.Interface.Cancel.performed += HandleCancelInput;
        }

        void HandleCancelInput(InputAction.CallbackContext _) {
            Hide().Forget();
        }

        protected override void OnHide() {
            GameSystems.GetSystem<InputSystem>().Bindings.Interface.Cancel.performed -= HandleCancelInput;
        }
    }
}
