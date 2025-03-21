using Graphs;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Game
{
    public partial class Dungeon
    {
        public GraphAlgorithms.Path GetShortestPath(Controller controller, Node start, Node goal)
        {
            if (start == null ||
                goal == null)
            {
                return null;
            }

            // initialize pathfinding
            foreach (Node node in m_nodes.Values)
            {
                node.ResetPathfinding();
            }

            // add start node
            start.m_fDistance = 0.0f;
            List<Node> open = new List<Node>();
            HashSet<Node> closed = new HashSet<Node>();
            open.Add(start);

            // search
            while (open.Count > 0)
            {
                // get next node (the one with the least remaining distance)
                Node current = open[0];
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].m_fDistance < current.m_fDistance)
                    {
                        current = open[i];
                    }
                }
                open.Remove(current);
                closed.Add(current);

                // found goal?
                if (current == goal)
                {
                    // construct path
                    GraphAlgorithms.Path path = new GraphAlgorithms.Path();
                    while (current != null)
                    {
                        path.Add(current.m_parentLink);
                        current = current != null && current.m_parentLink != null ? current.m_parentLink.Source : null;
                    }

                    path.RemoveAll(l => l == null);     // HACK: check if path contains null links
                    path.Reverse();
                    return path;
                }
                else
                {
                    foreach (DungeonLink link in current.Links)
                    {
                        if (link.Target is Node neighbor)
                        {
                            if (!closed.Contains(neighbor) && neighbor.Owner == null)
                            {
                                // Determine if the node is a trap
                                bool isTrap = neighbor is Node_Trap; 

                                float newDistance = current.m_fDistance + Vector3.Distance(current.WorldPosition, neighbor.WorldPosition) +      // Distance to node
                                                    neighbor.AdditionalCost + link.AdditionalCost;                                               // additional costs

                                // ASSIGNMENT PART 1/2! Apply different penalties for traps if its a hero or an enemy!!!
                                if (isTrap)
                                {
                                    if (controller is HeroController)
                                    {
                                        // Hero avoids traps, high penalty
                                        newDistance += 1000f; 
                                    }
                                    else
                                    {
                                        // Enemy doesn't avoid traps but gets a penalty
                                        newDistance = 0f; 
                                    }
                                }

                                if (closed.Contains(neighbor) ||
                                    open.Contains(neighbor))
                                {
                                    if (newDistance < neighbor.m_fDistance)
                                    {
                                        // re-parent neighbor node
                                        neighbor.m_fDistance = newDistance;
                                        neighbor.m_parentLink = link;
                                    }
                                }
                                else
                                {
                                    // add target to openlist
                                    neighbor.m_fDistance = newDistance;
                                    neighbor.m_parentLink = link;
                                    open.Add(neighbor);
                                }
                            }
                        }
                    }
                }
            }

            // no path found :(
            return null;
        }
    }
}