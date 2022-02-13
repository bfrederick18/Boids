using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    public class AcceleratingSprite : MovingSprite
    {
        protected Vector2 acceleration;
        protected float decelerationFactor;

        public AcceleratingSprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color tint, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, float transparency, Vector2 velocity, Vector2 acceleration, float decelerationFactor)
           : base(texture, position, sourceRectangle, tint, rotation, origin, scale, effects, layerDepth, transparency, velocity)
        {
            this.acceleration = acceleration;
            this.decelerationFactor = decelerationFactor;
        }

        public AcceleratingSprite(Texture2D texture, Vector2 position, Color tint, Vector2 scale, float transparency, Vector2 velocity, Vector2 acceleration, float decelerationFactor)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), tint, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0, transparency, velocity, acceleration, decelerationFactor) { }

        public AcceleratingSprite(Texture2D texture, Vector2 position, Color tint, Vector2 scale, Vector2 velocity, Vector2 acceleration, float decelerationFactor)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), tint, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0, 1.0f, velocity, acceleration, decelerationFactor) { }

        public AcceleratingSprite(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, Vector2 acceleration)
           : this(texture, position, Color.White, scale, velocity, acceleration, 1f) { }

        public override void Update(GameTime gameTime, Viewport window)
        {
            velocity += acceleration;
            velocity *= decelerationFactor;
            base.Update(gameTime, window);
        }
    }
}
