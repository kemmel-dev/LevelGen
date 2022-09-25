using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Direction[] connections;
    public bool[] connected;

    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        BOTTOM = 2,
        LEFT = 3
    }

}
