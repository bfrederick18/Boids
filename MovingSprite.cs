using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    public class MovingSprite : BaseSprite
    {
        protected Vector2 velocity;

        public MovingSprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color tint, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, float transparency, Vector2 velocity)
            : base(texture, position, sourceRectangle, tint, rotation, origin, scale, effects, layerDepth, transparency)
        {
            this.velocity = velocity;
        }

        public MovingSprite(Texture2D texture, Vector2 position, Color tint, Vector2 scale, Vector2 velocity)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), tint, 0f, scale, Vector2.One, SpriteEffects.None, 0, 1.0f, velocity) { }

        public MovingSprite(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity)
           : this(texture, position, Color.White, scale, velocity) { }

        public override void Update(GameTime gameTime, Viewport window)
        {
            position += velocity;
            base.Update(gameTime, window);
        }

        public Vector2 GetVel()
        {
            return velocity;
        }
    }
}
