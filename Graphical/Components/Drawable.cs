using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Graphical.Components
{
    internal abstract class Drawable
    {
        protected Visuals visuals;
        protected GraphicsDevice graphicsDevice;
        protected SpriteBatch spriteBatch;

        public virtual void Initialize(Visuals visuals, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.visuals = visuals;
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Visualize()
        {

        }

        public virtual void Draw(GameTime game)
        {

        }
    }
}
