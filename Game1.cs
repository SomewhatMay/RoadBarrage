using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoadBarrage.Visible;

namespace RoadBarrage
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private StaticWindow staticWindow;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Constants.WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.HEIGHT;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            staticWindow = new StaticWindow();
        }

        protected override void Initialize()
        {
            staticWindow.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            staticWindow.LoadContent(_spriteBatch);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            staticWindow.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            staticWindow.Draw(gameTime);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
