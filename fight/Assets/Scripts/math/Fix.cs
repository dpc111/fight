#define _CLIENT_LOGIC_

using System;
using System.IO;

public partial struct Fix : IEquatable<Fix>, IComparable<Fix>
{
    const long pi = 12868;
    const long pi2 = 25736;
    const int fractionalPlace = 12;
    const long one = 1L << fractionalPlace;
    public static readonly decimal mPrecision = (decimal)(new Fix(1L));
    public static readonly Fix fixOne = new Fix(one);
    public static readonly Fix fixZero = new Fix();
    public static readonly Fix fixPi = new Fix(pi);
    public static readonly Fix fixPi2 = new Fix(pi2);
    public static readonly Fix fixPi180 = new Fix((long)72);
    public static readonly Fix fixRad2Deg = pi * (Fix)2 / (Fix)360;
    public static readonly Fix fixDeg2Rad = (Fix)360 / (pi * (Fix)2);
    public readonly long rawValue;
    
    Fix(long value)
    {
        rawValue = value;
    }

    public long RawValue { get { return rawValue; } }

    public Fix(int value)
    {
        rawValue = value * one;
    }

    public static explicit operator Fix(long value)
    {
        return new Fix(value * one);
    }

    public static explicit operator long(Fix value)
    {
        return value.rawValue >> fractionalPlace;
    }

    public static explicit operator Fix(float value)
    {
        return new Fix((long)(value * one));
    }

    public static explicit operator float(Fix value)
    {
        return (float)value.rawValue / one;
    }

    public static explicit operator Fix(double value)
    {
        return new Fix((long)(value * one));
    }

    public static explicit operator double(Fix value)
    {
        return (double)value.rawValue / one;
    }

    public static explicit operator Fix(decimal value)
    {
        return new Fix((long)(value * one));
    }

    public static explicit operator decimal(Fix value)
    {
        return (decimal)value / one;
    }

    public static Fix operator +(Fix x, Fix y)
    {
        return new Fix(x.rawValue + y.rawValue);
    }

    public static Fix operator +(Fix x, int y)
    {
        return x + (Fix)y;
    }

    public static Fix operator +(int x, Fix y)
    {
        return (Fix)x + y;
    }

    public static Fix operator +(Fix x, float y)
    {
        return x + (Fix)y;
    }

    public static Fix operator +(float x, Fix y)
    {
        return (Fix)x + y;
    }

    public static Fix operator +(Fix x, double y)
    {
        return x + (Fix)y;
    }

    public static Fix operator +(double x, Fix y)
    {
        return (Fix)x + y;
    }

    public static Fix operator -(Fix x, Fix y)
    {
        return new Fix(x.rawValue - y.rawValue);
    }

    public static Fix operator -(Fix x, int y)
    {
        return x - (Fix)y;
    }

    public static Fix operator -(int x, Fix y)
    {
        return (Fix)x - y;
    }

    public static Fix operator -(Fix x, float y)
    {
        return x - (Fix)y;
    }

    public static Fix operator -(float x, Fix y)
    {
        return (Fix)x - y;
    }

    public static Fix operator -(Fix x, double y)
    {
        return x - (Fix)y;
    }

    public static Fix operator -(double x, Fix y)
    {
        return (Fix)x - y;
    }

    public static Fix operator *(Fix x, Fix y)
    {
        return new Fix((x.rawValue * y.rawValue) >> fractionalPlace);
    }

    public static Fix operator *(Fix x, int y)
    {
        return x * (Fix)y;
    }

    public static Fix operator *(int x, Fix y)
    {
        return (Fix)x * y;
    }

    public static Fix operator *(Fix x, float y)
    {
        return x * (Fix)y;
    }

    public static Fix operator *(float x, Fix y)
    {
        return (Fix)x * y;
    }

    public static Fix operator *(Fix x, double y)
    {
        return x * (Fix)y;
    }

    public static Fix operator *(double x, Fix y)
    {
        return (Fix)x * y;
    }

    public static Fix operator /(Fix x, Fix y)
    {
        return new Fix((x.rawValue << fractionalPlace) / y.rawValue);
    }

    public static Fix operator /(Fix x, int y)
    {
        return x / (Fix)y;
    }

    public static Fix operator /(int x, Fix y)
    {
        return (Fix)x / y;
    }

    public static Fix operator /(Fix x, float y)
    {
        return x / (Fix)y;
    }

    public static Fix operator /(float x, Fix y)
    {
        return (Fix)x / y;
    }

    public static Fix operator /(Fix x, double y)
    {
        return x / (Fix)y;
    }

    public static Fix operator /(double x, Fix y)
    {
        return (Fix)x / y;
    }

    public static Fix operator %(Fix x, Fix y)
    {
        return new Fix(x.rawValue % y.rawValue);
    }

    public static Fix operator -(Fix x)
    {
        return new Fix(-x.rawValue);
    }

    public static bool operator ==(Fix x, Fix y)
    {
        return x.rawValue == y.rawValue;
    }

    public static bool operator !=(Fix x, Fix y)
    {
        return x.rawValue != y.rawValue;
    }

    public static bool operator >(Fix x, Fix y)
    {
        return x.rawValue > y.rawValue;
    }

    public static bool operator >(Fix x, int y)
    {
        return x.rawValue > y;
    }

    public static bool operator <(Fix x, Fix y)
    {
        return x.rawValue < y.rawValue;
    }
    public static bool operator <(Fix x, int y)
    {
        return x.rawValue < y;
    }

    public static bool operator >=(Fix x, Fix y)
    {
        return x.rawValue >= y.rawValue;
    }

    public static bool operator <=(Fix x, Fix y)
    {
        return x.rawValue <= y.rawValue;
    }

    public static Fix operator >>(Fix x, int num)
    {
        return new Fix(x.rawValue >> num);
    }

    public static Fix operator <<(Fix x, int num)
    {
        return new Fix(x.rawValue << num);
    }

    public override bool Equals(object obj)
    {
        return obj is Fix && ((Fix)obj).rawValue == rawValue;
    }

    public override int GetHashCode()
    {
        return rawValue.GetHashCode();
    }

    public bool Equals(Fix oth)
    {
        return rawValue == oth.rawValue;
    }

    public int CompareTo(Fix oth)
    {
        return rawValue.CompareTo(oth.rawValue);
    }

    public override string ToString()
    {
        return ((decimal)this).ToString();
    }

    public string ToStringRound(int round = 2)
    {
        return (float)Math.Round((float)this, round) + "";
    }

    public static Fix FromRaw(long rawValue)
    {
        return new Fix(rawValue);
    }

    public static int Sign(Fix value)
    {
        return value.rawValue < 0 ? -1 : value.rawValue > 0 ? 1 : 0;
    }

    public static Fix Abs(Fix value)
    {
        return new Fix(value.rawValue > 0 ? value.rawValue : -value.rawValue);
    }

    public static Fix Floor(Fix value)
    {
        return new Fix((long)((ulong)value.rawValue & 0xFFFFFFFFFFFFF000));
    }

    public static Fix Ceil(Fix value)
    {
        return ((value.rawValue & 0x0000000000000FFF) != 0) ? Floor(value) + fixOne : value;
    }

    public static Fix Pow(Fix x, int y)
    {
        if (y == 1)
            return x;
        Fix res = Fix.fixZero;
        Fix tmp = Pow(x, y / 2);
        if ((y & 1) != 0)
            res = x * tmp * tmp;
        else
            res = tmp * tmp;
        return res;
    }

    public static Fix Sqrt(Fix x, int num)
    {
        if (x.rawValue < 0)
            throw new ArithmeticException("sqrt error");
        if (x.rawValue == 0)
            return Fix.fixZero;
        Fix k = x + Fix.fixOne >> 1;
        for (int i = 0; i < num; i++)
            k = (k + (x / k)) >> 1;
        if (k.rawValue < 0)
            throw new ArithmeticException("overflow");
        return k;
    }

    public static Fix Sqrt(Fix x)
    {
        int num = 8;
        if (x.rawValue > 0x64000)
            num = 12;
        if (x.rawValue > 0x3e8000)
            num = 16;
        return Sqrt(x, num);
    }

    #region Sin
    private static int[] sinTable = {
        0, 71, 142, 214, 285, 357, 428, 499, 570, 641,
        711, 781, 851, 921, 990, 1060, 1128, 1197, 1265, 1333,
        1400, 1468, 1534, 1600, 1665, 1730, 1795, 1859, 1922, 1985,
        2048, 2109, 2170, 2230, 2290, 2349, 2407, 2464, 2521, 2577,
        2632, 2686, 2740, 2793, 2845, 2896, 2946, 2995, 3043, 3091,
        3137, 3183, 3227, 3271, 3313, 3355, 3395, 3434, 3473, 3510,
        3547, 3582, 3616, 3649, 3681, 3712, 3741, 3770, 3797, 3823,
        3849, 3872, 3895, 3917, 3937, 3956, 3974, 3991, 4006, 4020,
        4033, 4045, 4056, 4065, 4073, 4080, 4086, 4090, 4093, 4095,
        4096
    };

    // 插值 x度数 y小数
    private static Fix SinLookUp(Fix x, Fix y)
    {
        if (y > 0 && y < Fix.FromRaw(10) && x < Fix.FromRaw(90))
            return Fix.FromRaw(sinTable[x.RawValue]) +
                ((Fix.FromRaw(sinTable[x.rawValue + 1]) - Fix.FromRaw(sinTable[x.rawValue])) /
                Fix.FromRaw(10)) * y;
        else
            return Fix.FromRaw(sinTable[x.rawValue]);
    }

    // x弧度值
    private static Fix Sin(Fix x)
    {
        Fix y = (Fix)0;
        for (; x < Fix.fixZero; )
            x += Fix.FromRaw(25736);
        if (x > Fix.FromRaw(25736))
            x %= Fix.FromRaw(25736);
        Fix z = (x * Fix.FromRaw(10)) / Fix.FromRaw(714);
        if (x != Fix.fixZero &&
            x != Fix.FromRaw(6434) &&
            x != Fix.FromRaw(12868) &&
            x != Fix.FromRaw(19302) &&
            x != Fix.FromRaw(25736))
            y = (x * Fix.FromRaw(100)) / Fix.FromRaw(714) - z * Fix.FromRaw(10);
        if (z <= Fix.FromRaw(90))
            return SinLookUp(z, y);
        else if (z <= Fix.FromRaw(180))
            return SinLookUp(Fix.FromRaw(180) - z, y);
        else if (z <= Fix.FromRaw(270))
            return -SinLookUp(z - Fix.FromRaw(180), y);
        else
            return -SinLookUp(Fix.FromRaw(360) - z, y);
    }
    #endregion

    #region Cos, Tan, Asin
    public static Fix Cos(Fix x)
    {
        return Sin(x + Fix.FromRaw(6435));
    }

    public static Fix Tan(Fix x)
    {
        return Sin(x) / Cos(x);
    }

    public static Fix Asin(Fix x)
    {
        bool isNegative = x < 0;
        x = Abs(x);
        if (x > Fix.fixOne)
            throw new ArithmeticException("bad Asin arg");
        Fix f1 = ((((Fix.FromRaw(145103 >> fractionalPlace) * x) -
            Fix.FromRaw(599880 >> fractionalPlace) * x) +
            Fix.FromRaw(1420468 >> fractionalPlace) * x) -
            Fix.FromRaw(3592413 >> fractionalPlace) * x) +
            Fix.FromRaw(26353447 >> fractionalPlace);
        Fix f2 = fixPi / (Fix)2 - (Sqrt(Fix.fixOne - x) * f1);
        return isNegative ? -f2 : f2;
    }
    #endregion

    #region ATan Atan2
    public static Fix Atan(Fix x)
    {
        return Asin(x / Sqrt(Fix.fixOne + (x * x)));
    }

    public static Fix Atan2(Fix f1, Fix f2)
    {
        if (f1.rawValue == 0 && f1.rawValue == 0)
            return (Fix)0;
        Fix res = (Fix)0;
        if (f2 > 0)
            res = Atan(f1 / f2);
        else if (f2 < 0)
        {
            if (f1 >= (Fix)0)
                res = fixPi - Atan(Abs(f1 / f2));
            else
                res = -fixPi + Atan(Abs(f1 / f2));
        }
        else
            res = (f1 >= (Fix)0 ? fixPi : -fixPi) / (Fix)2;
        return res;
    }
    #endregion
}

public struct FixVector3
{
    public Fix x;
    public Fix y;
    public Fix z;

    public FixVector3(Fix x, Fix y, Fix z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public FixVector3(FixVector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Fix this[int index]
    {
        get
        {
            if (index == 0)
                return x;
            else if (index == 1)
                return y;
            else
                return z;
        }
        set
        {
            if (index == 0)
                x = value;
            else if (index == 1)
                y = value;
            else
                z = value;
        }
    }

    public static FixVector3 Zero
    {
        get { return new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero); }
    }

    public static FixVector3 operator +(FixVector3 a, FixVector3 b)
    {
        return new FixVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static FixVector3 operator -(FixVector3 a, FixVector3 b)
    {
        return new FixVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static FixVector3 operator *(FixVector3 a, Fix b)
    {
        return new FixVector3(a.x * b, a.y * b, a.z * b);
    }

    public static FixVector3 operator *(Fix a, FixVector3 b)
    {
        return new FixVector3(b.x * a, b.y * a, b.z * a);
    }

    public static FixVector3 operator /(FixVector3 a, Fix b)
    {
        return new FixVector3(a.x / b, a.y / b, a.z / b);
    }

    public static bool operator ==(FixVector3 a, FixVector3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(FixVector3 a, FixVector3 b)
    {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }

    public static Fix SqrMod(FixVector3 a)
    {
        return a.x * a.x + a.y * a.y + a.z * a.z;
    }

    public static Fix Mod(FixVector3 a)
    {
        return Fix.Sqrt(SqrMod(a));
    }

    public static Fix Distance(FixVector3 a, FixVector3 b)
    {
        return Mod(a - b);
    }

    public void Normalize()
    {
        Fix n = x * x + y * y + z * z;
        if (n == Fix.fixZero)
            return;
        n = Fix.Sqrt(n);
        if (n < (Fix)0.0001)
            return;
        n = 1 / n;
        x *= n;
        y *= n;
        z *= n;
    }

    public FixVector3 GetNormalize()
    {
        FixVector3 v = new FixVector3(this);
        v.Normalize();
        return v;
    }

    public override string ToString()
    {
       return string.Format("x:{0} y:{1} z:{2}", x, y, z);
    }

    public override bool Equals(object obj)
    {
        return obj is FixVector3 && ((FixVector3)obj) == this;
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode() + this.z.GetHashCode();
    }

    public static FixVector3 Lerp(FixVector3 a, FixVector3 b, Fix c)
    {
        return a * (1 - c) + b * c;
    }

    #if _CLIENT_LOGIC_
    public UnityEngine.Vector3 ToVector3()
    {
        return new UnityEngine.Vector3((float)x, (float)y, (float)z);
    }
    #endif
}

public struct FixVector2
{
    public Fix x;
    public Fix y;

    public FixVector2(Fix x, Fix y)
    {
        this.x = x;
        this.y = y;
    }

    public FixVector2(FixVector2 v)
    {
        this.x = v.x;
        this.y = v.y;
    }

    public Fix this[int index]
    {
        get
        {
            if (index == 0)
                return x;
            else
                return y;
        }
        set
        {
            if (index == 0)
                x = value;
            else
                y = value;
        }
    }

    public static FixVector2 Zero
    {
        get { return new FixVector2(Fix.fixZero, Fix.fixZero); }
    }

    public static FixVector2 operator +(FixVector2 a, FixVector2 b)
    {
        return new FixVector2(a.x + b.x, a.y + b.y);
    }

    public static FixVector2 operator -(FixVector2 a, FixVector2 b)
    {
        return new FixVector2(a.x - b.x, a.y - b.y);
    }

    public static FixVector2 operator *(FixVector2 a, Fix b)
    {
        return new FixVector2(a.x * b, a.y * b);
    }

    public static FixVector2 operator *(Fix a, FixVector2 b)
    {
        return new FixVector2(b.x * a, b.y * a);
    }

    public static FixVector2 operator /(FixVector2 a, Fix b)
    {
        return new FixVector2(a.x / b, a.y / b);
    }

    public static bool operator ==(FixVector2 a, FixVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(FixVector2 a, FixVector2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static Fix SqrMod(FixVector2 a)
    {
        return a.x * a.x + a.y * a.y;
    }

    public static Fix Mod(FixVector2 a)
    {
        return Fix.Sqrt(SqrMod(a));
    }

    public static Fix Distance(FixVector2 a, FixVector2 b)
    {
        return Mod(a - b);
    }

    public void Normalize()
    {
        Fix n = x * x + y * y;
        if (n == Fix.fixZero)
            return;
        n = Fix.Sqrt(n);
        if (n < (Fix)0.0001)
            return;
        n = 1 / n;
        x *= n;
        y *= n;
    }

    public FixVector2 GetNormalize()
    {
        FixVector2 v = new FixVector2(this);
        v.Normalize();
        return v;
    }

    public override string ToString()
    {
        return string.Format("x:{0} y:{1}", x, y);
    }

    public override bool Equals(object obj)
    {
        return obj is FixVector2 && ((FixVector2)obj) == this;
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode();
    }

    public static FixVector2 Lerp(FixVector2 a, FixVector2 b, Fix c)
    {
        return a * (1 - c) + b * c;
    }

#if _CLIENT_LOGIC_
    public UnityEngine.Vector2 ToVector2()
    {
        return new UnityEngine.Vector2((float)x, (float)y);
    }
#endif
}

public struct NormalVector2
{
    public float x;
    public float y;

    public NormalVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public NormalVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public NormalVector2(NormalVector2 v)
    {
        this.x = v.x;
        this.y = v.y;
    }

    public static NormalVector2 operator -(NormalVector2 a, int b)
    {
        return new NormalVector2(a.x - b, a.y - b);
    }

    public float this[int index]
    {
        get 
        {
            if (index == 0)
                return x;
            else
                return y;
        }
        set
        {
            if (index == 0)
                x = value;
            else
                y = value;
        }
    }

    public static NormalVector2 Zero
    {
        get
        {
            return new NormalVector2(0, 0);
        }
    }

    public static NormalVector2 operator +(NormalVector2 a, NormalVector2 b)
    {
        return new NormalVector2(a.x + b.y, a.y + b.y);
    }

    public static NormalVector2 operator -(NormalVector2 a, NormalVector2 b)
    {
        return new NormalVector2(a.x - b.y, a.y - b.y);
    }

    public static NormalVector2 operator *(NormalVector2 a, float b)
    {
        return new NormalVector2(a.x * b, a.y * b);
    }

    public static NormalVector2 operator *(float a, NormalVector2 b)
    {
        return new NormalVector2(a * b.x, a * b.y);
    }

    public static NormalVector2 operator /(NormalVector2 a, float b)
    {
        return new NormalVector2(a.x / b, a.y / b);
    }

    public static bool operator ==(NormalVector2 a, NormalVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(NormalVector2 a, NormalVector2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public override bool Equals(object obj)
    {
        return obj is NormalVector2 && ((NormalVector2)obj) == this;
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode();
    }

    public static float SqrMod(NormalVector2 a)
    {
        return a.x * a.x + a.y * a.y;
    }

    public static float Mod(NormalVector2 a)
    {
        // ???
        return SqrMod(a);
    }

    public static float Distance(NormalVector2 a, NormalVector2 b)
    {
        return Mod(a - b);
    }

    public void Normalize()
    {
        float n = x * x + y * y;
        if (n == 0)
            return;
        // n = float.Sqrt(n);
        if (n < (float)0.0001)
            return;
        n = 1 / n;
        x *= n;
        y *= n;
    }

    public NormalVector2 GetNormalize()
    {
        NormalVector2 v = new NormalVector2(this);
        v.Normalize();
        return v;
    }

    public override string ToString()
    {
        return string.Format("x:{0} y:{1}", x, y);
    }

#if _CLIENT_LOGIC_
    public UnityEngine.Vector2 ToVector2()
    {
        return new UnityEngine.Vector2((float)x, (float)y);
    }
#endif
}

public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public IntVector2(IntVector2 v)
    {
        this.x = v.x;
        this.y = v.y;
    }

    public static IntVector2 operator -(IntVector2 a, int b)
    {
        return new IntVector2(a.x - b, a.y - b);
    }

    public int this[int index]
    {
        get
        {
            if (index == 0)
                return x;
            else
                return y;
        }
        set
        {
            if (index == 0)
                x = value;
            else
                y = value;
        }
    }

    public static IntVector2 Zero
    {
        get
        {
            return new IntVector2(0, 0);
        }
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x - b.x, a.y - b.y);
    }

    public static IntVector2 operator *(IntVector2 a, int b)
    {
        return new IntVector2(a.x * b, a.y * b);
    }

    public static IntVector2 operator *(int a, IntVector2 b)
    {
        return new IntVector2(a * b.x, a * b.y);
    }

    public static IntVector2 operator /(IntVector2 a, int b)
    {
        return new IntVector2(a.x / b, a.y / b);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public override bool Equals(object obj)
    {
        return obj is IntVector2 && ((IntVector2)obj) == this;
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() + this.y.GetHashCode();
    }

    public static int SqrMod(IntVector2 a)
    {
        return a.x * a.x + a.y * a.y;
    }

    public static int Mod(IntVector2 a)
    {
        // ???
        return IntVector2.SqrMod(a);
    }

    public static int Distance(IntVector2 a, IntVector2 b)
    {
        return Mod(a - b);
    }

    public void Normalize()
    {
        int n = x * x + y * y;
        if (n == 0)
            return;
        // n = int.Sqrt(n);
        if (n < (int)0.0001)
        {
            return;
        }
        n = 1 / n;
        x *= n;
        y *= n;
    }

    public IntVector2 GetNormalize()
    {
        IntVector2 v = new IntVector2(this);
        v.Normalize();
        return v;
    }

    public override string ToString()
    {
        return string.Format("x:{0} y:{1}", x, y);
    }

#if _CLIENT_LOGIC_
    public UnityEngine.Vector2 ToVector2()
    {
        return new UnityEngine.Vector2((int)x, (int)y);
    }
#endif
}