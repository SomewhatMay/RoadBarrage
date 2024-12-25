using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoadBarrage.External;
using System;
using System.Diagnostics;

namespace RoadBarrage
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly Random random = new Random();
        private Visuals visuals;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Constants.WindowDimensions.Width;
            _graphics.PreferredBackBufferHeight = Constants.WindowDimensions.Height;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Debug.Write(FlowField.DrawCross(0));
            //Debug.Write("Hello, world!");
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            visuals = new Visuals(_spriteBatch, GraphicsDevice);

            /**
            visuals.UpdateWorldData(0, 0, Color.Green);
            visuals.UpdateWorldData(0, Constants.ChunkRes.ResolutionY - 1, Color.Green);
            visuals.UpdateWorldData(Constants.ChunkRes.ResolutionX - 1, 0, Color.Green);
            visuals.UpdateWorldData(Constants.ChunkRes.ResolutionX - 1, Constants.ChunkRes.ResolutionY - 1, Color.Green);

            visuals.UpdateWorldData(3, 3, FlowField.DrawCross(20));

            Color[,] colorGrid = new Color[Constants.ChunkRes.BlockSize, Constants.ChunkRes.BlockSize];
            for (int x = 0; x < colorGrid.GetLength(0); x++)
            {
                for (int y = 0; y < colorGrid.GetLength(1); y++)
                {
                    int b = random.Next(255);
                    colorGrid[x, y] = new Color(b, b, b);
                }
            }
            visuals.UpdateWorldData(3, 3, colorGrid);
            */

            //visuals.UpdateWorldData(1, 2, FlowField.DrawCross(0));
            //visuals.UpdateWorldData(2, 2, FlowField.DrawCross(90));
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

            //int x = random.Next(Constants.ChunkRes.ResolutionX);
            //int y = random.Next(Constants.ChunkRes.ResolutionY);
            //Color c = Color.Green;
            //visuals.UpdateWorldData(x, y, c);

            visuals.UpdateWorldData(1, 2, FlowField.DrawCross((int)(gameTime.TotalGameTime.TotalSeconds * 100)));
            visuals.Draw();

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
