using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Visible
{
    internal class StaticWindow : Service
    {
        private Texture2D _texture;
        private Color[] _pixelData;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private readonly Random random = new Random();

        public StaticWindow()
        {
        }

        public void Initialize(GraphicsDevice _graphicsDevice)
        {
            this._graphicsDevice = _graphicsDevice;
            _texture = new Texture2D(_graphicsDevice, Constants.WIDTH, Constants.HEIGHT);
            _pixelData = new Color[Constants.WIDTH * Constants.HEIGHT];

            base.Initialize();
        }
        public void LoadContent(SpriteBatch _spriteBatch)
        {
            this._spriteBatch = _spriteBatch;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < _pixelData.Length; i++)
            {
                _pixelData[i] = random.Next(2) == 0 ? Color.Black : Color.White;
            }

            _texture.SetData(_pixelData);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);

            base.Draw(gameTime);
        }
    }
}
