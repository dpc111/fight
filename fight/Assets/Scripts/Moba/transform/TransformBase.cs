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

    public UnitBase mUnit = null;
    public TransformBlock mBlock = new TransformBlock();
    public TransformMoveTarget mMoveTarget = new TransformMoveTarget();
    public TransformMoveDir mMoveDir = new TransformMoveDir();
    public TransformMoveLock mMoveLock = new TransformMoveLock();
    public TransformMoveDirTo mMoveDirTo = new TransformMoveDirTo();
    public TransformMoveBase mMoveCur = null;

    public Fix Speed { get { return mSpeed; } set { mSpeed = value; } }
    public FixVector3 Pos { get { mGPos.x = mPos.x; mGPos.z = mPos.y; return mGPos; } }
    public FixVector3 Dir { get { mGDir.x = mDir.x; mGDir.z = mDir.y; return mGDir; } }
    public Matrix3 RolMat { get { return Matrix3.RollVecToVecAxisY(GameConst.InitForward, Dir); } }
    public bool Move { 
        get { 
            return mIsMove; 
        } 
        set {
            if (mIsMove && !value) {
                mIsMove = value;
                mUnit.OnMoveStop();
            } else if (!mIsMove && value) {
                mIsMove = value;
                mUnit.OnMoveStart();
            } else {
                mIsMove = value;
            }
        } 
    }
    public void SetMoveForce(bool isMove) { mIsMove = isMove; }


    public virtual void Init(UnitBase unit, FixVector3 pos, FixVector3 dir, Fix blockRange, Fix speed) {
        mPos = new FixVector2(pos.x, pos.z);
        mDir = new FixVector2(dir.x, dir.z);
        mGPos = pos;
        mGDir = dir;
        mSpeed = speed;
        Move = false;
        mUnit = unit;
        mBlock.Init(this, blockRange);
        mMoveTarget.Init(this);
        mMoveDir.Init(this);
        mMoveLock.Init(this);
        mMoveDirTo.Init(this);
        mMoveCur = null;
    }

    public void Update() {
        //mBlock.Update();
        if (!Move || mMoveCur == null) {
            return;
        }
        mMoveCur.Update();
    }

    public void MoveTarget(FixVector3 target) {
        MoveTarget(target, Fix.fix0);
    }

    public void MoveTarget(FixVector3 target, Fix overLen) {
        Move = true;
        mMoveCur = mMoveTarget;
        mMoveTarget.Move(new FixVector2(target.x, target.z), overLen);
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

    public void MoveDirTo(FixVector3 pos) {
        Move = true;
        mMoveCur = mMoveDirTo;
        mMoveDirTo.Move(new FixVector2(pos.x, pos.z));
    }
}
