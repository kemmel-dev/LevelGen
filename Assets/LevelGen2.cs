using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cell;
using Random = UnityEngine.Random;

public class LevelGen2 : MonoBehaviour
{

    public Cell[] options;

    public static Cell[,] grid;
    public static Vector2Int gridSize;


    private void Awake()
    {
        GenerateOptions();
    }

    private void GenerateOptions()
    {
        CellType[] cellTypes = new CellType[] { CellType.HUB, CellType.CORNER, CellType.DEADEND, CellType.TJUNC, CellType.CORRIDOR };
        List<Cell> _options = new List<Cell>();
        foreach (CellType type in cellTypes)
        {
            // Iterate differently based on type
            (int end, int step) iterator = type switch
            {
                CellType.HUB => (1, 1),
                CellType.CORRIDOR => (3, 2),
                _ => (4, 1)
            };
            for (int orientation = 0; orientation < iterator.end; orientation += iterator.step)
            {
                _options.Add(new Cell(new Vector2Int(-1, -1), orientation, type));
            }
        }
        options = _options.ToArray();
    }

    private Direction FlipDirection(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    // Get Cells that can fit in the cell that's one adjacent inDirection.
    private Cell[] GetOptions(Direction inDirection, Cell cell)
    {
        List<Direction> alreadyTaken = new List<Direction>();
        alreadyTaken.Add(FlipDirection(inDirection));

        (Cell correspondingCell, Direction inDirection)[] neighbours = cell.GetNeighbours();

        for (int i = 0; i < neighbours.Length; i++)
        {
            alreadyTaken.Add(FlipDirection(neighbours[i].inDirection));
        }
        
        return GetOptions(alreadyTaken.ToArray());
    }

    private Cell[] GetOptions(Direction[] takenDirections)
    {
        List<Cell> filteredOptions = new List<Cell>();

        for (int i = 0; i < options.Length; i++)
        {
            Direction[] availableConnections = options[i].connections;
        }
    }

    public void GenerateLevel()
    {
        InitGrid();
        PopulateGrid();
        InstantiateGrid();
    }
    private void InitGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x,y] = new Cell(new Vector2Int(x,y));
            }
        }
    }

    private void PopulateGrid()
    {
        // Pick random non-border 
        Vector2Int randStart = new Vector2Int(Random.Range(1, gridSize.x - 1), Random.Range(1, gridSize.y - 1));
        grid[randStart.x, randStart.y] = new Cell(randStart, 0, CellType.HUB);
    }

    private void InstantiateGrid()
    {
        throw new NotImplementedException();
    }




}
