using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Boids
{
    public class Boid : AcceleratingSprite
    {
        BoidSettings settings;
        public Vector2 fwd;

        public Vector2 avgAvoidanceHeading; // Seperation
        public Vector2 avgFlockHeading;     // Alignment
        public Vector2 centerOfFlockmates;  // Cohesion
        public Vector2 obstaclePush;        // Obstacles

        private Vector2 seperationForce;
        private Vector2 alignmentForce;
        private Vector2 cohesionForce;
        private Vector2 obstacleForce;
        private Vector2 momentumForce;

        private bool selected;
        public Color popColor;
        private Texture2D pixel;

        private List<Line> lines;

        public Boid(Texture2D texture, Vector2 position, Color tint, Vector2 scale, Vector2 velocity, Vector2 acceleration, Color popColor, Texture2D pixel)
            : base(texture, position, tint, scale, 1.0f, velocity, acceleration, 1.0f)
        {
            settings = new BoidSettings();
            fwd = new Vector2();
            selected = false;
            this.popColor = popColor;
            this.pixel = pixel;
        }

        public override void Update(GameTime gameTime, Viewport window)
        {
            acceleration = Vector2.Zero;

            seperationForce = SteerForce(avgAvoidanceHeading) * settings.seperationWeight;
            alignmentForce = SteerForce(avgFlockHeading) * settings.alignmentWeight;
            cohesionForce = SteerForce(centerOfFlockmates) * settings.cohesionWeight;
            obstacleForce = SteerForce(obstaclePush) * settings.obstacleWeight;
            momentumForce = velocity * settings.momentumWeight;

            acceleration += seperationForce;
            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += obstacleForce;
            acceleration += momentumForce;
            
            base.Update(gameTime, window); //Basic Vel and Pos Updates

            float speed = (float)Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)); // Clamp velocity and set "normalized" fwd
            fwd = velocity / new Vector2(speed);
            speed = Math.Clamp(speed, settings.minSpeed, settings.maxSpeed);
            velocity = fwd * new Vector2(speed);

            float normal = (float)Math.Atan2(velocity.Y, velocity.X); // Normal vector
            rotation = normal + (float)MathHelper.PiOver2;            // Plus 90 degrees (or PI / 2)

            float distW = (float)Math.Abs(Math.Sqrt(Math.Pow(texture.Width * scale.X / 2, 2) + Math.Pow(texture.Height * scale.Y / 2, 2)) * Math.Cos(normal)); // The perpendicular y component of the dist to the farthest corner

            position.X = position.X < -distW ? window.Width + distW : (position.X > window.Width + distW ? -distW : position.X);
            position.Y = position.Y < -distW ? window.Height + distW : (position.Y > window.Height + distW ? -distW : position.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (selected)
            {
                lines = new List<Line>();

                lines.Add(new Line(pixel, Color.White, position, position + (fwd * settings.lineMulti))); // Forward
                lines.Add(new Line(pixel, Color.Red, position, position + (seperationForce * settings.lineMulti * 2))); // Seperation
                lines.Add(new Line(pixel, Color.Yellow, position, position + (alignmentForce * settings.lineMulti * 4))); // Alignment
                lines.Add(new Line(pixel, Color.Blue, position, position + (cohesionForce * settings.lineMulti * 4))); // Cohesion
                lines.Add(new Line(pixel, Color.Pink, position, position + (obstacleForce * settings.lineMulti))); // Obstacle

                foreach (var e in lines)
                {
                    e.Draw(spriteBatch);
                }
            }
        }

        Vector2 SteerForce(Vector2 vector)
        {
            if (vector.Length() == 0 || (vector.X == 0 && vector.Y == 0))
                return Vector2.Zero;

            Vector2 v = vector;
            v.Normalize();
            v *= settings.maxSpeed;
            v -= velocity;
            v.Normalize();
            return v * Math.Min(v.Length(), settings.maxSteerForce);
        }

        public void Select()
        {
            selected = true;
        }
        public void Deselect()
        {
            selected = false;
        }
        public bool IsSelected()
        {
            return selected;
        }
    }
}