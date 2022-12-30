using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButton : MonoBehaviour {
    [HideInInspector] public int listLen, moves;
    [HideInInspector] public int[,] graph;
    [HideInInspector] public List<GameObject> tileList;
    public int startingVertex;
    public Transform parentTile;
    public GameObject player;
    NumberTile numberTile;
	GameManager gameManager;

    void Start() {

		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        //Adding Tiles into List
        foreach (Transform child in parentTile) {
            if(child.tag == "Normal" || child.tag == "Goal" || child.tag == "Number" || child.tag == "Switch")
                tileList.Add(child.gameObject);
        }

        listLen = tileList.Count;
        graph = new int[listLen,listLen];        
    }

    void Update() { 

    }

    public void HintClicked() {

		Debug.Log(gameManager.moves);

        for(int i = 0; i < listLen; i++) {
            
            var posX = tileList[i].transform.position.x;
            var posY = tileList[i].transform.position.y;

            for(int j = 0; j < listLen; j++) {

                var adjX = tileList[j].transform.position.x;
                var adjY = tileList[j].transform.position.y;

                // Left || Right || Down || Up
				if( (adjX == (posX-1) && adjY == (posY)) || (adjX == (posX+1) && adjY == (posY)) ||
                    (adjY == (posY-1) && adjX == (posX)) || (adjY == (posY+1) && adjX == (posX))){

                    if(tileList[j].tag == "Normal"){
                        graph[i,j] = 0;
                    }
                    else if(tileList[j].tag == "Number") {
                        numberTile = tileList[j].GetComponent<NumberTile>();
                        graph[i,j] = numberTile.number;
                    }
                    else {
                        graph[i,j] = 0;
                    }
                }
            }
        }       

        GFG t = new GFG();

		// Function Call
		t.dijkstra(graph, startingVertex);
    }

   class GFG {

		// A utility function to find the vertex with minimum distance value, from the set of vertices
    	// not yet included in shortest path tree
    	static int V = 13;

    	int minDistance(int[] dist, bool[] sptSet) {
        
			// Initialize min value
        	int min = int.MaxValue, min_index = -1;
 
        	for (int v = 0; v < V; v++)
            	if (sptSet[v] == false && dist[v] <= min) {
                	min = dist[v];
                	min_index = v;
            	}
 
        	return min_index;
    	}
 
    	// A utility function to print the constructed distance array
    	void printSolution(int[] dist) {

        	Debug.Log("Vertex \t\t Distance " + "from Source\n");
        	for (int i = 0; i < V; i++)
            	Debug.Log(i + " \t\t " + dist[i] + "\n");
    	}
 
    	// Function that implements Dijkstra's single source shortest path algorithm
    	// for a graph represented using adjacency matrix representation
    	public void dijkstra(int[, ] graph, int src) {

        	// The output array. 
			// dist[i] will hold the shortest distance from src to i
			int[] dist = new int[V];
 
        	// sptSet[i] will true if vertex i is included in shortest path
        	// tree or shortest distance from src to i is finalized
        	bool[] sptSet = new bool[V];
 
        	// Initialize all distances as INFINITE and stpSet[] as false
        	for (int i = 0; i < V; i++) {
            	dist[i] = int.MaxValue;
            	sptSet[i] = false;
        	}
 
        	// Distance of source vertex from itself is always 0
        	dist[src] = 0;
 
        	// Find shortest path for all vertices
        	for (int count = 0; count < V - 1; count++) {
            
				// Pick the minimum distance vertex from the set of vertices not yet
            	// processed. u is always equal to src in first iteration.
            	int u = minDistance(dist, sptSet);
 
            	// Mark the picked vertex as processed
            	sptSet[u] = true;
 
            	// Update dist value of the adjacent vertices of the picked vertex.
            	for (int v = 0; v < V; v++)
 
                // Update dist[v] only if is not in sptSet, there is an edge from u to v, and total weight of path
                // from src to v through u is smaller than current value of dist[v]
                if (!sptSet[v] && graph[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                    dist[v] = dist[u] + graph[u, v];
        	}
 
        	// print the constructed distance array
        	printSolution(dist);
    	}
	}
}