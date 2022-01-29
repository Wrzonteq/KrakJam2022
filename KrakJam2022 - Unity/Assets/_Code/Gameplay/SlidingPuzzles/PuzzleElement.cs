using TMPro;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.SlidingPuzzles {
    public class PuzzleElement : MonoBehaviour {
        public BoxCollider2D selfBoxCollider2D;
        public TextMeshPro text;

        public int Id { get; private set; }

        bool finishingAnimation = false;
        Vector2 desiredScale = new Vector2(2, 2);
        Vector2 desiredPos;
        
        public void Update() {
            if (finishingAnimation) {
                transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, 4f * Time.deltaTime);
                transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, 4f * Time.deltaTime);

                if (Vector2.Distance(transform.localPosition, desiredPos) < 0.0001f && Vector2.Distance(transform.localScale, desiredScale) < 0.0001f)
                    finishingAnimation = false;
            }
        }

        public void SetId(int id) {
            this.Id = id;
            text.text = id.ToString();
            // TODO: Set image
        }

        public void PlayEndSequence(Vector2 pos) {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            desiredPos = pos;
            finishingAnimation = true;
        }
    }
}
