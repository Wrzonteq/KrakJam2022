using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EscapeGame1 : Minigame {
        Transform playerTransform;

        [SerializeField] Transform entrancePoint;
        [SerializeField] CollectibleMemory positiveMemory;
        [SerializeField] CollectibleMemory negativeMemory;
        [SerializeField] Tilemap tilemap;

        BoundsInt tileMapSettings;
        bool runningLevel = false;
        bool collapsingFloor = false;
        int collapsedTilesCol;
        float miliseconds = 0;
        bool leftToRight;
        Dictionary<Vector3Int, TileBase> originalTilemap = new Dictionary<Vector3Int, TileBase>();


        public override void Initialise() {
            positiveMemory.gameObject.SetActive(false); // show it after floor starts collapsing

            negativeMemory.MemoryCollectedEvent += StartCollapsingFloor;
            positiveMemory.MemoryOpenedEvent += ShowExitAndStopCollapsing;

            tilemap.CompressBounds(); // to recalculate cell bounds
            tileMapSettings = tilemap.cellBounds;
            StoreOriginalTileMap();
            ResetFloorState();
            collapsingFloor = false;
        }

        public override void OnPlayerEnterLevel() {
            runningLevel = true;
            playerTransform = GameSystems.GetSystem<GameplaySystem>().PlayerInstance.transform;
        }

        void Update() {
            if (!runningLevel)
                return;

            miliseconds += Time.deltaTime;
            if (miliseconds > 0.4f) { // timer for collapsing floor
                miliseconds -= 0.4f;
                CollapseTheFloorByCol();
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
            Debug.Log("start collapsing");
            collapsingFloor = true;
        }

        void ShowExitAndStopCollapsing(MemoryData memoryData) {
            collapsingFloor = false;
            RestoreOriginalTileMap();
            runningLevel = false;
        }

        void MovePlayerToTeleport(Transform target) {
            playerTransform.transform.position = target.position;
        }

        void StoreOriginalTileMap() {
            if (originalTilemap.Count > 0)
                return;

            for (int x = tileMapSettings.xMin; x <= tileMapSettings.xMax; x++) {
                for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                    Vector3Int vector3Int = new Vector3Int(x, y, 0);
                    originalTilemap.Add(vector3Int, tilemap.GetTile(vector3Int));

                }
            }
        }

        void RestoreOriginalTileMap() {
            for (int x = tileMapSettings.xMin; x <= tileMapSettings.xMax; x++) {
                for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                    Vector3Int vector3Int = new Vector3Int(x, y, 0);
                    tilemap.SetTile(vector3Int, originalTilemap[vector3Int]);
                }
            }
        }

        void ResetFloorState() {
            leftToRight = true;
            collapsedTilesCol = tileMapSettings.min.x;
        }

        void CollapseTheFloorByCol() {
            if (!collapsingFloor)
                return;

            RestoreOriginalTileMap();

            Vector3Int playerTile = tilemap.WorldToCell(playerTransform.transform.position);

            leftToRight = collapsedTilesCol < playerTile.x; // collapsing direction towards player
            if (!leftToRight) {                             // when collapsing changes first direction
                positiveMemory.gameObject.SetActive(true);
            }

            // floor is disappearing from right to left and then from left to right
            for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                tilemap.SetTile(new Vector3Int(collapsedTilesCol, y), null);
            }

            collapsedTilesCol = leftToRight ? collapsedTilesCol + 1 : collapsedTilesCol - 1;
        }
    }
}
