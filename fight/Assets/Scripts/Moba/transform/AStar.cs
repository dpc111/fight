using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {
    public static int findPathLen = 500;
    public static int objectBlockLen = 50;

    public int mX;
    public int mZ;
    public int mTotal;
    public Fix mXLen;
    public Fix mZLen;
    public Fix mWidth;
    public int mRunTime;
    public MinHeap mOpenGrids;
    public GridPos[] mGrids;
    public GridPos[] mPath;

    public void Init(Fix xLen, Fix zLen, Fix width) {
        mX = (int)(xLen / width);
        mZ = (int)(zLen / width);
        mTotal = mX * mZ;
        mXLen = xLen;
        mZLen = zLen;
        mWidth = width;
        mRunTime = 0;
        mOpenGrids = new MinHeap(mX * mZ + 1);
        mGrids = new GridPos[mTotal];
        mPath = new GridPos[mTotal];
        for (int kz = 0; kz < mZ; kz++) {
            for (int kx = 0; kx < mX; kx++) {
                int index = kz * mX + kx;
                mGrids[index] = new GridPos(kx, kz, index);
                mGrids[index].posCenter = new FixVector2((Fix)(kx * mWidth) + width / (Fix)2, (Fix)(kz * mWidth) + width / (Fix)2);
            }
        }
    }

    public int ToGridIndex(FixVector2 v) {
        int x = (int)(v.x / mWidth);
        int y = (int)(v.y / mWidth);
        if (x < 0) {
            x = 0;
        }
        if (y < 0) {
            y = 0;
        }
        if (x > mX) {
            x = mX;
        }
        if (y > mZ) {
            y = mZ;
        }
        int index = y * mX + x;
        return index;
    }

    public FixVector2 ToCenterPos(int index) {
        GridPos gridPos = GetGridPos(index);
        if (gridPos == null) {
            return new FixVector2(Fix.fix0, Fix.fix0);
        }
        return gridPos.posCenter;
    }

    public GridPos GetGridPos(int index) {
        if (index < 0 || index >= mTotal) {
            return null;
        }
        GridPos pos = mGrids[index];
        return pos;
    }

    public GridPos GetGridPos(int x, int z) {
        if (x < 0 || x >= mX || z < 0 || z >= mZ) {
            return null;
        }
        int index = z * mX + x;
        return GetGridPos(index);
    }

    public GridPos GetGridPos(FixVector2 pos) {
        return GetGridPos(ToGridIndex(pos));
    }

    public void GetAround(GridPos pos, ref GridPos[] arounds) {
        if (pos == null) {
            return;
        }
        arounds[0] = GetGridPos(pos.x - 1, pos.z - 1);
        arounds[1] = GetGridPos(pos.x - 1, pos.z);
        arounds[2] = GetGridPos(pos.x - 1, pos.z + 1);
        arounds[3] = GetGridPos(pos.x, pos.z - 1);
        arounds[4] = GetGridPos(pos.x, pos.z + 1);
        arounds[5] = GetGridPos(pos.x + 1, pos.z - 1);
        arounds[6] = GetGridPos(pos.x + 1, pos.z);
        arounds[7] = GetGridPos(pos.x + 1, pos.z + 1);
    }

    public GridPos OptAround(GridPos around, GridPos cur, GridPos end, int desLen = 0) {
        int step = 0;
        if (Mathf.Abs(around.x - cur.x) == Mathf.Abs(around.z - cur.z)) {
            step = 14;
        } else {
            step = 10;
        }
        int devX = Mathf.Abs(around.x - end.x);
        int devZ = Mathf.Abs(around.z - end.z);
        int devMax = Mathf.Max(devX, devZ);
        int devMin = Mathf.Min(devX, devZ);
        int h = devMin * 14 + (devMax - devMin) * 10;
        int g = cur.g + step;
        int f = g + h;
        if (around.f == 0 || around.f > f) {
            //重新排序???
            around.g = g;
            around.h = h;
            around.f = f;
            around.posParent = cur;
            if (h <= desLen) {
                return around;
            }
            if (around.state != GridPos.stateOpen) {
                around.state = GridPos.stateOpen;
                mOpenGrids.Push(around);
            }
        }
        return null;
    }

    public void CheckPathDir(GridPos posEnd, ref int[] path, ref int pathLen) {
        if (posEnd == null) {
            return;
        }

        int index = 0;
        GridPos posPath = posEnd;
        while (posPath != null) {
            mPath[index] = posPath;
            posPath = posPath.posParent;
            index++;
        }

        int devX = 0;
        int devZ = 0;
        int devX1 = 0;
        int devZ1 = 0;
        int checkLen = 0;
        GridPos posCur = mPath[--index];
        GridPos posNext = null;
        path[checkLen] = posCur.index;
        while (index > 0) {
            posNext = mPath[--index];
            devX1 = posNext.x - posCur.x;
            devZ1 = posNext.z - posCur.z;
            if (devX == 0 && devZ == 0) {
                devX = devX1;
                devZ = devZ1;
            }
            if (devX != devX1 || devZ != devZ1) {
                checkLen++;
            }
            devX = devX1;
            devZ = devZ1;
            posCur = posNext;
            path[checkLen] = posCur.index;
        }
        pathLen = checkLen + 1;
    }

    public bool FindPath(int start, int end, ref int[] path, ref int pathLen) {
        return FindPath(start, end, ref path, ref pathLen, Fix.fix0);
    }

    public bool FindPath(int start, int end, ref int[] path, ref int pathLen, Fix overLen) {
        mRunTime++;
        if (start < 0 || start >= mTotal || end < 0 || end >= mTotal) {
            return false;
        }
        GridPos posStart = GetGridPos(start);
        if (posStart == null) {
            return false;
        }
        GridPos posEnd = GetGridPos(end);
        if (posEnd == null) {
            return false;
        }
        mOpenGrids.Clear();

        posStart.g = 0;
        posStart.h = 0;
        posStart.f = posStart.g + posStart.h;
        posStart.state = GridPos.stateOpen;
        mOpenGrids.Push(posStart);

        GridPos[] arounds = new GridPos[8];
        GridPos posFindEnd = null;
        int tarLen = ((int)overLen) * 10;

        while (true) {
            GridPos posCur = (GridPos)mOpenGrids.Pop();
            if (posCur == null) {
                break;
            }
            if (posCur.state != GridPos.stateOpen) {
                break;
            }
            GetAround(posCur, ref arounds);
            for (int i = 0; i < 8; i++) {
                GridPos posA = arounds[i];
                if (posA == null) {
                    continue;
                }
                if (posA.index == posEnd.index) {
                    posA.posParent = posCur;
                    posA.state = GridPos.stateClose;
                    posFindEnd = posA;
                    break;
                }
                if (posA.state == GridPos.stateClose) {
                    continue;
                }
                if (posA.block != GridPos.blockNone) {
                    continue;
                }
                posFindEnd = OptAround(posA, posCur, posEnd, tarLen);
                if (posFindEnd != null) {
                    break;
                }
            }
            posCur.state = GridPos.stateClose;
            if (posFindEnd != null) {
                break;
            }
        }

        CheckPathDir(posFindEnd, ref path, ref pathLen);

        if (posFindEnd == null) {
            return false;
        }
        return true;
    }

    public void SetBlockDynamic(ref int[] blocks, int len) {
        for (int i = 0; i < len; i++) {
            GridPos pos = GetGridPos(blocks[i]);
            if (pos == null) {
                continue;
            }
            if (pos.block == GridPos.blockNone) {
                pos.block = GridPos.blockDynamic;
            }
        }
    }

    public void ResetCach() {
        for (int i = 0; i < mTotal; i++) {
            GridPos pos = mGrids[i];
            pos.Init();
        }
    }

    public void TestShowBlock() {
        string blocks = "";
        for (int i = 0; i < mTotal; i++) {
            GridPos pos = mGrids[i];
            if (pos.block != GridPos.blockNone) {
                blocks = blocks + " " + i;
            }
        }
        Debug.Log(blocks);
    }
}
