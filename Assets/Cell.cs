using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public CellType type;
    public Vector2Int index;

    public Direction[] connections;
    public Cell[] connectedCells;

    private int orientation;

    public Cell(Vector2Int _index, int _orientation, CellType _type = CellType.EMPTY)
    {
        index = _index;
        type = _type;
        orientation = _orientation;
        SetConnections();
        connectedCells = new Cell[4];
    }

    private void SetConnections()
    {
        connections = type switch
        {
            CellType.CORNER => new Direction[] { Direction.TOP, Direction.RIGHT },
            CellType.CORRIDOR => new Direction[] { Direction.TOP, Direction.BOTTOM },
            CellType.TJUNC => new Direction[] { Direction.TOP, Direction.RIGHT, Direction.BOTTOM },
            CellType.DEADEND => new Direction[] { Direction.TOP },
            _ => new Direction[] { }
        };
        ShiftConnections(orientation);
    }

    private void ShiftConnections(int orientation)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            connections[i] = (Direction)(((int)connections[i] + orientation) % 4);
        }
    }

    public (Cell, Direction)[] GetNeighbours()
    {
        List<(Cell, Direction)> neighbours = new List<(Cell, Direction)>();

        int[] offset_x = new int[] { 0, 1, 0, -1 };
        int[] offset_y = new int[] { -1, 0, 1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int x = index.x + offset_x[i];
            int y = index.y + offset_y[i];

            if (x < 0 || x >= LevelGen2.gridSize.x || y < 0 || y >= LevelGen2.gridSize.y)
            {
                continue;
            }

            Cell neighbour = LevelGen2.grid[x, y];

            if (neighbour.type != CellType.EMPTY)
            {
                neighbours.Add((neighbour, (Direction) i));
            }
        }

        return neighbours.ToArray();
    }

    // Directions numbered in a clockwise fashion
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        BOTTOM = 2,
        LEFT = 3
    }

    public enum CellType
    {
        EMPTY,
        HUB,
        CORNER,
        DEADEND,
        TJUNC,
        CORRIDOR
    }

}

