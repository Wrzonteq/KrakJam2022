using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public class SanityProgressBar : MonoBehaviour {
        [SerializeField]
        SpriteRenderer sanityBar;

        [SerializeField]
        SpriteMask sanityProgressMask;

        void Start() {
            // var playerDataSystem = GameSystems.GetSystem<PlayerDataSystem>();
            // playerDataSystem.Sanity.ChangedValue += ChangeSanityProgressBar;

            // mask should cover whole progress bar in the prefab
            sanityProgressMask.transform.position = new Vector3(sanityBar.size.x, 0, 0);
            // put the mask after the bar
        }

        /* for testing
        int testingProgressBar = 102;
        float miliseconds = 0;
        private void Update() {
            miliseconds += Time.deltaTime;
            if (miliseconds > 1) {
                testingProgressBar -= 1;
                miliseconds -= 1;
                ChangeSanityProgressBar(testingProgressBar);
                if (testingProgressBar < 0) testingProgressBar = 100;
            }
        }
        */

        void ChangeSanityProgressBar(int progress) {
            float maskTranslate = (sanityBar.size.x / 100f) * progress;
            sanityProgressMask.transform.position = new Vector3(maskTranslate, sanityBar.transform.position.y, sanityBar.transform.position.z);
        }
    }
}
