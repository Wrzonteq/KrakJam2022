using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PartTimeKamikaze.KrakJam2022 {

    public class CrosshairController : MonoBehaviour {
        public static CrosshairController Instance { get; private set; }

        [SerializeField] Transform followPointTransform;
        
        protected void Awake()
        {
            if (Instance == null)
                Instance = this;

            gameObject.transform.parent = Camera.main.transform;
        }
        
        void Update() {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (Vector2.Distance(transform.position, mousePosition) > 0.01f) {
                transform.position = mousePosition;
                followPointTransform.localPosition = transform.localPosition / 4;
            }
        }
    }
}
