using Microsoft.Xna.Framework;
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

        public Drawable(Visuals visuals)
        {
            this.visuals = visuals;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(GameTime time, bool softUpdate = false)
        {

        }
    }
}
