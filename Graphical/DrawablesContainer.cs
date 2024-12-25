using Microsoft.Xna.Framework;
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
        private Visuals visuals;

        public List<Drawable> drawables { get; private set; }

        public DrawablesContainer(Visuals visuals)
        {
            this.visuals = visuals;
            drawables = new List<Drawable>();
        }

        public void Add(Drawable drawable)
        {
            drawables.Add(drawable);
        }

        public bool Remove(Drawable drawable)
        {
            return drawables.Remove(drawable);
        }

        public void Clear()
        {
            drawables.Clear();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            foreach (Drawable item in drawables)
            {
                item.Draw(gameTime, true);
            }

            visuals.SyncTexture();
        }
    }
}
