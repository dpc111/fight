namespace Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class Config
    {
        public const float gravity = 0.98f;
        public const float bulletXMin = 0f;
        public const float bulletXMax = 100f;
        public const float bulletYMin = 0f;
        public const float bulletYMax = 20f;
        public const float bulletZMin = 0f;
        public const float bulletZMax = 50f;
        public const float bulletLifeTime = 20f;
    }
}