using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RoadBarrage.Graphical.Components
{
    internal class Road : Drawable
    {
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }
        public int Thickness { get; private set; }

        private Vector2 startPoint, endPoint, edge;
        float angle;
        private Texture2D texture;
        private Rectangle rect;

        public Road(int startX, int startY, int endX, int endY, int thickness)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            Thickness = thickness;

            startPoint = new Vector2(startX, startY);
            endPoint = new Vector2(endX, endY);
        }

        public override void Initialize(Visuals visuals, GraphicsDevice graphicsDevice, SpriteBatch spritebatch)
        {
            base.Initialize(visuals, graphicsDevice, spritebatch);

            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });
            edge = endPoint - startPoint;
            angle = (float)Math.Atan2(edge.Y, edge.X);
            rect = new Rectangle((int)startPoint.X, (int)startPoint.Y, (int)edge.Length(), Thickness);
        }

        public override void Draw(GameTime game)
        {
            spriteBatch.Draw(texture,
                rect,
                null,
                Color.Gray,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );

            base.Draw(game);
        }
    }
}
