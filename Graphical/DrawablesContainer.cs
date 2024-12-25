using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadBarrage.Graphical.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Graphical
{
    internal class DrawablesContainer
    {
        private readonly Visuals visuals;
        private readonly GraphicsDevice graphicsDevice;
        private readonly SpriteBatch spriteBatch;

        public List<Drawable> Drawables { get; private set; }

        public DrawablesContainer(Visuals visuals, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.visuals = visuals;
            Drawables = new List<Drawable>();
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        public void Add(Drawable drawable)
        {
            drawable.Initialize(visuals, graphicsDevice, spriteBatch);
            Drawables.Add(drawable);
        }

        public bool Remove(Drawable drawable)
        {
            return Drawables.Remove(drawable);
        }

        public void Clear()
        {
            Drawables.Clear();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Visualize()
        {
            foreach (Drawable item in Drawables)
            {
                item.Visualize();
            }

            visuals.SyncTexture();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Drawable item in Drawables)
            {
                item.Draw(gameTime);
            }
        }
    }
}
