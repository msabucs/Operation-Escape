using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    // 1 for wall tile
    // 2 for goal tile
    // 3 for number tile
    // 4 for switch tile

    [HideInInspector] public int tileNum, randomVariant, numtileVarIndex;
    public int width, height;
    public GameObject wallTile;
    public NormalTile normalTile;
    public GoalTile goalTile;
    public NumberTile numberTile;
    public SwitchTile switchTile;
    public int[] tileType;
    public int[] numTileVar0, numTileVar1, numTileVar2;

    void Start() {

        randomVariant = Random.Range(0,3);
        CreateGrid();
    }

    void CreateGrid() {

        for(int x = 0; x < width; x++) {

             for(int y = 0; y < height; y++) {

                if(tileType[tileNum] == 1) {
                    var spawnedTile = Instantiate(wallTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                else if(tileType[tileNum] == 2) {
                    var spawnedTile = Instantiate(goalTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                else if(tileType[tileNum] == 3) {

                    var spawnedTile = Instantiate(numberTile, new Vector3(x-2, y-3), Quaternion.identity);
                    if(randomVariant == 0) {
                        spawnedTile.number = numTileVar0[numtileVarIndex];
                        numtileVarIndex++;
                    }
                    else if(randomVariant == 1) {
                        spawnedTile.number = numTileVar1[numtileVarIndex];
                        numtileVarIndex++;
                    }
                    else {
                        spawnedTile.number = numTileVar2[numtileVarIndex];
                        numtileVarIndex++;
                    }                    
                }
                else if(tileType[tileNum] == 4) {
                    var spawnedTile = Instantiate(switchTile, new Vector3(x-2, y-3), Quaternion.identity);
                }
                else {
                    var spawnedTile = Instantiate(normalTile, new Vector3(x-2, y-3), Quaternion.identity);
                }

                tileNum++;
             }

        }
    }
}
