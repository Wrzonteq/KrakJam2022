using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        public static PlayerController Instance { get; private set; }

        [SerializeField] Rigidbody2D selfRigidbody2D;
        [SerializeField] private int movementSpeed = 10;

        Vector3 move;
        
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
        
        // Update is called once per frame
        void Update() {
            selfRigidbody2D.MovePosition(transform.position + move * movementSpeed / 10.0f);
            //this.transform.Translate(move * (movementSpeed * Time.deltaTime));
        }
        
        public void OnMove(InputValue value) {
            move = value.Get<Vector2>();
            Debug.Log(move);
        }
    }
}
    
