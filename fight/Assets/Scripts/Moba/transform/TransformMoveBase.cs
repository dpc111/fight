using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveBase {
    public TransformBase mTransform = null;

    public virtual void Init(TransformBase tran) {
        mTransform = tran;
    }

    public virtual void Update() {

    }
}