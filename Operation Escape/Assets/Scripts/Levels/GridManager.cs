using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {

    [HideInInspector] public int tileNum, randomVariant, numtileVarIndex, sceneIndex, playerToSwitchDist;
    [HideInInspector] public bool isSwitch;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector2Int start, end, playerPos, goalPos, switchPos;
    [HideInInspector] public Queue<Vector2Int> path = new Queue<Vector2Int>();
    [HideInInspector] public List<int> numbers, playerToNumDist, numToGoalDist, numToSwitchDist;
    [HideInInspector] public List<List<int>> gridMap = new List<List<int>>();
    [HideInInspector] public List<Dictionary<int, int>> distanceDict = new List<Dictionary<int, int>>();
    public GameObject grid;
    public int width, height;
    public WallTile wallTile;
    public NormalTile normalTile;
    public GoalTile goalTile;
    public NumberTile numberTile;
    public SwitchTile switchTile;
    public List<int> tileType;
    public List<int> numTileVar0, numTileVar1, numTileVar2;

    void Start() {

        player = GameObject.Find("Player");
        playerPos = new Vector2Int((int)player.transform.position.x+2, (int)Mathf.Ceil(player.transform.position.y+2));

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        if (sceneIndex != 2) {
            randomVariant = Random.Range(0,3);
        } 
        else {
            randomVariant = 0;
        }

        CreateGrid();
        if(sceneIndex >= 7) {
            NumberTile[] numTiles = GameObject.FindObjectsOfType<NumberTile>();
            foreach(var n in numTiles) 
                numbers.Add(n.number);
            
            // Distances of each number tile to every number tile
            for(int i = 0; i < numTiles.Length; i++) {
                var subDict = new Dictionary<int,int>();
                for(int j = 0; j < numbers.Count; j++) {
                    var start = new Vector2Int((int)numTiles[i].transform.position.x+2, (int)numTiles[i].transform.position.y+3);
                    var end = new Vector2Int((int)numTiles[j].transform.position.x+2, (int)numTiles[j].transform.position.y+3);
                    if(start == end)
                        subDict.Add(numbers[j], 2);
                    else {
                        var path = ShortestPath(start, end);
                        var pathCount = path.Count;
                        var numInPath = 0;
                        foreach(var n in path) 
                            if(gridMap[n.x][n.y] == 2) 
                                numInPath++;
                        subDict.Add(numbers[j], pathCount + numInPath-1);
                    }
                }
                distanceDict.Add(subDict);
            }

            // Distances of each number tile to player
            for(int i = 0; i < numbers.Count; i++) {
                var start = playerPos;
                var end = new Vector2Int((int)numTiles[i].transform.position.x+2, (int)numTiles[i].transform.position.y+3);
                var path = ShortestPath(start, end);
                var pathCount = path.Count;
                var numInPath = 0;
                foreach(var n in path)
                    if(gridMap[n.x][n.y] == 2)
                        numInPath++;
                playerToNumDist.Add(pathCount + numInPath-1);
            }

            // Distances of each number tile to goal
            for(int i = 0; i < numbers.Count; i++) {
                var start = goalPos;
                var end = new Vector2Int((int)numTiles[i].transform.position.x+2, (int)numTiles[i].transform.position.y+3);
                var path = ShortestPath(start, end);
                var pathCount = path.Count;
                var numInPath = 0;
                foreach(var n in path)
                    if(gridMap[n.x][n.y] == 2)
                        numInPath++;
                numToGoalDist.Add(pathCount + numInPath-1);
            }

            // Distances of each number tile to switch
            if(isSwitch) {
                for(int i = 0; i < numbers.Count; i++) {
                    var start = switchPos;
                    var end = new Vector2Int((int)numTiles[i].transform.position.x+2, (int)numTiles[i].transform.position.y+3);
                    var path = ShortestPath(start, end);
                    numToSwitchDist.Add(path.Count);
                }
            }

            // Distance of player to switch tile
            if(isSwitch) {
                var start = playerPos;
                var end = switchPos;
                var path = ShortestPath(start, end);
                playerToSwitchDist = path.Count;
            }    
        }
    }

    void CreateGrid() {

        for (int x = 0; x < width; x++) {
            var subList = new List<int>();
             for (int y = 0; y < height; y++) {

                // Spawn Normal Tile
                if (tileType[tileNum] == 1) {
                    var spawnedTile = Instantiate(normalTile, new Vector3(x-2, y-3), Quaternion.identity);
                    spawnedTile.transform.parent = grid.transform;
                    subList.Add(1);
                }
                // Spawn Goal Tile
                else if (tileType[tileNum] == 2) {
                    var spawnedTile = Instantiate(goalTile, new Vector3(x-2, y-3), Quaternion.identity);
                    spawnedTile.transform.parent = grid.transform;
                    goalPos = new Vector2Int((int)spawnedTile.transform.position.x+2, (int)spawnedTile.transform.position.y+3);
                    subList.Add(1);
                }
                // Spawn Number Tile
                else if (tileType[tileNum] == 3) {

                    var spawnedTile = Instantiate(numberTile, new Vector3(x-2, y-3), Quaternion.identity);
                    spawnedTile.transform.parent = grid.transform;

                    if (randomVariant == 0) {
                        spawnedTile.number = numTileVar0[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar0[numtileVarIndex];
                        //numbers.Add(spawnedTile.number);
                        subList.Add(2);
                    }
                    else if (randomVariant == 1) {
                        spawnedTile.number = numTileVar1[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar1[numtileVarIndex];
                        //numbers.Add(spawnedTile.number);
                        subList.Add(2);
                    } 
                    else {
                        spawnedTile.number = numTileVar2[numtileVarIndex];
                        spawnedTile.name = "Number Tile " +numTileVar2[numtileVarIndex];
                        //numbers.Add(spawnedTile.number);
                        subList.Add(2);
                    }
                    
                    numtileVarIndex++;                
                }
                // Spawn Switch Tile
                else if (tileType[tileNum] == 4) {
                    var spawnedTile = Instantiate(switchTile, new Vector3(x-2, y-3), Quaternion.identity);
                    spawnedTile.transform.parent = grid.transform;
                    switchPos = new Vector2Int((int)spawnedTile.transform.position.x+2, (int)spawnedTile.transform.position.y+3);
                    isSwitch = true;
                    subList.Add(2);
                }
                // Spawn Wall Tile
                else {
                    var spawnedTile = Instantiate(wallTile, new Vector3(x-2, y-3), Quaternion.identity);
                    spawnedTile.transform.parent = grid.transform;
                    subList.Add(0);
                }

                tileNum++;
            }
            gridMap.Add(subList);
        }
    }

    Queue<Vector2Int> ShortestPath(Vector2Int start, Vector2Int end) {

        Dictionary<Vector2Int, Vector2Int> nextTileToGoal = new Dictionary<Vector2Int, Vector2Int>();
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        List<Vector2Int> visited = new List<Vector2Int>();

        frontier.Enqueue(end);

        while(frontier.Count > 0) {

            Vector2Int curTile = frontier.Dequeue();
            var neighbors = NeighborTile(curTile);

            foreach(Vector2Int neighbor in neighbors) {
                if(!visited.Contains(neighbor) && !frontier.Contains(neighbor)) {
                    if(gridMap[(int)neighbor.x][(int)neighbor.y] > 0) {
                        frontier.Enqueue(neighbor);
                        nextTileToGoal[neighbor] = curTile;
                    }
                }
            }
            
            visited.Add(curTile);
        }

        if(!visited.Contains(start))
            return null;

        Queue<Vector2Int> path = new Queue<Vector2Int>();
        Vector2Int curPathTile = start;
        while(curPathTile != end) {
            curPathTile = nextTileToGoal[curPathTile];
            path.Enqueue(curPathTile);
        }
        
        return path;
    }

    Vector2Int[] NeighborTile(Vector2Int curTile) {

        List<Vector2Int> neighbors = new List<Vector2Int>();
        var posX = curTile.x;
        var posY = curTile.y;

        // Up
        if(posY+1 <= 4) 
            if(gridMap[posX][posY+1] > 0) 
                neighbors.Add(new Vector2Int(posX,posY+1));
        // Right
        if(posX+1 <= 4) 
            if(gridMap[posX+1][posY] > 0) 
                neighbors.Add(new Vector2Int(posX+1,posY));
        // Down
        if(posY-1 >= 0) 
            if(gridMap[posX][posY-1] > 0) 
                neighbors.Add(new Vector2Int(posX,posY-1));
        // Left
        if(posX-1 >= 0) 
            if(gridMap[posX-1][posY] > 0) 
                neighbors.Add(new Vector2Int(posX-1,posY));
        
        return neighbors.ToArray();
    }
}
