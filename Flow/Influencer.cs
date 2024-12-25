namespace RoadBarrage.Flow
{
    internal abstract class Influencer
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int size { get; private set; }

        // [x, y, 0] -> x component of angle at pos (x, y)
        // [x, y, 1] -> y component of angle at pos (x, y)
        public double[,,] field { get; private set; }

        public Influencer(int x, int y, int size)
        {
            this.x = x;
            this.y = y;
            this.size = size;

            field = new double[Constants.ChunkRes.ResolutionX, Constants.ChunkRes.ResolutionY, 2];
        }

        protected abstract void CalculateField();
    }
}
