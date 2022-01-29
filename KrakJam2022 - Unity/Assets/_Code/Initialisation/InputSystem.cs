using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InputSystem : BaseGameSystem {
        public InputMaps Bindings { get; private set; }


        public override void OnCreate() {
            Bindings = new InputMaps();
            DisableInput();
        }

        public override void Initialise() { }

        public void DisableInput() {
            Bindings.Disable();
        }

        public void SwitchToInterfaceInput() {
            Bindings.Gameplay.Disable();
            Bindings.Interface.Enable();
        }

        public void SwitchToGameplayInput() {
            Cursor.visible = false;
            Bindings.Interface.Disable();
            Bindings.Gameplay.Enable();
        }
    }
}
