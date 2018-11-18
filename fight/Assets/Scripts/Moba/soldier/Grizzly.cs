using System.Collections;
using System.Collections.Generic;

public class Grizzly : BaseSoldier
{
    public Grizzly()
    {
        LoadProperties();
        CreateFromPrefab("prefabs/soldier", this);
        mObjType = GameConst.ObjTypeMagicGrizzly;
    }

    public override void LoadProperties()
    {
        SetHp((Fix)200);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
