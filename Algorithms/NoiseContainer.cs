using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadBarrage.Algorithms
{
    internal class NoiseContainer
    {
        public static int Seed { get; private set; }
        public static List<FastNoiseLite> Noises { get; private set; } = new List<FastNoiseLite>();

        protected static int noiseOffset = 1;

        public static void SetSeed(int seed)
        {
            Seed = seed;
            noiseOffset = 1;

            for (int i = 0; i < Noises.Count; i++)
            {
                Noises[i].SetSeed(seed + i);
            }
        }

        public static FastNoiseLite CreateNoise(float frequency = 0.01f)
        {
            int newSeed = Seed + noiseOffset;
            FastNoiseLite noiseFunction = new FastNoiseLite(newSeed);
            Noises.Add(noiseFunction);
            noiseOffset++;

            return noiseFunction;
        }
    }
}
