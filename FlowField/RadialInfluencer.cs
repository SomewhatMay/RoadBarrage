using System;

namespace RoadBarrage.FlowField
{
    internal class RadialInfluencer : Influencer
    {
        public RadialInfluencer(int x, int y, int size)
        : base(x, y, size)
        {
            CalculateField();
        }

        protected override void CalculateField()
        {
            for (int blockX = 0; blockX < Constants.ChunkRes.ResolutionX; blockX++)
            {
                for (int blockY = 0; blockY < Constants.ChunkRes.ResolutionY; blockY++)
                {
                    int dx = blockX - x;
                    int dy = blockY - y;
                    double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                    if (distance > size)
                    {
                        dx = 0;
                        dy = 0;
                    }

                    field[blockX, blockY, 0] = dx;
                    field[blockX, blockY, 1] = dy;
                }
            }
        }
    }
}
