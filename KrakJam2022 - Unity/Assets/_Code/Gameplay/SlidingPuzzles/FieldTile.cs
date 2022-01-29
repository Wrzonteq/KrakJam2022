using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022.SlidingPuzzles {
    public class FieldTile : MonoBehaviour {
        public int CurrentPuzzleId { get; private set; }
        public PuzzleElement CurrentPuzzle { get; private set; }
        public Vector2 Position { get; private set; }
        
        SlidingPuzzleController board;
        
        public void SetBoard(SlidingPuzzleController controller) {
            board = controller;
        }
        
        void OnTriggerStay2D(Collider2D col) {
            if (col.gameObject.tag == "SlidingPuzzleBlock") {
                CurrentPuzzle = col.gameObject.GetComponent<PuzzleElement>();
                CurrentPuzzleId = CurrentPuzzle.Id;
            }
        }

        void OnTriggerExit2D(Collider2D col) {
            CurrentPuzzleId = 0;
            
            if (col.gameObject.tag == "SlidingPuzzleBlock") {
                StartCoroutine(board.CheckWinCondition());
            }
        }

        public void SetPosition(int row, int col) {
            Position = new Vector2(row, col);
        }
    }
}
