using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosGameScreensaver
{
    class Vector
    {

        public double X { get; set; }
        public double Y { get; set; } 

        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Vector Set(double x, double y)
        {
            this.X = x;
            this.Y = y;
            return this;
        }

        public Vector Set(Vector a)
        {
            this.X = a.X;
            this.Y = a.Y;
            return this;
        }

        
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator *(double b, Vector a)
        {
            return new Vector(a.X * b, a.Y * b);
        }

        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.X / b, a.Y / b);
        }

        public double Length()
        {
            return Math.Sqrt((this.X*this.X) + (this.Y*this.Y));
        }

        public double Distance(Vector a)
        {
            return (this - a).Length();
        }

        public Vector Normalize()
        {
            return this.Set(this/this.Length());
        }

        public double Angle()
        {
            if (this.Length() == 0)
                return 0;
            else
                return Math.Acos(this.X/this.Length());
        }

        public double AngleBetween(Vector a)
        {
            return (this.Angle() - a.Angle());
        }

        public Vector SetFromRotation(double r, double ang)
        {
            this.X = r * Math.Cos(ang);
            this.Y = r * Math.Sin(ang);
            return this;
        }

        public Vector SetFromRotation(double r, double ang, Vector orgin)
        {
            return this.Set(this.SetFromRotation(r, ang) + orgin);
        }

        public Vector SetRotation(double ang)
        {
            return this.SetFromRotation(this.Length(), ang);
        }

        public Vector Rotate(double ang)
        {
            return this.SetRotation(this.Angle() - ang);
        }

        public Vector RotateAround(Vector v, double ang)
        {
            this.Set(this - v);
            this.Rotate(ang);
            this.Set(this + v);
            return this;
        }

        public override string ToString()
        {
            
            return "(" + this.X + ", " + this.Y + ")";
        }

    }
}