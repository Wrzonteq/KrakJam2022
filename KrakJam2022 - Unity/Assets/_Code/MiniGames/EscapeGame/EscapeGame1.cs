using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PartTimeKamikaze.KrakJam2022 {
    public class EscapeGame1 : Minigame {
        Transform mainCharacter;

        [SerializeField]
        EscapeGameTeleport enterTeleport;

        [SerializeField]
        EscapeGameTeleport teleport1;

        [SerializeField]
        EscapeGameTeleport teleport2;

        [SerializeField]
        EscapeGameTeleport teleport3;

        [SerializeField]
        EscapeGameTeleport teleport4;

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
            teleport1.AddCollisionCallback(TouchTeleport1);
            teleport3.AddCollisionCallback(TouchTeleport3);
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
            if (miliseconds > 0.4f) { // timer for collapsing floor
                miliseconds -= 0.4f;
                CollapseTheFloorByCol();
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

        private void TouchTeleport1() {
            MovePlayerToTeleport(teleport2);
        }

        private void TouchTeleport3() {
            MovePlayerToTeleport(teleport4);
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

        int collapsedTilesCol;
        bool leftToRight;

        private void ResetFloorState() {
            leftToRight = true;
            collapsedTilesCol = tileMapSettings.min.x;
        }

        private void CollapseTheFloorByCol() {
            if (!collapsingFloor) return;

            RestoreOriginalTileMap();

            Vector3Int playerTile = tilemap.WorldToCell(mainCharacter.transform.position);

            leftToRight = collapsedTilesCol < playerTile.x; // collapsing direction towards player
            if(!leftToRight) { // when collapsing changes first direction
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
