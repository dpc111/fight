using System.Collections;
using System.Collections.Generic;

public class MagicStand : BaseTower 
{
    public MagicStand()
    {
        Init();
    }

    public override void LoadProperties()
    {
        mFixDamage = (Fix)50;
        mFixAttackRange = (Fix)6;
        mFixAttackSpeed = (Fix)1;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    void Init()
    {
        LoadProperties();
        CreateFromPrefab("prefabs/tower", this);
        mObjType = GameConst.ObjTypeMagicStand;
    }
}
