using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoadBarrage.Algorithms;
using RoadBarrage.Flow;
using RoadBarrage.Graphical;
using RoadBarrage.Graphical.Components;
using System;
using System.Diagnostics;

namespace RoadBarrage
{
    public class Game1 : Game
    {
        public readonly GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        private readonly Random random = new Random();
        private readonly FastNoiseLite noise = new FastNoiseLite();

        internal FlowField flowField;
        internal Visuals visuals;
        internal DrawablesContainer drawablesContainer;
        internal RoadContainer roadFollow;

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
            //NoiseContainer.SetSeed((int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % 10000));
            NoiseContainer.SetSeed(3920);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            visuals = new Visuals(_spriteBatch, GraphicsDevice);
            flowField = new FlowField(visuals);
            drawablesContainer = new DrawablesContainer(visuals, GraphicsDevice, _spriteBatch);
            roadFollow = new RoadContainer(drawablesContainer);

            roadFollow.Visualize();

            for (int cx = 0; cx < 3; cx++)
            {
                for (int cy = 0; cy < 3; cy++)
                {
                    Chunk chunk = new Chunk(this, cx, cy);
                    chunk.Visualize(true);
                }
            }
            visuals.SyncTexture();

            //flowField.AddInfluencer(new GridInfluencer(10, 10, 10, Math.PI / 4));
            //flowField.AddInfluencer(new RadialInfluencer(15, 15, 10));
            //flowField.Visualize();

            //drawablesContainer.Add(new Road(0, 0, Constants.WindowDimensions.Width, Constants.WindowDimensions.Height, 5));
            //drawablesContainer.Add(new Road(0, 10, 100, 10, 5));

            //drawablesContainer.Visualize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            drawablesContainer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Debug.WriteLine(1 / gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            drawablesContainer.Draw(gameTime);
            visuals.Draw();

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
