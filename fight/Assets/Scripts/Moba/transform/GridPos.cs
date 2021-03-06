﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPos {
    public static int blockNone = 1;
    public static int blockStatic = 2;
    public static int blockDynamic = 3;
    public static int stateNone = 1;
    public static int stateOpen = 2;
    public static int stateClose = 3;

    public FixVector2 posCenter;
    public GridPos posParent;
    public int block;
    public int state;
    public int index;
    public int x;
    public int z;
    public int f;
    public int g;
    public int h;

    public GridPos(int x_, int z_, int index_) {
        block = GridPos.blockNone;
        state = stateNone;
        posParent = null;
        index = index_;
        x = x_;
        z = z_;
        f = 0;
        g = 0;
        h = 0;
    }

    public void Init() {
        if (block != GridPos.blockStatic) {
            block = GridPos.blockNone;
        }
        state = stateNone;
        posParent = null;
        f = 0;
        g = 0;
        h = 0;
    }

    public int GetValue() {
        return f;
    }
}
