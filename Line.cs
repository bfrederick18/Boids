using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Boids
{
    public class Line
    {
        protected Texture2D texture;
        protected Color tint;
        protected Vector2 p1;
        protected Vector2 p2;

        protected List<BaseSprite> pixels;

        public Vector2 P1
        {
            get { return p1; }
            set
            {
                p1 = value;
                Populate();
            }
        }
        public Vector2 P2
        {
            get { return p2; }
            set
            {
                p2 = value;
                Populate();
            }
        }

        public Line(Texture2D texture, Color tint, Vector2 p1, Vector2 p2)
        {
            this.texture = texture;
            this.tint = tint;
            this.p1 = p1;
            this.p2 = p2;

            Populate();
        }

        private void Populate()
        {
            pixels = new List<BaseSprite>();
            float x, y, dX, dY, step;

            dX = p2.X - p1.X;
            dY = p2.Y - p1.Y;

            if (Math.Abs(dX) >= Math.Abs(dY))
                step = Math.Abs(dX);
            else
                step = Math.Abs(dY);

            dX = dX / step;
            dY = dY / step;
            x = p1.X;
            y = p1.Y;

            for (int i = 1; i <= step; i++)
            {
                addPixel(x, y);
                x = x + dX;
                y = y + dY;
            }
        }

        public void addPixel(float x, float y)
        {
            pixels.Add(new BaseSprite(texture, new Vector2(x, y), tint, Vector2.One));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var e in pixels)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
