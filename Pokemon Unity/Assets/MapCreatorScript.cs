using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreatorScript : MonoBehaviour 
{
	public PlayerMovement Player;
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject tile;
	public int width = 1;
	public int height = 1;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
		for(int i = 0; i < width; i++) {
			for(int o = 0; o > (System.Math.Abs(height) * (-1)); o--) {
				Instantiate(tile, new Vector3(i, 0, o), Quaternion.Euler(90, 0, 0)); // Instantiate at position (0, 0, 0) and zero rotation.
			}
		}
    }
}