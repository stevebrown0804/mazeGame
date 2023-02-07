using mazeGame.GameElements.Derived_classes;
using mazeGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mazeGame.Setup.Maze
{
    internal class Maze
    {
        internal IMazeStorage mazeStorage;
        internal IMazeCreation mazeCreator;
        internal Player player;
        internal IMazeSolver solver;

        internal Maze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> mazeElements)
        {
            //This seems like as good a place as any to create these lists
            mazeElements[MazeElement.ElementType.Cell] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Wall] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.BreadcrumbTrail] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.ShortestPath] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Hint] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Player] = new List<MazeElement>();

            //PASTED IN FROM MAZEGENERATRION
            //First we'll create the starting point for the maze
            /*IMazeStorage*/
            mazeStorage = new MazeStorage_Dictionary(size, size);

            //Next up, we'll create a maze using a specific routine            
            //IMazeCreation mazeCreator = new DepthFirst_Iterative();
            /*IMazeCreation*/ mazeCreator = new Prims();
            mazeStorage = mazeStorage.CreateMaze(mazeCreator);

            //We'll create a player object
            /*Player*/ player = new(mazeStorage);

            //And a maze-solver object
            /*IMazeSolver*/ solver = new MazeSolver();
            solver = solver.Solve(mazeStorage, player);

            //..and then render it
            //IMazeRenderer renderTarget = new RenderDictionaryToConsole();
            //IMazeRenderer renderTarget = new RenderDictionaryToFile();
            //maze.Render(renderTarget, solver);
            //maze.Render(renderTarget);
            //END PASTED IN etc.
        }

        internal void SetupMaze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> maze)
        {
            //TODO: set up the maze (ie. render it from one dictionary to a dictionary of 6 lists)

            

        }
    }
}
