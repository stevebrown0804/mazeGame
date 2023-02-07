namespace mazeGenerator
{
    public class Prims : IMazeCreation
    {

        /*This algorithm is a randomized version of Prim's algorithm.

        Start with a grid full of walls.
        Pick a cell, mark it as part of the maze.Add the walls of the cell to the wall list.
        While there are walls in the list:
            Pick a random wall from the list.If only one of the cells that the wall divides is visited, then:
                Make the wall a passage and mark the unvisited cell as part of the maze.
                Add the neighboring walls of the cell to the wall list.
            Remove the wall from the list. */

        (bool, string) CheckThatOnlyOneCellThatTheWallDividesHasBeenVisited(Wall wall, Dictionary<string, bool> visited_dict)
        {
            int cellCounter = 0;
            string str_theWall = $"r{wall.row}c{wall.col}";
            string unvisited_cell = "";

            if (visited_dict.ContainsKey(str_theWall))
                if (visited_dict[str_theWall])
                    cellCounter++;
                else
                    unvisited_cell = str_theWall;

            if (wall.direction == Wall.WallDirection.horizontal)   //it's a Below wall, so check the cell below it
            {                
                string str_below = $"r{wall.row + 1}c{wall.col}";
                if (visited_dict.ContainsKey(str_below))
                    if (visited_dict[str_below])
                        cellCounter++;
                    else
                        unvisited_cell = str_below;
            }
            else //wall.directiondir == vertical      //it's a ToTheRight wall, so check the cell to the right
            {
                string str_toTheRight = $"r{wall.row}c{wall.col + 1}";
                if (visited_dict.ContainsKey(str_toTheRight))
                    if (visited_dict[str_toTheRight])
                        cellCounter++;
                    else
                        unvisited_cell = str_toTheRight;
            }

            return (cellCounter == 1, unvisited_cell);
        
        }//END CheckThatOnlyOneCellThatTheWallDividesHasBeenVisited()


        public IMazeStorage CreateMaze(IMazeStorage maze)
        {
            //Console.WriteLine("In progress: Prim's algorithm\n");  //TMP

            (int rows, int cols) = maze.GetRowsAndColumns();
            Dictionary<string, CellsAndWalls> mazeDict = maze.GetDict();
            List<Wall> walls = new();
            Dictionary<string, bool> visited = new();

            //Initialize the 'visited' dictionary to false
            foreach (var k in mazeDict.Keys)
                 visited[k] = false;
 
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int starting_row = rnd.Next(1, rows + 1);
            int starting_col = rnd.Next(1, cols + 1);
            string starting_str = $"r{starting_row}c{starting_col}";

            //
            // "Pick a cell, mark it as part of the maze..."
            //
            visited[starting_str] = true;

            //
            //"... and add the walls of the cell to the wall list."
            //
            if (mazeDict[starting_str].wallBelow != null)
                walls.Add(mazeDict[starting_str].wallBelow);
            
            if (mazeDict[starting_str].wallToTheRight != null)
                walls.Add(mazeDict[starting_str].wallToTheRight);

            //  Add the below wall from the cell above this one (if it exists)
            string str_up = $"r{mazeDict[starting_str].cell.row - 1}c{mazeDict[starting_str].cell.col}";
            if (mazeDict.ContainsKey(str_up))
                if(mazeDict[str_up].wallBelow != null)
                    walls.Add(mazeDict[str_up].wallBelow);

            //  Add the ToTheRight wall from the cell to the left of this one (if it exists)
            string str_left = $"r{mazeDict[starting_str].cell.row}c{mazeDict[starting_str].cell.col - 1}";
            if (mazeDict.ContainsKey(str_left))
                if (mazeDict[str_left].wallToTheRight != null)
                    walls.Add(mazeDict[str_left].wallToTheRight);

            //Then remove duplicates
            walls = walls.Distinct().ToList();      //Necessary?  *shrug*

            //
            // "While there are walls in the list:"
            //
            while (walls.Count > 0)
            {
                 /*for (int i = 0; i < walls.Count; i++)
                {
                    if (walls[i] == null)
                        throw new Exception($"walls[{i}] is null");
                }*/

                //
                // "Pick a random wall from the list."
                //
                int rnd_wall = rnd.Next(0, walls.Count);                

                bool isOnlyOneVisited = false;
                Wall theWall = walls[rnd_wall];
                string str_theWall = $"r{theWall.row}c{theWall.col}";

                if (theWall == null)
                    throw new Exception("Prims.CreateMaze says: theWall is null (just after initialization)");

                string str_unvistedCell = "";
                (isOnlyOneVisited, str_unvistedCell) = CheckThatOnlyOneCellThatTheWallDividesHasBeenVisited(theWall, visited);

                //
                //"If only one of the cells that the wall divides is visited, then:"
                //
                if (isOnlyOneVisited)
                {
                    //
                    //"Make the wall a passage..."
                    // (aka set the wall to null)
                    //
                    if (theWall.direction == Wall.WallDirection.horizontal)
                        mazeDict[str_theWall].wallBelow = null;
                    else if (theWall.direction == Wall.WallDirection.vertical)
                        mazeDict[str_theWall].wallToTheRight = null;
                    else
                        throw new Exception("Prims.CreateMaze says: theWall is in an unrecognized direction");

                    //
                    //"..and mark the unvisited cell as part of the maze."
                    //
                    visited[str_unvistedCell] = true;

                    //
                    //"Add the neighboring walls of the (recently marked as visited) cell to the wall list."
                    //
                    if (mazeDict.ContainsKey(str_unvistedCell))
                    {
                        if (mazeDict[str_unvistedCell].wallBelow != null)
                            walls.Add(mazeDict[str_unvistedCell].wallBelow);

                        if (mazeDict[str_unvistedCell].wallToTheRight != null)
                            walls.Add(mazeDict[str_unvistedCell].wallToTheRight);

                        str_up = $"r{mazeDict[str_unvistedCell].cell.row - 1}c{mazeDict[str_unvistedCell].cell.col}";  //Below

                        if (mazeDict.ContainsKey(str_up))
                            if (mazeDict[str_up].wallBelow != null)
                                walls.Add(mazeDict[str_up].wallBelow);

                        str_left = $"r{mazeDict[str_unvistedCell].cell.row}c{mazeDict[str_unvistedCell].cell.col - 1}"; //ToTheRight

                        if (mazeDict.ContainsKey(str_left))
                            if (mazeDict[str_left].wallToTheRight != null)
                                walls.Add(mazeDict[str_left].wallToTheRight);

                        //Then remove duplicates
                        walls = walls.Distinct().ToList();  //Again, necessary? *shrug*
                    }
                        

                }//END if (isOnlyOneVisited)

                //
                // "Remove the wall (theWall) from the list."
                //
                if (theWall == null)
                    throw new Exception("Prims.CreateMaze says: theWall is null");

                bool removedTheWall = walls.Remove(theWall);
                if (!removedTheWall)
                    throw new Exception("Prims.CreateMaze says: unsuccessfully called Remove() on theWall");
            
            }//END while (walls.Count > 0)

            /*int visitedTrueCounter = 0;
            foreach (var k in visited.Keys)
            {
                if (visited[k] == true)
                    visitedTrueCounter++;
            }
            Console.WriteLine($"Visited.Count == true: {visitedTrueCounter}\n"); */ 

            return new MazeStorage_Dictionary(rows, cols, mazeDict);
        
        }//END CreateMaze()

    }//END class Prims
}
