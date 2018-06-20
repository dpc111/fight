namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GridMgr
    {
        public const int ROWNUM = 10;
        public const int COLNUM = 5;
        public static int[,] grids = { 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                         { 0, 0, 0, 0, 0 }, 
                                     };

        public static void Reset() 
        {
            for (int i = 0; i < ROWNUM; i++) 
            {
                for (int j = 0; j < COLNUM; j++)
                {
                    grids[i, j] = 0;
                }
            }
        }

        public static int GetEntity(int row, int col)
        {
            if (row < 1 
                || row > ROWNUM
                || col < 1
                || col > COLNUM)
            {
                return 0;
            }
            int id = grids[row - 1, col - 1];
            return id;
        }

        public static bool ExistEntity(int row, int col) {
            int id = GetEntity(row, col);
            if (id == 0)
            {
                return false;
            }
            return true;
        }

        public static void CreateEntity(int row, int col, int entityId) {
            if (ExistEntity(row, col))
            {
                Debug.Log("");
                return;
            }
            grids[row - 1, col - 1] = entityId;
        }

        public static void DestroyEntity(int entityId)
        {
            for (int i = 0; i < ROWNUM; i++)
            {
                for (int j = 0; j < COLNUM; j++)
                {
                    int id = grids[i, j];
                    if (id == entityId)
                    {
                        grids[i, j] = 0;
                        return;
                    }
                }
            }
            
        }
    }
}
