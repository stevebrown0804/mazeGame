using maze;
using mazeGame.GameElements.Derived_classes;
using mazeGenerator;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
/*using System.Text;
using System.Threading.Tasks;*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.ComponentModel.Design;
using mazeGame.GameElements.Base_classes;
using mazeGame.GameElements.Derived_classes.Maze_stuff;

namespace mazeGame.Setup.Maze
{
    internal class Maze
    {
        internal IMazeStorage mazeStorage;
        internal IMazeCreation mazeCreator;
        internal Player player;
        internal IMazeSolver solver;
        List<CellsAndWalls> oneTruePath;

        internal Dictionary<string, bool> visitedCells;

        internal int breadCrumbSize = 25;

        internal Maze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> mazeElements)
        {
            //This seems like as good a place as any to create these lists
            mazeElements[MazeElement.ElementType.Cell] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Wall] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.BreadcrumbTrail] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.ShortestPath] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Hint] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Goal] = new List<MazeElement>();
            mazeElements[MazeElement.ElementType.Player] = new List<MazeElement>();

            mazeStorage = new MazeStorage_Dictionary(size, size);          
            // mazeCreator = new DepthFirst_Iterative();        //Comment out Prims and uncomment this to change the maze-generation algorithm!
            mazeCreator = new Prims();
            mazeStorage = mazeStorage.CreateMaze(mazeCreator);
            player = new(mazeStorage);
            solver = new MazeSolver();
        }


        internal void SetupMaze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> maze, MazeGame game)
        {
            //One-time stuff:
            player.ResetPosition();
            player.ResetVisitedCellsDictionary();
            visitedCells = player.GetVisitedCellsDict();

            //Set (1,1) (or w/e the starting point is) to 'true' in the 'visitedCells' dictionary
            (int row, int col) = player.GetPosition();
            visitedCells[$"r{row}c{col}"] = true;

            //Get the shortest path from 1,1 to goal
            solver = solver.Solve(mazeStorage, player, SolverStartingPoint.PlayerStartingPoint);
            List<CellsAndWalls> tmpList =  solver.GetShortestPath();
            oneTruePath = new();
            for(int i = 0; i < tmpList.Count; i++)
            {
                oneTruePath.Add(tmpList[i]);
            }
            
            player.SetPosition(row, col); //right...sovler uses player's position to do its thing.  I should change that.  maybe.
        }//END SetupMaze()

        public void MakeMaze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> maze, MazeGame game)
        {
            //Set up the maze (ie. render it from one dictionary to a dictionary of 6 lists)
            Dictionary<string, CellsAndWalls> dict = mazeStorage.GetDict();
            int player_row;
            int player_col;
            int cellHeight;
            int cellWidth;

            MazeElement el;
            MazeTextElement elTxt;

            switch (size)   //soo...this could be shortened to "cellHeight = cellWidth = (game.windowSize.X / size);," right?  and an (if size = 5 || etc.) then throw?
            {
                case 5:
                    cellHeight = cellWidth = (game.windowSize.X / 5);
                    break;
                case 10:
                    cellHeight = cellWidth = (game.windowSize.X / 10);
                    break;
                case 15:
                    cellHeight = cellWidth = (game.windowSize.X / 15);
                    break;
                case 20:
                    cellHeight = cellWidth = (game.windowSize.X / 20);  //Note: all these numbers need to divide game.windowSize.X (and .Y).
                                                                        //  (1200 seems like a good choice; (20*15) * 4, I think)
                    break;
                default:
                    throw new Exception("Unrecognized maze size; size must be 5, 10, 15 or 20");
            }
            
            int wallThickness = 4;  //needs to be divisble by 2

            //First, we'll add the outer walls
            el = new(game.yellow1x1, CallType.Rectangle, new Rectangle(0, 0, game.windowSize.X, wallThickness / 2), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);
            el = new(game.yellow1x1, CallType.Rectangle, new Rectangle(0, 0, wallThickness / 2, game.windowSize.Y - 100), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);
            el = new(game.yellow1x1, CallType.Rectangle, new Rectangle(0, game.windowSize.Y - 100 - (wallThickness / 2), game.windowSize.X, wallThickness / 2), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);
            el = new(game.yellow1x1, CallType.Rectangle, new Rectangle(game.windowSize.X - (wallThickness / 2), 0, wallThickness / 2, game.windowSize.Y - 100), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);

            //Then, for each cell, we'll evaluate whether there should be walls drawn
            for (int row = 1; row <= size; row++)
                for (int col = 1; col <= size; col++)
                {
                    int cell_x_left = (col - 1) * cellWidth;
                    int cell_x_right = col * cellWidth;
                    int cell_y_top = (row - 1) * cellHeight;
                    int cell_y_bottom = row * cellHeight;

                    string cell_str = $"r{row}c{col}";
                    if (dict[cell_str].wallBelow != null)
                    {
                        //add the wall below the current cell
                        el = new(game.yellow1x1, CallType.Rectangle,
                                 new Rectangle(cell_x_left, cell_y_bottom - wallThickness / 2, cellWidth, wallThickness), Color.White);
                        maze[MazeElement.ElementType.Wall].Add(el);
                    }
                    if (dict[cell_str].wallToTheRight != null)
                    {
                        // add the wall to the right of the current cell
                        el = new(game.yellow1x1, CallType.Rectangle,
                                 new Rectangle(cell_x_right - wallThickness / 2, cell_y_top, wallThickness, cellHeight), Color.White);
                        maze[MazeElement.ElementType.Wall].Add(el);

                    }
                }// inner for

            //Then, if we should draw a BreadcrumbTrail, do so...
            if (game.showBreadcrumbTrail)
            {
                (int rows, int cols) = mazeStorage.GetRowsAndColumns();
                Dictionary<string, bool> hasCellBeenVisited = player.GetVisitedCellsDict();

                for(int row = 1; row <= rows; row++)
                    for(int col = 1; col <= cols; col++)
                    {
                        string str = $"r{row}c{col}";
                        if (hasCellBeenVisited[str])
                        {
                            el = new(game.purple1x1, CallType.Rectangle,
                                     new Rectangle(cellWidth * (col - 1), cellHeight * (row - 1), breadCrumbSize, breadCrumbSize),
                                     Color.White);
                            maze[MazeElement.ElementType.BreadcrumbTrail].Add(el);
                        }
                    }
            }

            //Then, if we should draw the ShortestPath, do so...
            if (game.showShortestPath)                                                                      
            {
                //Let's just have this render the 'one true path' (aka r1c1 to goal)
                for (int i = 0; i < oneTruePath.Count(); i++)
                {
                    int currentRow = oneTruePath[i].cell.row;
                    int currentCol = oneTruePath[i].cell.col;
                    string str = $"r{currentRow}c{currentCol}";
                    el = new(game.yellow1x1, CallType.Rectangle,
                             new Rectangle((currentCol - 1) * cellWidth, (currentRow - 1) * cellHeight, cellWidth, cellHeight),
                             Color.White);
                    maze[MazeElement.ElementType.ShortestPath].Add(el);
                }
            }

            //Then, if we should draw a Hint, do so...
            if (game.showHint)
            {
                //Let's use solver to get the shortest path from the current position to the goal, then show the 0th (1st?) step
                (player_row, player_col) = player.GetPosition();
                solver = solver.Solve(mazeStorage, player, SolverStartingPoint.PlayerPosition);   
                
                List<CellsAndWalls> shortestPath = solver.GetShortestPath();
                int row = shortestPath[1].cell.row;
                int col = shortestPath[1].cell.col;

                el = new(game.red1x1, CallType.Rectangle,
                             new Rectangle((col - 1) * cellWidth, (row - 1) * cellHeight, cellWidth, cellHeight),
                             Color.White);
                maze[MazeElement.ElementType.Hint].Add(el);

                player.SetPosition(player_row, player_col); //and restore the player's position
            }


            //Then we'll draw the goal...
            (int goalRow, int goalCol) = player.GetGoalPosition();
            el = new(game.goal_marker, CallType.Vector2, new Vector2(cellWidth * (goalRow-1), cellHeight * (goalCol-1)), Color.White);
            maze[MazeElement.ElementType.Goal].Add(el);

            //Then, we'll draw the player
            (player_row, player_col) = player.GetPosition();
            el = new(game.player_sprite, CallType.Vector2,
                        new Vector2((player_col - 1) * cellWidth, (player_row - 1) * cellHeight), 
                        Color.White);
            maze[MazeElement.ElementType.Player].Add(el);

            //TODO: Draw the score and the timer (at 0, 1200 through 0, 1300)                                                       //IN PROGRESS
            //elTxt = new(



            //Then we're done!

        }//END MakeMaze()

    }//END class maze
}
