using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {
    public class PlayerController : MonoBehaviour {
        public static PlayerController Instance { get; private set; }
        
        [SerializeField] private int movementSpeed = 10;

        Vector2 move;
        
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }
        
        // Update is called once per frame
        void Update() {
            this.transform.Translate(move * (movementSpeed * Time.deltaTime));
        }
        
        public void OnMove(InputValue value) {
            move = value.Get<Vector2>();
            Debug.Log(move);
        }
    }
}
    
