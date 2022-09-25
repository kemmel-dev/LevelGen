using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{

    public GameObject levelStart;
    private Transform level;

    public GameObject[] topConnecting;
    public GameObject[] rightConnecting;
    public GameObject[] bottomConnecting;
    public GameObject[] leftConnecting;

    public GameObject[] deadends;

    private float cellSize = 10f;

    private void Start()
    {
        level = Instantiate(new GameObject("Level")).transform;
        GenerateLevel();
    }

    private void GenerateLevel()
    {

        Queue<Cell> cells = new Queue<Cell>();

        cells.Enqueue(Instantiate(levelStart, Vector3.zero, Quaternion.identity, level).GetComponent<Cell>());
        int existingRooms = 1;
        
        // While queue not empty
        while (cells.Count > 0)
        {
            // Dequeue current cell
            Cell currentCell = cells.Dequeue();

            // For each possible connection...
            foreach (Cell.Direction connection in currentCell.connections)
            {
                // Get a random prefab and non-random displacement based on the connection direction
                Vector3 displacement;
                GameObject prefab;
                switch (connection)
                {
                    case Cell.Direction.TOP:
                        displacement = new Vector3(0, 0, cellSize);
                        prefab = bottomConnecting[UnityEngine.Random.Range(0, bottomConnecting.Length)];
                        break;
                    case Cell.Direction.RIGHT:
                        displacement = new Vector3(cellSize, 0, 0);
                        prefab = leftConnecting[UnityEngine.Random.Range(0, leftConnecting.Length)];
                        break;
                    case Cell.Direction.LEFT:
                        displacement = new Vector3(-cellSize, 0, 0);
                        prefab = rightConnecting[UnityEngine.Random.Range(0, rightConnecting.Length)];
                        break;
                    case Cell.Direction.BOTTOM:
                        displacement = new Vector3(0, 0, -cellSize);
                        prefab = topConnecting[UnityEngine.Random.Range(0, topConnecting.Length)];
                        break;
                    default:
                        throw new ArgumentException("???");
                };

                // Check if the new room location isn't already taken
                Vector3 newPos = currentCell.transform.position + displacement;
                if (Physics.OverlapSphere(newPos, 3f, LayerMask.GetMask("Room")).Length == 0)
                {
                    // Check whether we need to start ending the paths
                    if (existingRooms > 50)
                    {
                        // If so, replace random prefabs with dead ends.
                        prefab = connection switch
                        {
                            Cell.Direction.TOP => deadends[2],
                            Cell.Direction.RIGHT => deadends[3],
                            Cell.Direction.BOTTOM => deadends[0],
                            Cell.Direction.LEFT => deadends[1],
                        };
                        // And just instantiate without queuing
                        Instantiate(prefab, currentCell.transform.position + displacement, prefab.transform.localRotation, level);
                    }
                    else
                    {
                        // Instantiate random prefab and enqueue the new cell for handling.
                        cells.Enqueue(Instantiate(prefab, currentCell.transform.position + displacement, prefab.transform.localRotation, level).GetComponent<Cell>());
                    }
                    existingRooms++;
                }
            }
        }



    }

    private void AddCell(Cell currentCell, Cell.Direction inDirection)
    {
        
    }
}

