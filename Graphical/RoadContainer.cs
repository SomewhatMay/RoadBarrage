using RoadBarrage.Algorithms;
using RoadBarrage.Flow;
using RoadBarrage.Graphical.Components;
using System;
using System.Collections.Generic;

namespace RoadBarrage.Graphical
{
    internal class RoadContainer
    {
        private DrawablesContainer drawablesContainer;

        public List<LinkedList<WorldPos>> Roads;

        public RoadContainer(DrawablesContainer drawablesContainer)
        {
            Roads = new List<LinkedList<WorldPos>>();
            this.drawablesContainer = drawablesContainer;
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
