using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public abstract class Minigame : MonoBehaviour {
        public abstract void Initialise();
        public virtual void OnPlayerEnterLevel() { }
    }
}