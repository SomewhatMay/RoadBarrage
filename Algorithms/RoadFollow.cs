using RoadBarrage.Flow;
using RoadBarrage.Graphical;
using RoadBarrage.Graphical.Components;
using System;
using System.Collections.Generic;

namespace RoadBarrage.Algorithms
{
    internal class RoadFollow
    {
        private Visuals visuals;
        private DrawablesContainer drawablesContainer;
        private FlowField flowField;

        private FastNoiseLite roadNoise = NoiseContainer.CreateNoise();
        private Random random = new Random();

        public List<LinkedList<WorldPos>> Roads;

        public RoadFollow(Visuals visuals, DrawablesContainer drawablesContainer, FlowField flowField)
        {
            Roads = new List<LinkedList<WorldPos>>();

            this.drawablesContainer = drawablesContainer;
            this.flowField = flowField;
            this.visuals = visuals;
        }

        public void CalculateRoads()
        {
            LinkedList<WorldPos> road = new LinkedList<WorldPos>();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    road.AddLast(new WorldPos(random.Next(Constants.WindowDimensions.Width), random.Next(Constants.WindowDimensions.Height)));
                }
                Roads.Add(road);
            }
        }

        public void Visualize()
        {
            foreach (LinkedList<WorldPos> road in Roads)
            {
                WorldPos? previousNode = null;
                foreach (WorldPos node in road)
                {
                    if (previousNode != null)
                    {
                        drawablesContainer.Add(
                            new Road(
                                previousNode.Value.X,
                                previousNode.Value.Y,
                                node.X,
                                node.Y,
                                1
                            )
                        );
                    }

                    previousNode = node;
                }
            }
        }
    }
}
