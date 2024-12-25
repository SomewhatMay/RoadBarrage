using Microsoft.Xna.Framework;
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


        public Road(Visuals visuals, int startX, int startY, int endX, int endY, int thickness) : base(visuals)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            Thickness = thickness;
        }

        public override void Draw(GameTime _, bool softUpdate = false)
        {
            visuals.SetWorldData(visuals.foreachPixel((x, y, color) =>
            {
                if (IsWithinRoadBounds(x, y))
                {
                    color = Color.White;
                }

                return color;
            }), softUpdate);
        }

        protected bool IsWithinRoadBounds(int x, int y)
        {
            // Vector from start to end of the road
            var dx = EndX - StartX;
            var dy = EndY - StartY;

            // Vector from start of the road to the point (x, y)
            var px = x - StartX;
            var py = y - StartY;

            // Calculate the length of the road line segment squared (avoid square roots for efficiency)
            var roadLengthSquared = dx * dx + dy * dy;

            if (roadLengthSquared == 0) // To prevent division by zero if the road is a point
                return false;

            // Projection of (px, py) onto the line segment, computing the ratio
            var projection = (px * dx + py * dy) / (float)roadLengthSquared;

            // Clamp projection between 0 and 1 to ensure the point is within the segment bounds
            projection = Math.Clamp(projection, 0, 1);

            // Closest point on the road segment to (x, y)
            var closestX = StartX + projection * dx;
            var closestY = StartY + projection * dy;

            // Distance from (x, y) to the closest point on the line
            var distSquared = (x - closestX) * (x - closestX) + (y - closestY) * (y - closestY);

            // If the distance is less than or equal to the thickness of the road
            return distSquared <= Thickness * Thickness;
        }
    }
}
