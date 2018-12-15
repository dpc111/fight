using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBase {
    public FixVector2 mPos = new FixVector2();
    public FixVector2 mDir = new FixVector2();
    public FixVector3 mGPos = new FixVector3();
    public FixVector3 mGDir = new FixVector3();
    public Fix mSpeed = Fix.fix0;
    public bool mIsMove = false;

    public TransformBlock mBlock = new TransformBlock();
    public TransformMoveTarget mMoveTarget = new TransformMoveTarget();
    public TransformMoveDir mMoveDir = new TransformMoveDir();
    public TransformMoveLock mMoveLock = new TransformMoveLock();
    public TransformMoveBase mMoveCur = null;

    public bool Move { get { return mIsMove; } set { mIsMove = value; } }
    public Fix Speed { get { return mSpeed; } set { mSpeed = value; } }
    public FixVector3 Pos { get { mGPos.x = mPos.x; mGPos.z = mPos.y; return mGPos; } }
    public FixVector3 Dir { get { mGDir.x = mDir.x; mGDir.z = mDir.y; return mGDir; } }

    public virtual void Init(FixVector3 pos, Fix blockRange, Fix speed) {
        mPos = new FixVector2(pos.x, pos.z);
        mGPos = pos;
        mSpeed = speed;
        Move = false;
        mBlock.Init(this, blockRange);
        mMoveTarget.Init(this);
        mMoveDir.Init(this);
        mMoveLock.Init(this);
        mMoveCur = null;
    }

    public void Update() {
        mBlock.Update();
        if (!Move || mMoveCur == null) {
            return;
        }
        mMoveCur.Update();
    }

    public void MoveTarget(FixVector3 target) {
        Move = true;
        mMoveCur = mMoveTarget;
        mMoveTarget.Move(new FixVector2(target.x, target.z));
    }

    public void MoveDir(FixVector3 dir) {
        Move = true;
        mMoveCur = mMoveDir;
        mMoveDir.Move(new FixVector2(dir.x, dir.z));
    }

    public void MoveLock(TransformBase objLock) {
        Move = true;
        mMoveCur = mMoveLock;
        mMoveLock.Move(objLock);
    }
}
