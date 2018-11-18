using System.Collections;
using System.Collections.Generic;

public class DirectionShootBullet : BaseBullet 
{
    Fix mFixMoveTime = Fix.fixZero;
    Fix mFixSpeed = Fix.fixZero;

    public override void InitData(LiveObject src, LiveObject dest, FixVector3 srcPos, FixVector3 destPos)
    {
        base.InitData(src, dest, srcPos, destPos);
        mFixMoveTime = FixVector3.Distance(srcPos, destPos) / mFixSpeed;
    }

    public override void CreateBody(string name)
    {
        CreateFromPrefab("prefabs/bullet", this);
    }

    public override void LoadProperties()
    {
        mFixSpeed = (Fix)10;
    }

    public override void Shoot()
    {
        mFv3LogicPos = mFv3SrcPos;
        MoveTo(mFv3SrcPos, mFv3DestPos, mFixMoveTime, delegate() { DoShootDest(); });
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
