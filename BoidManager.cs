using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Boids
{
    public class BoidManager
    {
        BoidSettings settings;
        private Texture2D texture;
        private Vector2 scale;

        private Boid[] boids;

        private Random rand;
        private Texture2D pixel;

        public BoidManager(Texture2D texture, Vector2 scale, Viewport window, int initHowMany, Texture2D pixel)
        {
            settings = new BoidSettings();

            this.texture = texture;
            this.scale = scale;

            boids = new Boid[initHowMany];

            rand = new Random();
            this.pixel = pixel;

            Populate(window);
        }

        void Populate(Viewport window)
        {
            int whichPopColor;
            for (int i = 0; i < boids.Length; i++)
            {
                whichPopColor = rand.Next(3);
                Vector2 popPos = new Vector2(rand.Next(window.Width), rand.Next(window.Height));
                Color popColor = new Color(0, 255 - (whichPopColor * 100), 0);
                Vector2 popVel = new Vector2((float)(rand.NextDouble() * 2 - 1), (float)(rand.NextDouble() * 2 - 1));
                boids[i] = new Boid(texture, popPos, popColor, scale, popVel, Vector2.Zero, popColor, pixel);
            }
        }

        public void Update(GameTime gameTime, Viewport window)
        {
            List<Vector2> obstacleList = new List<Vector2>();
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            for (int i = 0; i < boids.Length; i++)
            {
                //obstacleList = GetBorderPoints(boids[i], window);

                obstacleList.Add(new Vector2(window.Width / 2, window.Height / 2));

                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    obstacleList.Add(new Vector2(mouseState.X, mouseState.Y));
                }
                if (mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed)
                {
                    //CHECK DIST AND MAKE RED
                    Vector2 tPos = boids[i].GetPos();
                    double tDist = Math.Sqrt(Math.Pow(mouseState.X - tPos.X, 2) + Math.Pow(mouseState.Y - tPos.Y, 2));
                    if (tDist <= 20)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                            boids[i].Select();
                        if (mouseState.RightButton == ButtonState.Pressed)
                            boids[i].Deselect();
                    }
                }

                boids[i].avgAvoidanceHeading = Seperation(boids[i], boids);
                boids[i].avgFlockHeading = Alignment(boids[i], boids);
                boids[i].centerOfFlockmates = Cohesion(boids[i], boids);
                boids[i].obstaclePush = Obstacles(boids[i], obstacleList);
                boids[i].Update(gameTime, window);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Boid b in boids)
            {
                if (b.IsSelected())
                {
                    b.SetTint(new Color(255, 10, 10)); //RED
                }
                else
                {
                    b.SetTint(b.popColor);
                }
                b.Draw(spriteBatch);
            }
        }

        public Vector2 Alignment(Boid boid, Boid[] boids)
        {
            float radius = settings.alignmentRadius;
            Vector2 align = new Vector2();
            int count = 0;

            foreach (Boid other in boids)
            {
                var distance = Vector2.Distance(boid.GetPos(), other.GetPos());
                var fwdAngle = Math.Atan2(boid.fwd.Y, boid.fwd.X) - Math.PI;

                Vector2 v1 = new Vector2((float)(Math.Cos(fwdAngle - (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle - (Math.PI / 2 * 3)) * radius));
                Vector2 v2 = new Vector2((float)(Math.Cos(fwdAngle + (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle + (Math.PI / 2 * 3)) * radius));

                Vector2 relPoint = other.GetPos() - boid.GetPos();

                if (!(-v1.X * relPoint.Y + v1.Y * relPoint.X > 0) &&
                    -v2.X * relPoint.Y + v2.Y * relPoint.X > 0 &&
                    relPoint.X * relPoint.X + relPoint.Y * relPoint.Y <= radius * radius)
                {
                    align += other.GetVel();
                    count++;
                }
            }

            if (count > 0)
                align /= count;

            var dir = align - boid.GetVel();
            dir.Normalize();
            return dir;
        }

        public Vector2 Seperation(Boid boid, Boid[] boids)
        {
            float radius = settings.seperationRadius;
            Vector2 avoid = new Vector2();
            int count = 0;

            foreach (Boid other in boids)
            {
                var distance = Vector2.Distance(boid.GetPos(), other.GetPos());
                var fwdAngle = Math.Atan2(boid.fwd.Y, boid.fwd.X) - Math.PI;

                Vector2 v1 = new Vector2((float)(Math.Cos(fwdAngle - (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle - (Math.PI / 2 * 3)) * radius));
                Vector2 v2 = new Vector2((float)(Math.Cos(fwdAngle + (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle + (Math.PI / 2 * 3)) * radius));

                Vector2 relPoint = other.GetPos() - boid.GetPos();

                if (!(-v1.X * relPoint.Y + v1.Y * relPoint.X > 0) &&
                    -v2.X * relPoint.Y + v2.Y * relPoint.X > 0 &&
                    relPoint.X * relPoint.X + relPoint.Y * relPoint.Y <= radius * radius)
                {
                    avoid += (boid.GetPos() - other.GetPos()) / (float)Math.Pow(distance, 2);
                    count++;
                }
            }

            if (count > 0)
                avoid /= count;

            if (avoid.Length() > 0)
                avoid.Normalize();
            else
                avoid = Vector2.Zero;

            return avoid;
        }

        public Vector2 Cohesion(Boid boid, Boid[] boids)
        {
            float radius = settings.cohesionRadius;
            Vector2 cohere = new Vector2();
            int count = 0;

            foreach (Boid other in boids)
            {
                var distance = Vector2.Distance(boid.GetPos(), other.GetPos());
                var fwdAngle = Math.Atan2(boid.fwd.Y, boid.fwd.X) - Math.PI;

                Vector2 v1 = new Vector2((float)(Math.Cos(fwdAngle - (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle - (Math.PI / 2 * 3)) * radius));
                Vector2 v2 = new Vector2((float)(Math.Cos(fwdAngle + (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle + (Math.PI / 2 * 3)) * radius));

                Vector2 relPoint = other.GetPos() - boid.GetPos();

                if (distance < radius && distance > 0)
                {
                    cohere += other.GetPos();
                    count++;
                }
            }

            if (count > 0)
                cohere /= count;

            var dir = cohere - boid.GetPos();
            dir.Normalize();
            return dir;
        }

        public Vector2 Obstacles(Boid boid, List<Vector2> points)
        {
            float radius = settings.obstacleRadius;
            Vector2 avoid = new Vector2();
            int count = 0;

            foreach (Vector2 point in points)
            {
                var distance = Vector2.Distance(boid.GetPos(), point);
                var fwdAngle = Math.Atan2(boid.fwd.Y, boid.fwd.X) - Math.PI;

                Vector2 v1 = new Vector2((float)(Math.Cos(fwdAngle - (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle - (Math.PI / 2 * 3)) * radius));
                Vector2 v2 = new Vector2((float)(Math.Cos(fwdAngle + (Math.PI / 2 * 3)) * radius), (float)(Math.Sin(fwdAngle + (Math.PI / 2 * 3)) * radius));

                Vector2 relPoint = point - boid.GetPos();

                if (!(-v1.X * relPoint.Y + v1.Y * relPoint.X > 0) &&
                    -v2.X * relPoint.Y + v2.Y * relPoint.X > 0 &&
                    relPoint.X * relPoint.X + relPoint.Y * relPoint.Y <= radius * radius)
                {
                    avoid += (boid.GetPos() - point) / (float)Math.Pow(distance, 10);
                    count++;
                }
            }

            if (count > 0)
                avoid /= count;

            if (avoid.Length() > 0)
                avoid.Normalize();
            else
                avoid = Vector2.Zero;

            return avoid;
        }

        private List<Vector2> GetBorderPoints(Boid boid, Viewport window)
        {
            return new List<Vector2>() {
            new Vector2(boid.GetPos().X, 0),
            new Vector2(boid.GetPos().X, window.Height),
            new Vector2(0, boid.GetPos().Y),
            new Vector2(window.Width, boid.GetPos().Y)
            };
        }
    }
}
