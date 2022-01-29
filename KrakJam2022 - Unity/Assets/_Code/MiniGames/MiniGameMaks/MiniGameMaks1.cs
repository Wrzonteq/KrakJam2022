using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PartTimeKamikaze.KrakJam2022 {
    public class MiniGameMaks1 : MonoBehaviour {
        [SerializeField]
        SpriteRenderer mainCharacter;

        [SerializeField]
        MiniGameTeleport enterTeleport;

        [SerializeField]
        MiniGameTeleport teleport1;

        [SerializeField]
        MiniGameTeleport teleport2;

        [SerializeField]
        MiniGameTeleport teleport3;

        [SerializeField]
        MiniGameTeleport teleport4;

        [SerializeField]
        MiniGameTeleport exitTeleport;

        [SerializeField]
        Tilemap tilemap;

        BoundsInt tileMapSettings;

        void Start() {
            teleport1.AddCollisionCallback(TouchTeleport1);
            //teleport2.AddCollisionCallback(TouchTeleport2);
            teleport3.AddCollisionCallback(TouchTeleport3);
            exitTeleport.AddCollisionCallback(ExitMiniGame);
            tilemap.CompressBounds();  // to recalculate cell bounds
            tileMapSettings = tilemap.cellBounds;
            Debug.Log(tileMapSettings.size.x);
            Debug.Log(tileMapSettings.max.x);
            StartMiniGame();
        }

        bool collapsing = false;
        private void StartMiniGame() {
            StoreOriginalTileMap();
            ResetFloorState();
            collapsing = true;

        }

        private void ExitMiniGame() {
            collapsing = false;
            RestoreOriginalTileMap();
        }

        float miliseconds = 0;
        private void Update() {
            if (!collapsing) return;

            miliseconds += Time.deltaTime;
            if (miliseconds > 0.4f) { // every second - maybe less?
                miliseconds -= 0.4f;
                CollapseTheFloorByCol();
            }
            CheckIfPlayerStoppedOnCollapsedTile();
        }

        private void CheckIfPlayerStoppedOnCollapsedTile() {
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

        private void TouchTeleport1() {
            MovePlayerToTeleport(teleport2);
        }

        private void TouchTeleport3() {
            MovePlayerToTeleport(teleport4);
        }

        private void MovePlayerToTeleport(MiniGameTeleport teleport) {
            mainCharacter.transform.position = new Vector3(teleport.transform.position.x, teleport.transform.position.y, teleport.transform.position.z);
        }

        Dictionary<Vector3Int, TileBase> originalTilemap = new Dictionary<Vector3Int, TileBase>();
        private void StoreOriginalTileMap() {
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
            RestoreOriginalTileMap();

            if (collapsedTilesCol >= tileMapSettings.max.x - 1) {
                leftToRight = false;  // turned the collapsing the other way
            }

            if (collapsedTilesCol <= tileMapSettings.min.x) {
                leftToRight = true;  // turned the collapsing the other way
            }

            // floor is disappearing from right to left and then from left to right
            for (int y = tileMapSettings.yMin; y <= tileMapSettings.yMax; y++) {
                tilemap.SetTile(new Vector3Int(collapsedTilesCol, y), null);
            }

            collapsedTilesCol = leftToRight ? collapsedTilesCol + 1 : collapsedTilesCol - 1;
        }
    }
}
