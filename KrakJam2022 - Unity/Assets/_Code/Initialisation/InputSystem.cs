using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class InputSystem : BaseGameSystem {
        public InputActionAsset bindings;

        protected override void OnInitialise() {
            GameSystems.input = this;
        }
    }
}
