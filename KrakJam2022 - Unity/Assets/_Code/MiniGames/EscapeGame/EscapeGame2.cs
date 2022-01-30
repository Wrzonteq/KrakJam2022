using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EscapeGame2 : Minigame {
        Transform mainCharacter;

        [SerializeField]
        EscapeGameTeleport enterTeleport;

        [SerializeField]
        EscapeGameTeleport exitTeleport;

        [SerializeField] CollectibleMemory positiveMemory;

        [SerializeField] CollectibleMemory negativeMemory;

        [SerializeField]
        Tilemap tilemap;

        BoundsInt tileMapSettings;
        bool runningLevel = false;
        bool collapsingFloor = false;

        public override void Initialise() {
            exitTeleport.AddCollisionCallback(ExitMiniGame);
            exitTeleport.gameObject.SetActive(false);

            positiveMemory.gameObject.SetActive(false); // show it after floor starts collapsing
            positiveMemory.MemoryCollectedEvent += ShowExitAndStopCollapsing;

            negativeMemory.MemoryCollectedEvent += StartCollapsingFloor;

            tilemap.CompressBounds();  // to recalculate cell bounds
            tileMapSettings = tilemap.cellBounds;
            StoreOriginalTileMap();
            ResetFloorState();
            collapsingFloor = false;
            runningLevel = true;
            //MovePlayerToTeleport(enterTeleport);
            mainCharacter = GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform;
        }

        private void ExitMiniGame() {
            runningLevel = false;
            RestoreOriginalTileMap();
        }

        float miliseconds = 0;
        private void Update() {
            if (!runningLevel) return;

            miliseconds += Time.deltaTime;
            if (miliseconds > 0.3f) { // timer for collapsing floor
                miliseconds -= 0.3f;
                FollowThePlayer();
            }
            CheckIfPlayerStoppedOnCollapsedTileOrWall();
        }

        private void CheckIfPlayerStoppedOnCollapsedTileOrWall() {
            Vector3Int currentTilePosition = tilemap.WorldToCell(mainCharacter.transform.position);
            TileBase tile = tilemap.GetTile(currentTilePosition);
            if (tile == null) {
                PlayerFailedMiniGame();
            }
        }

        private void PlayerFailedMiniGame() {
            ResetFloorState();
            MovePlayerToTeleport(enterTeleport);
        }

        private void StartCollapsingFloor(MemoryData memoryData) {
            Debug.Log("start collapsing");
            collapsingFloor = true;
        }

        private void ShowExitAndStopCollapsing(MemoryData memoryData) {
            collapsingFloor = false;
            RestoreOriginalTileMap();
            exitTeleport.gameObject.SetActive(true);
        }

        private void MovePlayerToTeleport(EscapeGameTeleport teleport) {
            mainCharacter.transform.position = new Vector3(teleport.transform.position.x, teleport.transform.position.y, teleport.transform.position.z);
        }

        Dictionary<Vector3Int, TileBase> originalTilemap = new Dictionary<Vector3Int, TileBase>();
        private void StoreOriginalTileMap() {
            if (originalTilemap.Count > 0) return;

            for (int x = tileMapSettings.xMin; x <= tileMapSettings.xMax; x++) {
                for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                    Vector3Int vector3Int = new Vector3Int(x, y, 0);
                    originalTilemap.Add(vector3Int, tilemap.GetTile(vector3Int));

                }
            }
        }

        private void RestoreOriginalTileMap() {
            for (int x = tileMapSettings.xMin; x <= tileMapSettings.xMax; x++) {
                for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                    Vector3Int vector3Int = new Vector3Int(x, y, 0);
                    tilemap.SetTile(vector3Int, originalTilemap[vector3Int]);
                }
            }
        }

        List<Vector3Int> snake1;
        List<Vector3Int> snake2;
        List<Vector3Int> snake3;
        private void ResetFloorState() {
            snake1 = new List<Vector3Int>() {
                new Vector3Int(tileMapSettings.min.x, tileMapSettings.min.y)
            };
            snake2 = new List<Vector3Int>() {
                new Vector3Int(tileMapSettings.max.x - 1, tileMapSettings.max.y - 1)
            };
            snake3 = new List<Vector3Int>() {
                new Vector3Int(tileMapSettings.max.x - 1, tileMapSettings.min.y)
            };
            positiveMemory.gameObject.SetActive(false); // when snake is big show the exit
        }

        private void FollowThePlayer() {
            if (!collapsingFloor)
                return;

            Vector3Int playerTile = tilemap.WorldToCell(mainCharacter.transform.position);
            MoveSnake(playerTile, snake1);
            MoveSnake(playerTile, snake2);
            MoveSnake(playerTile, snake3);

            RestoreOriginalTileMap();

            foreach (Vector3Int vector3Int in snake1) {
                tilemap.SetTile(vector3Int, null);
            }
            foreach (Vector3Int vector3Int in snake2) {
                tilemap.SetTile(vector3Int, null);
            }
            foreach (Vector3Int vector3Int in snake3) {
                tilemap.SetTile(vector3Int, null);
            }
        }

        private void MoveSnake(Vector3Int playerTile, List<Vector3Int> snake) { 
            Vector3Int lastSnakeTile = snake.Last();

            List<Vector3Int> possibleNextTiles = new List<Vector3Int>();

            possibleNextTiles.Add(new Vector3Int(lastSnakeTile.x - 1, lastSnakeTile.y));
            possibleNextTiles.Add(new Vector3Int(lastSnakeTile.x + 1, lastSnakeTile.y));
            possibleNextTiles.Add(new Vector3Int(lastSnakeTile.x, lastSnakeTile.y - 1));
            possibleNextTiles.Add(new Vector3Int(lastSnakeTile.x, lastSnakeTile.y + 1));
            possibleNextTiles = possibleNextTiles.OrderBy(x => (x - playerTile).sqrMagnitude).ToList();

            Vector3Int nextTile = possibleNextTiles.First();
            bool foundNextTile = false;
            foreach (Vector3Int possibleNextTile in possibleNextTiles) {
                if (tilemap.GetTile(possibleNextTile) != null) {  //snake is not already there
                    nextTile = possibleNextTile;
                    foundNextTile = true;
                    break;
                }
            }
            if (!foundNextTile) return; // player was too smart

            if (snake.Count > 8) {
                positiveMemory.gameObject.SetActive(true); // when snake is big show the exit
                snake.RemoveAt(0);
            }
            snake.Add(nextTile);
        }
    }
}
