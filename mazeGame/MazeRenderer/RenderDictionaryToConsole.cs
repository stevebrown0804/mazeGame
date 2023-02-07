namespace mazeGenerator
{
    internal class RenderDictionaryToConsole : IMazeRenderer
    {
        public void Render(IMazeStorage maze, IMazeSolver? solver = null)
        {
            Dictionary<string, CellsAndWalls> dict = maze.GetDict();
            (int rows, int cols) = maze.GetRowsAndColumns();

            if (solver == null)
            {
                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        string str = $"r{i}c{j}";
                        if (dict[str].cell != null)
                            //Console.Write("  *  ");
                            Console.Write("     ");
                        if (dict[str].wallToTheRight != null)
                            Console.Write("|");
                        else
                            Console.Write(" ");

                    }
                    Console.WriteLine();
                    for (int j = 1; j <= cols; j++)
                    {
                        string str = $"r{i}c{j}";
                        if (dict[str].wallBelow != null)
                            Console.Write(" ___  ");
                        else
                            Console.Write("      ");
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            else // solver != null
            {
                List<CellsAndWalls> shortestPath = solver.GetShortestPath();
                Dictionary<string, bool> shortestPath_dict = new();
                for (int i = 0; i < shortestPath.Count; i++) 
                {
                    string str = $"r{shortestPath[i].cell.row}c{shortestPath[i].cell.col}";
                    shortestPath_dict[str] = true;
                }
                
                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        string str = $"r{i}c{j}";
                        if (dict[str].cell != null)
                        {
                            if(shortestPath_dict.ContainsKey(str))
                            {
                                Console.Write("  X  ");
                            }
                            else
                            {
                                //Console.Write("  *  ");
                                Console.Write("     ");
                            }
                        }
                            
                        if (dict[str].wallToTheRight != null)
                            Console.Write("|");
                        else
                            Console.Write(" ");

                    }
                    Console.WriteLine();
                    for (int j = 1; j <= cols; j++)
                    {
                        string str = $"r{i}c{j}";
                        if (dict[str].wallBelow != null)
                            Console.Write(" ___  ");
                        else
                            Console.Write("      ");
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                }

            }//END if(solver == null) / else
        
        }//END Render()

    }//END class RenderDictionaryToConsole
}
