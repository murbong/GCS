﻿using System;
using Grid.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCS
{
    public abstract class Shape
    {
        public string Name { get; set; }
        public float Border { get; set; } = 2f;
        public Color Color { get; set; } = Color.Black;
        public bool Focused { get; set; } = false;
        public bool Selected { get; set; } = false;
        /// <summary>
        /// 마우스를 떼면 Selected가 false가 되어야 하는가?
        /// </summary>
        internal bool UnSelect { get; set; } = false;
        public virtual void Draw(SpriteBatch sb)
        {
            Color = Color.Black;
            if (Focused) Color = Color.Orange;
            if (Selected) Color = Color.Cyan;
        }
        public abstract void Move(Vector2 add);
    }

    public class Circle : Shape
    {
        public static int Sides = 100;
        public Dot Center;
        public Dot Another;
        public float Radius => Vector2.Distance(Center.Coord, Another.Coord);

        public Circle(Dot center, Dot another)
        {
            Center = center;
            Another = another;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            GUI.DrawCircle(sb, Center.Coord, Radius, Border, Color, Sides);
        }

        public override void Move(Vector2 add)
        {
            Center.Move(add);
            Another.Move(add);
        }
    }

    public class Line : Shape
    {
        private Dot _p1;
        public Dot Point1 { get => _p1; set { _p1 = value; ResetAB(); } }
        private Dot _p2;
        public Dot Point2 { get => _p2; set { _p2 = value; ResetAB(); } }

        private float _grad;
        public float Grad { get => _grad; set { _grad = value; ResetPoints(); } }
        private float _yint;
        public float Yint { get => _yint; set { _yint = value; ResetPoints(); } }

        public Line(Dot p1, Dot p2)
        {
            _p1 = p1;
            _p2 = p2;
            ResetAB();
        }

        public Line(float grad, float yint)
        {
            _grad = grad;
            _yint = yint;
            ResetPoints();
        }

        public Line(Dot p1, float grad)
        {
            _p1 = p1;
            _p2 = new Dot(Point1.Coord.X + 1, Point1.Coord.Y + grad);
            _grad = grad;
            _yint = p1.Coord.Y - grad * p1.Coord.X;
        }

        private void ResetAB()
        {
            _grad = (Point2.Coord - Point1.Coord).Y / (Point2.Coord - Point1.Coord).X;
            _yint = (Point1.Coord.Y) - Grad * Point1.Coord.X;
        }

        private void ResetPoints()
        {
            _p1 = new Dot(0, Yint);
            _p2 = new Dot(1, Grad + Yint);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            GUI.DrawLine(sb, new Vector2(0, Yint), new Vector2(Scene.CurrentScene.ScreenBounds.X, Scene.CurrentScene.ScreenBounds.X * Grad + Yint), Border, Color);
        }

        public override void Move(Vector2 add)
        {
            _p1.Move(add);
            _p2.Move(add);
            ResetAB();
        }
    }

    public class Segment : Shape
    {
        private Dot _p1;
        public Dot Point1 { get => _p1; set { _p1 = value; ResetAB(); } }
        private Dot _p2;
        public Dot Point2 { get => _p2; set { _p2 = value; ResetAB(); } }

        public float Grad { get; private set; }
        public float Yint { get; private set; }

        public Segment(Dot p1, Dot p2)
        {
            _p1 = p1;
            _p2 = p2;
            ResetAB();
        }

        private void ResetAB()
        {
            Grad = (Point2.Coord - Point1.Coord).Y / (Point2.Coord - Point1.Coord).X;
            Yint = (Point1.Coord.Y) - Grad * Point1.Coord.X;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            GUI.DrawLine(sb, Point1.Coord, Point2.Coord, Border, Color);
        }

        public Line ToLine()
        {
            return new Line(Grad, Yint);
        }

        public override void Move(Vector2 add)
        {
            _p1.Move(add);
            _p2.Move(add);
            ResetAB();
        }
    }

    public class Dot : Shape
    {
        public Vector2 Coord;

        public Dot(Vector2 coord)
        {
            Coord = coord;
            Color = Color.OrangeRed;
        }

        public Dot(float x, float y) : this(new Vector2(x, y))
        { }

        public override bool Equals(object obj)
        {
            var o = obj as Dot;
            return o == null ? false : Coord.Equals(o.Coord);
        }

        public override int GetHashCode()
            => Coord.GetHashCode();

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            GUI.DrawCircle(sb, Coord, 4f, Border, Color, 20);
            // GUI.DrawPoint(sb, Coord, Border, Color);
        }

        public override void Move(Vector2 add)
        {
            Coord += add;
        }
    }
}
