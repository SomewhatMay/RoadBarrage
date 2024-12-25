using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoadBarrage.Flow;
using RoadBarrage.Graphical;
using System;

namespace RoadBarrage
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly Random random = new Random();

        private FlowField flowField;
        private Visuals visuals;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Constants.WindowDimensions.Width;
            _graphics.PreferredBackBufferHeight = Constants.WindowDimensions.Height;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            visuals = new Visuals(_spriteBatch, GraphicsDevice);
            flowField = new FlowField(visuals);
            flowField.Visualize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            visuals.Draw();

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
