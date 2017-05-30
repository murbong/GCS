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
        public abstract void Draw(SpriteBatch sb);
    }

    public class Circle : Shape
    {
        public static int Sides = 100;
        public Vector2 Center;
        public float Radius;
        
        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public override void Draw(SpriteBatch sb)
        {
            GUI.DrawCircle(sb, Center, Radius, Border, Color, Sides);
        }
    }

    public class Line : Shape
    {
        public Vector2 Point1;
        public Vector2 Point2;
        public float Grad { get => (Point2 - Point1).Y / (Point2 - Point1).X; }
        public float Yint => (Point1.Y) - Grad * Point1.X;


        public Line(Vector2 p1, Vector2 p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        public override void Draw(SpriteBatch sb)
        {
            GUI.DrawLine(sb, Point1, Point2, Border, Color);
        }
    }


    public class Segment : Shape
    {
        public Vector2 Point1;
        public Vector2 Point2;
        public float Grad { get => (Point2 - Point1).Y / (Point2 - Point1).X; }
        public float Yint => (Point1.Y) - Grad * Point1.X;
        
 
        public Segment(Vector2 p1, Vector2 p2)
        {
            Point1 = p1;
            Point2 = p2;
        }

        public override void Draw(SpriteBatch sb)
        {
            GUI.DrawLine(sb, Point1, Point2, Border, Color);
        }
    }

    public class Dot : Shape
    {
        public Vector2 Coord;

        public Dot(Vector2 coord)
        {
            Coord = coord;
            Color = Color.Red;
        }

        public override bool Equals(object obj)
        {
            var o = obj as Dot;
            return o == null ? false : Coord.Equals(o.Coord);
        }

        public override int GetHashCode()
            => Coord.GetHashCode();

        public override void Draw(SpriteBatch sb)
        {
            GUI.DrawCircle(sb, Coord, 3f, Border, Color, 10);
            // GUI.DrawPoint(sb, Coord, Border, Color);
        }
    }
}
