using System.Collections.Generic;

namespace mazeGenerator
{
    internal class MainClass
    {
        public static void Main()
        {
            //Console.WriteLine("main: MazeStorage_Dictionary -- depth-first -- iterative\n");  //TMP
            //Console.WriteLine("main: MazeStorage_Dictionary -- Prim's algorithm\n");

            //First we'll create the starting point for the maze
            IMazeStorage maze = new MazeStorage_Dictionary(10, 10);

            //Next up, we'll create a maze using a specific routine            
            //IMazeCreation mazeCreator = new DepthFirst_Iterative();
            IMazeCreation mazeCreator = new Prims();
            maze = maze.CreateMaze(mazeCreator);

            //We'll create a player object
            Player player = new(maze);
            
            //And a maze-solver object
            IMazeSolver solver = new MazeSolver();
            solver = solver.Solve(maze, player);

            //..and then render it
            IMazeRenderer renderTarget = new RenderDictionaryToConsole();
            //IMazeRenderer renderTarget = new RenderDictionaryToFile();
            maze.Render(renderTarget, solver);
            //maze.Render(renderTarget);

            //TODO: Have RenderDictionaryToConsole (and to file, eventually) incorporate the shortest path

            //TMP
            /*List<CellsAndWalls> shortestPath = solver.GetShortestPath();
            for (int i = 0; i < shortestPath.Count; i++)
            {
                Console.Write($"({shortestPath[i].cell.row}, {shortestPath[i].cell.col}) ");
            }
            Console.WriteLine("");*/
            //END TMP
        }
    }
}
