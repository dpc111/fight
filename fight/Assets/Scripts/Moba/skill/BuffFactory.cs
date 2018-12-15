using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory {
    public static BuffBase CreateBuff(int type) {
        BuffBase buff = new BuffBase();
        return buff;
    }

    public static void AddBuff(UnitBase tri, UnitBase tar, int type) {
        BuffBase buff = CreateBuff(type);
        buff.mUnitTri = tri;
        buff.mUnitTar = tar;
        tar.mBuffMgr.Add(buff);
        tri.mBuffMgr.AddTri(buff);
        buff.Init(type);
        buff.Start();
    }

    public static void RemoveBuff(BuffBase buff) {
        buff.End();
        buff.mUnitTar.mBuffMgr.Remove(buff);
        buff.mUnitTri.mBuffMgr.RemoveTri(buff);
    }
}
