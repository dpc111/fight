using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPos
{
    public static int stateNone = 1;
    public static int stateOpen = 2;
    public static int stateClose = 3;
    public int state = stateNone;
    public GridPos posParent;
    public GridPos posNext;
    public int x;
    public int y;
    public int f;
    public int g;
    public int h;
}
