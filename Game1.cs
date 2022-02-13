using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Boids
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D pixel;
        Texture2D gTri;

        BoidManager bManager;
        SpriteFont font1;

        DateTime lastTime;
        TimeSpan elapsedTime;
        float fPS;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1801;
            graphics.PreferredBackBufferHeight = 1001;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            gTri = Content.Load<Texture2D>("greenTriangle3");
            bManager = new BoidManager(gTri, new Vector2(0.095f, 0.12f), GraphicsDevice.Viewport, 200, pixel);

            font1 = Content.Load<SpriteFont>("Font1");
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bManager.Update(gameTime, GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            bManager.Draw(spriteBatch);

            spriteBatch.Draw(pixel, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 1, 1), Color.White);

            elapsedTime = DateTime.Now - lastTime;
            fPS = (float)(1.0f / elapsedTime.TotalSeconds);
            spriteBatch.DrawString(font1, "FPS: " + String.Format("{0:0.00}", fPS) + " (" + elapsedTime + ")", new Vector2(0, 0), Color.Green);
            lastTime = DateTime.Now;

            spriteBatch.DrawString(font1, "LClick on BOIDS to highlight | RClick to un-highlight\n        Press Z to turn mouse into an obsticle", new Vector2(GraphicsDevice.Viewport.Width / 2 - 190, GraphicsDevice.Viewport.Height / 2 - 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
