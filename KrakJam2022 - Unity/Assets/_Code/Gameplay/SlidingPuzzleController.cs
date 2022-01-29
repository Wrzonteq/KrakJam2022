using System.Collections.Generic;
using PartTimeKamikaze.KrakJam2022.SlidingPuzzles;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2022 {
    public abstract class Minigame : MonoBehaviour {
        public abstract void Initialise();
    }

    public class SlidingPuzzleController : Minigame {
        [SerializeField] CollectibleMemory reward;

        [SerializeField] GameObject container;
        [SerializeField] GameObject background;
        [SerializeField] PuzzleElement puzzlePrefab;

        [SerializeField] GameObject blockadePrefab; 
        [SerializeField] FieldTile fieldPrefab;

        [SerializeField] int rowSize = 4;
        [SerializeField] Field[] rows;
        [SerializeField] Sprite[] sprites;

        List<PuzzleElement> puzzles = new();
        FieldTile[,] fields;

        bool won = false;

        public override void Initialise() {
            reward.gameObject.SetActive(false);
            int colSize = rows.Length / rowSize;
            
            background.transform.localScale = new Vector3(rowSize * 2, colSize * 2);
            background.transform.localPosition = new Vector2(rowSize - 1, -colSize + 1);
            fields = new FieldTile[colSize, rowSize];
            
            for (int i = 0; i < rows.Length; i++) {
                int col = i % rowSize;
                int row = Mathf.FloorToInt( i / rowSize);
                
                if (rows[i].state == FieldState.Disabled) {
                    SpawnMapObject(blockadePrefab, row, col);
                } else {
                    SpawnField(fieldPrefab, row, col);
                    
                    if (rows[i].state == FieldState.Occupied) {
                        SpawnPuzzleElement(rows[i].puzzleElementId, row, col);
                    }
                }
            }
        }

        public void SpawnPuzzleElement(int puzzleElementId, int row, int col) {
            PuzzleElement block = Instantiate(puzzlePrefab, container.transform);
            block.transform.localPosition = new Vector3(2 * col, -2 * row, 0);
            block.SetPuzzle(puzzleElementId, sprites[puzzleElementId - 1]);
            puzzles.Add(block);
        }

        public void SpawnField(FieldTile prefab, int row, int col) {
            FieldTile field = Instantiate(prefab, container.transform);
            field.transform.localPosition = new Vector3(2 * col, -2 * row, 0);
            field.SetBoard(this);
            field.SetPosition(row, col);
            fields[row, col] = field;
        }

        public void SpawnMapObject(GameObject prefab, int row, int col) {
            GameObject block = Instantiate(prefab, container.transform);
            block.transform.localPosition = new Vector3(2 * col, -2 * row, 0);
        }

        public void CheckWinCondition() {
            if (won) { return; }
            
            FieldTile currentField;
            
            for (int i = 0; i < fields.Length; i++) {
                int col = i % rowSize;
                int row = i / rowSize;

                currentField = fields[row, col];

                if (currentField == null || currentField.CurrentPuzzleId == 0) continue;
                
                if (currentField.CurrentPuzzleId > 1) { break; } // If starts with something else than 1, dont bother
                if (col == rowSize - 1) { break; } // Same if that's last element in row
                    
                if (fields[row, col + 1].CurrentPuzzleId == 2 &&
                    fields[row + 1, col].CurrentPuzzleId == 3 &&
                    fields[row + 1, col + 1].CurrentPuzzleId == 4) {
                    WinPuzzle(row, col);
                }
            }
        }

        public void WinPuzzle(int row, int col) {
            won = true;
            EnableReward();
            MovePuzzlesToPosition(row, col);
        }

        public void EnableReward() {
            reward.gameObject.SetActive(true);
        }

        public void MovePuzzlesToPosition(int row, int col) {
            FieldTile fieldTile;
            
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    fieldTile = fields[row + i, col + j];
                    fieldTile.CurrentPuzzle.PlayEndSequence(new Vector2(fieldTile.Position.y * 2, -fieldTile.Position.x * 2));
                }
            }
        }
    }
}
