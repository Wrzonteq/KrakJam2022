using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PartTimeKamikaze.KrakJam2022 {

    public class CrosshairController : MonoBehaviour {
        // Start is called before the first frame update
        [SerializeField] Transform playerTransform;
        [SerializeField] Transform followPointTransform;
        
        // Update is called once per frame
        void Update() {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);
            followPointTransform.localPosition = transform.localPosition / 4;
        }
    }
}
