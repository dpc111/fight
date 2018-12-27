using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3
{
    public Fix[,] data = new Fix[3,3];

    public Matrix3()
    {
        Set(Fix.fix0, Fix.fix0, Fix.fix0,
            Fix.fix0, Fix.fix0, Fix.fix0,
            Fix.fix0, Fix.fix0, Fix.fix0);
    }

    public Matrix3(
        Fix x00, Fix x01, Fix x02,
        Fix x10, Fix x11, Fix x12,
        Fix x20, Fix x21, Fix x22)
    {
        Set(x00, x01, x02,
            x10, x11, x12,
            x20, x21, x22);
    }

    public void Set(
        Fix x00, Fix x01, Fix x02,
        Fix x10, Fix x11, Fix x12,
        Fix x20, Fix x21, Fix x22)
    {
        data[0, 0] = x00;
        data[0, 1] = x01;
        data[0, 2] = x02;
        data[1, 0] = x10;
        data[1, 1] = x11;
        data[1, 2] = x12;
        data[2, 0] = x20;
        data[2, 1] = x21;
        data[2, 2] = x22;
    }

    public static bool operator ==(Matrix3 m1, Matrix3 m2) 
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (m1.data[i, j] != m2.data[i, j])
                    return false;
            }
        }
        return true;
    }

    public static bool operator !=(Matrix3 m1, Matrix3 m2)
    {
        return !(m1 == m2);
    }

    public static Matrix3 operator +(Matrix3 m1, Matrix3 m2)
    {
        Matrix3 res = new Matrix3();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res.data[i, j] = m1.data[i, j] + m2.data[i, j];
            }
        }
        return res;
    }

    public static Matrix3 operator -(Matrix3 m1, Matrix3 m2)
    {
        Matrix3 res = new Matrix3();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res.data[i, j] = m1.data[i, j] - m2.data[i, j];
            }
        }
        return res;
    }

    public static Matrix3 operator -(Matrix3 m1)
    {
        Matrix3 res = new Matrix3();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res.data[i, j] = -m1.data[i, j];
            }
        }
        return res;
    }

    public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
    {
        Matrix3 res = new Matrix3();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res.data[i, j] =
                    m1.data[i, 0] * m2.data[0, j] +
                    m1.data[i, 1] * m2.data[1, j] +
                    m1.data[i, 2] * m2.data[2, j];
            }
        }
        return res;
    }

    public static Matrix3 operator *(Fix x, Matrix3 m1)
    {
        Matrix3 res = new Matrix3();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                res.data[i, j] = x * m1.data[i, j];
            }
        }
        return res;
    }

    public static Matrix3 operator *(Matrix3 m1, Fix x)
    {
        return x * m1;
    }

    public static FixVector3 operator *(Matrix3 m1, FixVector3 v) {
        FixVector3 res = new FixVector3();
        res.x= m1.data[0, 0] * v.x +
                m1.data[0, 1] * v.y +
                m1.data[0, 2] * v.z;
        res.y = m1.data[1, 0] * v.x +
                m1.data[1, 1] * v.y +
                m1.data[1, 2] * v.z;
        res.z = m1.data[2, 0] * v.x +
                m1.data[2, 1] * v.y +
                m1.data[2, 2] * v.z;
        return res;
    }

    // unity YXZ
    //X   
    //1       0       0
    //0       cos     sin
    //0       -sin    cos
    //Y   
    //cos     0       -sin
    //0       1       0
    //sin     0       cos
    //Z   
    //cos     sin     0
    //-sin    cos     0
    //0       0       1
    public static Matrix3 RollEulerAngleXYZ(Fix ax, Fix ay, Fix az)
    {
        Fix rx = Fix.Ang2Rad(ax);
        Fix ry = Fix.Ang2Rad(ay);
        Fix rz = Fix.Ang2Rad(az);
        Matrix3 res;
        Fix cos, sin;
        cos = Fix.Cos(rx);
        sin = Fix.Sin(rx);
        Matrix3 mx = new Matrix3(
            Fix.fix1, Fix.fix0, Fix.fix0,
            Fix.fix0, cos, -sin,
            Fix.fix0, sin, cos);
        cos = Fix.Cos(ry);
        sin = Fix.Sin(ry);
        Matrix3 my = new Matrix3(
            cos, Fix.fix0, sin,
            Fix.fix0, Fix.fix1, Fix.fix0,
            -sin, Fix.fix0, cos);
        cos = Fix.Cos(rz);
        sin = Fix.Sin(rz);
        Matrix3 mz = new Matrix3(
            cos, -sin, Fix.fix0,
            sin, cos, Fix.fix0,
            Fix.fix0, Fix.fix0, Fix.fix1);
        res = mx * (my * mz);
        return res;
    }

    public static Matrix3 RollEulerAngleYXZ(Fix ax, Fix ay, Fix az)
    {
        Fix rx = Fix.Ang2Rad(ax);
        Fix ry = Fix.Ang2Rad(ay);
        Fix rz = Fix.Ang2Rad(az);
        Matrix3 res;
        Fix cos, sin;
        cos = Fix.Cos(rx);
        sin = Fix.Sin(rx);
        Matrix3 mx = new Matrix3(
            Fix.fix1, Fix.fix0, Fix.fix0,
            Fix.fix0, cos, -sin,
            Fix.fix0, sin, cos);
        cos = Fix.Cos(ry);
        sin = Fix.Sin(ry);
        Matrix3 my = new Matrix3(
            cos, Fix.fix0, -sin,
            Fix.fix0, Fix.fix1, Fix.fix0,
            sin, Fix.fix0, cos);
        cos = Fix.Cos(rz);
        sin = Fix.Sin(rz);
        Matrix3 mz = new Matrix3(
            cos, sin, Fix.fix0,
            -sin, cos, Fix.fix0,
            Fix.fix0, Fix.fix0, Fix.fix1);
        res = my * (mx * mz);
        return res;
    }

    public static Matrix3 RollAxis(FixVector3 axis, Fix r) {
        axis.Normalize();
        Fix u = axis.x;
        Fix v = axis.y;
        Fix w = axis.z;
        Matrix3 res = new Matrix3();
        Fix sin = Fix.Sin(r);
        Fix cos = Fix.Cos(r);
        res.data[0, 0] = u * u + (1 - u * u) * cos;
        res.data[0, 1] = u * v * (1 - cos) - w * sin;
        res.data[0, 2] = u * w * (1 - cos) + v * sin;
        res.data[1, 0] = u * v * (1 - cos) + w * sin;
        res.data[1, 1] = v * v + (1 - v * v) * cos;
        res.data[1, 2] = v * w * (1 - cos) - u * sin;
        res.data[2, 0] = u * w * (1 - cos) - v * sin;
        res.data[2, 1] = v * w * (1 - cos) + u * sin;
        res.data[2, 2] = w * w + (1 - w * w) * cos;
        return res;
    }

    public static Matrix3 RollVecToVec(FixVector3 v1, FixVector3 v2) {
        FixVector3 axis = FixVector3.Cross(v1, v2);
        axis.Normalize();
        Fix angle = Fix.Acos((v1 * v2) / (FixVector3.Mod(v1) * FixVector3.Mod(v2)));
        return Matrix3.RollAxis(axis, angle);
    }

    public static Matrix3 RollVecToVecAxisY(FixVector3 v1, FixVector3 v2) {
        Fix angle = Fix.Acos((v1 * v2) / (FixVector3.Mod(v1) * FixVector3.Mod(v2)));
        return Matrix3.RollAxis(GameConst.AixY, angle);
    }
}
