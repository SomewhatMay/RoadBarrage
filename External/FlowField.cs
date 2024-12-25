using Microsoft.Xna.Framework;

namespace RoadBarrage.External
{
    internal class FlowField
    {
        private Visuals visuals;

        public double[,] Angles { get; private set; } = 
            new double[Constants.ChunkResolution.ResolutionX, Constants.ChunkResolution.ResolutionX];

        public FlowField(Visuals visuals)
        {
            this.visuals = visuals;
        }

        public void Visualize()
        {
            Color[] worldData = visuals.WorldData;

            for (int i = 0; i < worldData.Length; i++)
            {
                int x = i % Constants.ChunkResolution.ResolutionX;
                int y = i / Constants.ChunkResolution.ResolutionX;
            }
        }
    }
}
