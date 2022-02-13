using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids
{
    public class BaseSprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle sourceRectangle;
        protected Color tint;
        protected float rotation;
        protected Vector2 scale;
        protected float layerDepth;
        protected Vector2 origin;
        protected SpriteEffects effects;

        protected float transparency;

        public BaseSprite(Texture2D texture, Vector2 position)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, 1.0f)
        {
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
        }

        public BaseSprite(Texture2D texture, Vector2 position, Color tint, Vector2 scale)
            : this(texture, position)
        {
            this.tint = tint;
            this.scale = scale;
        }

        public BaseSprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color tint, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, float transparency)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.tint = tint;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
            this.transparency = transparency;
        }

        public virtual void Update(GameTime gameTime, Viewport window) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, tint * transparency, rotation, origin, scale, effects, layerDepth);
        }

        public Vector2 GetPos()
        {
            return position;
        }
        public Rectangle GetSourceRect()
        {
            return sourceRectangle;
        }

        public void SetTint(Color tint)
        {
            this.tint = tint;
        }
    }
}
