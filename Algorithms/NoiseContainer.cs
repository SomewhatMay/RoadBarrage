using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Algorithms
{
    internal class NoiseContainer
    {
        public static int Seed { get; private set; }
        public static List<FastNoiseLite> Noises { get; private set; } = new List<FastNoiseLite>();

        protected static int noiseOffset = 0;

        public static void SetSeed(int seed)
        {
            Seed = seed;
        }

        public static FastNoiseLite CreateNoise(float frequency = 0.01f)
        {
            FastNoiseLite noiseFunction = new FastNoiseLite(Seed + noiseOffset);
            Noises.Add(noiseFunction);
            noiseOffset++;

            return noiseFunction;
        }
    }
}
