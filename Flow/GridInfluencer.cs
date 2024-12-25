using System;

namespace RoadBarrage.Flow
{
    internal class GridInfluencer : Influencer
    {
        public double angle { get; private set; }
        private double dirX, dirY;
        public GridInfluencer(int x, int y, int size, double angle)
        : base(x, y, size)
        {
            this.angle = angle;

            dirX = Math.Cos(angle);
            dirY = Math.Sin(angle);

            CalculateField();
        }

        protected override void CalculateField()
        {
            for (int blockX = 0; blockX < Constants.WorldRes.ResolutionX; blockX++)
            {
                for (int blockY = 0; blockY < Constants.WorldRes.ResolutionY; blockY++)
                {
                    double distance = Math.Sqrt(Math.Pow(blockX - x, 2) + Math.Pow(blockY - y, 2));
                    double _dirX = dirX;
                    double _dirY = dirY;

                    if (distance > size)
                    {
                        _dirX = 0;
                        _dirY = 0;
                    }

                    field[blockX, blockY, 0] = _dirX;
                    field[blockX, blockY, 1] = _dirY;
                }
            }
        }
    }
}
