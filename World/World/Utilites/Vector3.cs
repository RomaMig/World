using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Lenght { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            Lenght = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public Vector3(Point3 b, Point3 e)
        {
            X = e.X - b.X;
            Y = e.Y - b.Y;
            Z = e.Z - b.Z;
            Lenght = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 normalize()
        {
            X /= Lenght;
            Y /= Lenght;
            Z /= Lenght;
            Lenght = 1;
            return this;
        }

        public bool isNormal()
        {
            return Lenght == 1;
        }

        public void Rotate(float x, float y, float z)
        {
            RotateByX(x);
            RotateByY(y);
            RotateByZ(z);
        }

        public void RotateByX(float a)
        {
            float y = (float)(Math.Cos(a) * Y + Math.Sin(a) * Z);
            float z = (float)(-Math.Sin(a) * Y + Math.Cos(a) * Z);
            Y = y;
            Z = z;
        }

        public void RotateByY(float a)
        {
            float x = (float)(Math.Cos(a) * X - Math.Sin(a) * Z);
            float z = (float)(Math.Sin(a) * X + Math.Cos(a) * Z);
            X = x;
            Z = z;
        }

        public void RotateByZ(float a)
        {
            float x = (float)(Math.Cos(a) * X + Math.Sin(a) * Y);
            float y = (float)(-Math.Sin(a) * X + Math.Cos(a) * Y);
            X = x;
            Y = y;
        }

        public static Vector3 normalize(Vector3 v)
        {
            return new Vector3(
                v.X / v.Lenght,
                v.Y / v.Lenght,
                v.Z / v.Lenght);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            float x = v1.Y * v2.Z - v1.Z * v2.Y;
            float y = v1.Z * v2.X - v1.X * v2.Z;
            float z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector3(x, y, z);
        }
    }
}
