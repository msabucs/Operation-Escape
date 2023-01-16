using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {

    [HideInInspector] public int tileNum, randomVariant, numtileVarIndex, sceneIndex;
    public int width, height;
    public WallTile wallTile;
    public NormalTile normalTile;
    public GoalTile goalTile;
    public NumberTile numberTile;
    public SwitchTile switchTile;
    public List<int> tileType;
    public List<int> numTileVar0, numTileVar1, numTileVar2;

    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        if (sceneIndex != 2) {
            randomVariant = Random.Range(0,3);
        } 
        else {
            randomVariant = 0;
        }

        CreateGrid();
    }

    void CreateGrid() {

        for (int x = 0; x < width; x++) {

             for (int y = 0; y < height; y++) {

                // Spawn Normal Tile
                if (tileType[tileNum] == 1) {
                    var spawnedTile = Instantiate(normalTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                // Spawn Goal Tile
                else if (tileType[tileNum] == 2) {
                    var spawnedTile = Instantiate(goalTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                // Spawn Number Tile
                else if (tileType[tileNum] == 3) {

                    var spawnedTile = Instantiate(numberTile, new Vector3(x-2, y-3), Quaternion.identity);

                    if (randomVariant == 0) {
                        spawnedTile.number = numTileVar0[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar0[numtileVarIndex];
                    }
                    else if (randomVariant == 1) {
                        spawnedTile.number = numTileVar1[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar1[numtileVarIndex];
                    } 
                    else {
                        spawnedTile.number = numTileVar2[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar2[numtileVarIndex];
                    }
                    
                    numtileVarIndex++;                
                }
                // Spawn Switch Tile
                else if (tileType[tileNum] == 4) {
                    var spawnedTile = Instantiate(switchTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                // Spawn Wall Tile
                else {
                    var spawnedTile = Instantiate(wallTile, new Vector3(x-2, y-3), Quaternion.identity);
                }

                tileNum++;
            }
        }
    }
}
