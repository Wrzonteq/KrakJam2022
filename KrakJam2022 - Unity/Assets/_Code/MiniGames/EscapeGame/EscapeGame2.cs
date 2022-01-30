using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EscapeGame2 : Minigame {
        Transform playerTransform;

        [SerializeField] Transform entrancePoint;
        [SerializeField] CollectibleMemory positiveMemory;
        [SerializeField] CollectibleMemory negativeMemory;
        [SerializeField] Tilemap tilemap;

        BoundsInt tileMapSettings;
        bool runningLevel = false;
        bool collapsingFloor = false;
        float miliseconds = 0;

        public override void Initialise() {
            positiveMemory.gameObject.SetActive(false); // show it after floor starts collapsing

            negativeMemory.MemoryCollectedEvent += StartCollapsingFloor;
            positiveMemory.MemoryOpenedEvent += ShowExitAndStopCollapsing;

            tilemap.CompressBounds();  // to recalculate cell bounds
            tileMapSettings = tilemap.cellBounds;
            StoreOriginalTileMap();
            ResetFloorState();
        }

        public override void OnPlayerEnterLevel() {
            runningLevel = true;
            playerTransform = GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform;
        }

        void ExitMiniGame() {
            runningLevel = false;
            RestoreOriginalTileMap();
        }

        void Update() {
            if (!runningLevel) return;

            miliseconds += Time.deltaTime;
            if (miliseconds > 0.3f) { // timer for collapsing floor
                miliseconds -= 0.3f;
                FollowThePlayer();
            }
            CheckIfPlayerStoppedOnCollapsedTileOrWall();
        }

        void CheckIfPlayerStoppedOnCollapsedTileOrWall() {
            Vector3Int currentTilePosition = tilemap.WorldToCell(playerTransform.transform.position);
            TileBase tile = tilemap.GetTile(currentTilePosition);
            if (tile == null) {
                PlayerFailedMiniGame();
            }
        }

        void PlayerFailedMiniGame() {
            ResetFloorState();
            MovePlayerToTeleport(entrancePoint);
        }

        void StartCollapsingFloor(MemoryData memoryData) {
            collapsingFloor = true;
        }

        void ShowExitAndStopCollapsing(MemoryData memoryData) {
            collapsingFloor = false;
            RestoreOriginalTileMap();
            runningLevel = false;
        }

        private void MovePlayerToTeleport(Transform target) {
            playerTransform.transform.position = target.position;
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
            positiveMemory.gameObject.SetActive(false); // when snake is big -> show the exit
        }

        private void FollowThePlayer() {
            if (!collapsingFloor)
                return;

            Vector3Int playerTile = tilemap.WorldToCell(playerTransform.transform.position);
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
