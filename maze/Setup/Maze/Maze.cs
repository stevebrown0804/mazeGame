﻿using maze;
using mazeGame.GameElements.Derived_classes;
using mazeGenerator;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.Design;

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
            mazeElements[MazeElement.ElementType.Goal] = new List<MazeElement>();
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
            solver = solver.Solve(mazeStorage, player);       //<---NOTE: temporarily (?) commented out

            //..and then render it
            //IMazeRenderer renderTarget = new RenderDictionaryToConsole();
            //IMazeRenderer renderTarget = new RenderDictionaryToFile();
            //maze.Render(renderTarget, solver);
            //maze.Render(renderTarget);
            //END PASTED IN etc.
        }

        internal void SetupMaze(int size, Dictionary<MazeElement.ElementType, List<MazeElement>> maze, MazeGame game)
        {
            //Set up the maze (ie. render it from one dictionary to a dictionary of 6 lists)
            Dictionary<string, CellsAndWalls> dict = mazeStorage.GetDict();
            MazeElement el;

            //First, we'll add the outer walls                                        //<--on hold, for now
            /*el = new(game.yellow1x1, MazeElement.CallType.Rectangle, new Rectangle(0, 0, 800, 2), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);
            el = new(game.yellow1x1, MazeElement.CallType.Rectangle, new Rectangle(0, 0, 2, 480), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);

            el = new(game.yellow1x1, MazeElement.CallType.Rectangle, new Rectangle(0, 480-2, 800, 2), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);
            el = new(game.yellow1x1, MazeElement.CallType.Rectangle, new Rectangle(800-2, 0, 2, 480), Color.White);
            maze[MazeElement.ElementType.Wall].Add(el);*/

            int cellHeight; // = 60;
            int cellWidth; // = 60;
            switch (size)
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
                    cellHeight = cellWidth = (game.windowSize.X / 20);  //Note: all these numbers need to divide game.windowSize.X (and .Y).  (1200 seems like a good choice; (20*15) * 4, I think)
                    break;
                default:
                    throw new Exception("Unrecognized maze size; size must be 5, 10, 15 or 20");
            }
            int wallThickness = 4;

            //Then, for each cell, we'll evaluate whether there should be walls drawn
            for (int row = 1; row <= size; row++)
                for(int col = 1; col <= size; col++)
                {
                    int cell_x_left = (col - 1) * cellWidth;
                    int cell_x_right = col * cellWidth;
                    int cell_y_top = (row - 1) * cellHeight;
                    int cell_y_bottom = row * cellHeight;

                    string cell_str = $"r{row}c{col}";
                    if (dict[cell_str].wallBelow != null)
                    {
                        //add the wall below the current cell
                        el = new(game.yellow1x1, MazeElement.CallType.Rectangle, 
                                 new Rectangle(cell_x_left, cell_y_bottom - wallThickness / 2, cellWidth, wallThickness), Color.White);
                        maze[MazeElement.ElementType.Wall].Add(el);
                    }
                    if (dict[cell_str].wallToTheRight != null)
                    {
                        // add the wall to the right of the current cell
                        el = new(game.yellow1x1, MazeElement.CallType.Rectangle,
                                 new Rectangle(cell_x_right - wallThickness / 2, cell_y_top, wallThickness, cellHeight), Color.White);
                        maze[MazeElement.ElementType.Wall].Add(el);

                    }

                }// inner for

            //Then, if we should draw BreadcrumbTrail, do so...
            //TODO

            //Then, if we should draw ShortestPath, do so...
            //TODO

            //Then, if we should draw Hint, do so...
            //TODO

            //Then we'll draw the goal...
            //TODO

            //Then, we'll draw the player
            player.ResetPosition();
            (int player_row, int player_col) = player.GetPosition();

            el = new(game.player_sprite, MazeElement.CallType.Vector2,
                                 new Vector2((player_row-1) * cellHeight, (player_col-1) * cellWidth), Color.White);
            maze[MazeElement.ElementType.Player].Add(el);


        }//END SetupMaze()

    }//END class maze
}
