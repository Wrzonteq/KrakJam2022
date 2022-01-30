using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InputSystem : BaseGameSystem {
        public bool PlayerInputEnabled { get; private set; }
        public bool MouseLookEnabled { get; private set; }
        public InputMaps Bindings { get; private set; }


        public override void OnCreate() {
            Bindings = new InputMaps();
            DisableInput();
        }

        public override void Initialise() { }

        public void DisableInput() {
            PlayerInputEnabled = false;
            MouseLookEnabled = false;
            Bindings.Disable();
        }

        public void SwitchToInterfaceInput() {
            Bindings.Gameplay.Disable();
            Bindings.Interface.Enable();
            PlayerInputEnabled = false;
        }

        public void SwitchToGameplayInput() {
            PlayerInputEnabled = true;
            MouseLookEnabled = true;
            Cursor.visible = false;
            Bindings.Interface.Disable();
            Bindings.Gameplay.Enable();
        }
    }
}
