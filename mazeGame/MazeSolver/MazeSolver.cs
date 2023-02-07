/*  Create an empty queue and enqueue the source cell having a distance 0 from the source (itself) and mark it as visited.
    Loop till queue is empty.
        Dequeue the front node.
        If the popped node is the destination node, then return its distance.
        Otherwise, for each of four adjacent cells of the current cell, enqueue each valid cell with +1 distance and mark them as visited.
    If all the queue nodes are processed, and the destination is not reached, then return false.*/

namespace mazeGenerator
{
    public class MazeSolver : IMazeSolver
    {
        List<CellsAndWalls> shortestPath;

        public MazeSolver()
        {
            shortestPath = new();
        }

        public IMazeSolver Solve(IMazeStorage maze, Player player)
        {
            //We'll want to find the shortest path from wherever the player current is, to the end
            //  We'll define "end" as lower-right corner.

            Dictionary<string, CellsAndWalls> dict = maze.GetDict();
            Dictionary<string, bool> visited = new();
            Dictionary<string, int> distances = new();
            Dictionary<string, string> whoQueuedMe = new();
            int theNodesDistance = 0;

            //Initialize the dictionaries we just declared
            foreach (var k in dict.Keys)
                visited[k] = false;

            foreach (var k in dict.Keys)
                distances[k] = 0;

            //Console.WriteLine("In progress: MazeSolver.Solve()\n");
            //
            // "Create an empty queue and enqueue the source cell having a distance 0 from the source (itself) and mark it as visited."
            //
            Queue<CellsAndWalls> q = new();

            string str_Start = $"r{player.startingPoint.row}c{player.startingPoint.col}";
            q.Enqueue(dict[str_Start]);
            visited[str_Start] = true;

            whoQueuedMe[str_Start] = "";

            //
            // "Loop till queue is empty."
            //
            while(q.Count > 0)
            {
                //
                // "Dequeue the front node."
                //
                CellsAndWalls theNode = q.Dequeue();
                string str_theNode = $"r{theNode.cell.row}c{theNode.cell.col}";
                player.position.row = theNode.cell.row; //Necessary?  TBD
                player.position.col = theNode.cell.col;

                //
                // "If the popped node is the destination node, then return its distance."
                //
                if(theNode.cell.row == player.goal.row & theNode.cell.col == player.goal.col)
                {
                    theNodesDistance = distances[str_theNode];
                    break;
                }
                else
                {
                    //
                    // "Otherwise, for each of four adjacent cells of the current cell, enqueue each valid cell with +1 distance and mark them as visited."
                    //
                    string str_above = $"r{theNode.cell.row - 1}c{theNode.cell.col}";
                    string str_below = $"r{theNode.cell.row + 1}c{theNode.cell.col}";
                    string str_toTheLeft = $"r{theNode.cell.row}c{theNode.cell.col - 1}";
                    string str_toTheRight = $"r{theNode.cell.row}c{theNode.cell.col + 1}";

                    if (dict.ContainsKey(str_above) && visited[str_above] == false && dict[str_above].wallBelow == null)   
                    {
                        distances[str_above] = distances[str_theNode] + 1;
                        whoQueuedMe[str_above] = str_theNode;
                        q.Enqueue(dict[str_above]);
                        visited[str_above] = true;
                    }
                    if (dict.ContainsKey(str_below) && visited[str_below] == false && dict[str_theNode].wallBelow == null)
                    {
                        distances[str_below] = distances[str_theNode] + 1;
                        whoQueuedMe[str_below] = str_theNode;
                        q.Enqueue(dict[str_below]);
                        visited[str_below] = true;
                     }
                    if (dict.ContainsKey(str_toTheLeft) && visited[str_toTheLeft] == false && dict[str_toTheLeft].wallToTheRight == null)
                    {
                        distances[str_toTheLeft] = distances[str_theNode] + 1;
                        whoQueuedMe[str_toTheLeft] = str_theNode;
                        q.Enqueue(dict[str_toTheLeft]);
                        visited[str_toTheLeft] = true;
                    }
                    if (dict.ContainsKey(str_toTheRight) && visited[str_toTheRight] == false && dict[str_theNode].wallToTheRight == null)
                    {
                        distances[str_toTheRight] = distances[str_theNode] + 1;
                        whoQueuedMe[str_toTheRight] = str_theNode;
                        q.Enqueue(dict[str_toTheRight]);
                        visited[str_toTheRight] = true;
                    }
                }//END if/else

            }//END while(q.Count > 0)

            //
            // "If all the queue nodes are processed, and the destination is not reached, then return false."
            //
            if (player.position.row != player.goal.row || player.position.col != player.goal.col)
            {
                theNodesDistance = -1;
            }

            //
            // With the algorithm done, we can wrap stuff up.
            //
 /*           if (theNodesDistance == -1)
            {
                Console.WriteLine("MazeSolver.Solve says: theNodesDistance was -1. Maze is unsolvable, apparently!");
            }
            else
            {
                Console.WriteLine($"MazeSolver.Solve says: theNodesDistance was equal to {theNodesDistance}.");
            }*/

            /*int visitedTrueCounter = 0;
            foreach (var k in visited.Keys)
            {
                if (visited[k] == true)
                    visitedTrueCounter++;
            }
            Console.WriteLine($"Visited.Count == true: {visitedTrueCounter}\n");*/

            //Generate the shortest path
            string str2 = $"r{player.goal.row}c{player.goal.col}";
            try
            {
                while (whoQueuedMe[str2] != "")
                {
                    shortestPath.Add(dict[str2]);
                    str2 = whoQueuedMe[str2];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in MazeSolver.Solve: {e.Message}");
            }

            //Console.WriteLine("Shortest path:");
            //Add the player's starting point to the list, then reverse the list
            string str_playerStartingPoint = $"r{player.startingPoint.row}c{player.startingPoint.col}";
            shortestPath.Add(dict[str_playerStartingPoint]);
            shortestPath.Reverse();

            /*for (int i = 0; i < shortestPath.Count; i++)
            {
                Console.Write($"({shortestPath[i].cell.row}, {shortestPath[i].cell.col}) ");
            }
            Console.WriteLine("");*/

            return this;

        }//end Solve()


        public List<CellsAndWalls> GetShortestPath()
        {
            return shortestPath;
        }

    }//END class MazeSolver
}
