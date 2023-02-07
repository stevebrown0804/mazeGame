/*Frequently implemented with a stack, this approach is one of the simplest 
 * ways to generate a cells using a computer. Consider the space for a cells 
 * being a large grid of cells (like a large chess board), each cell starting 
 * with four walls. Starting from a random cell, the computer then selects a 
 * random neighbouring cell that has not yet been visited. The computer 
 * removes the wall between the two cells and marks the new cell as visited, 
 * and adds it to the stack to facilitate backtracking. The computer continues 
 * this process, with a cell that has no unvisited neighbours being considered 
 * a dead-end. When at a dead-end it backtracks through the path until it 
 * reaches a cell with an unvisited neighbour, continuing the path generation 
 * by visiting this new, unvisited cell(creating a new junction). This process 
 * continues until every cell has been visited, causing the computer to 
 * backtrack all the way back to the beginning cell. We can be sure every cell 
 * is visited.*/

/*As given above this algorithm involves deep recursion which may cause stack 
 * overflow issues on some computer architectures. The algorithm can be 
 * rearranged into a loop by storing backtracking information in the cells 
 * itself. This also provides a quick way to display a solution, by starting 
 * at any given point and backtracking to the beginning.*/

/*Recursive implementation
The depth-first search algorithm of cells generation is frequently implemented 
using backtracking. This can be described with a following recursive routine:

    Given a current cell as a parameter
    Mark the current cell as visited
    While the current cell has any unvisited neighbour cells
        Choose one of the unvisited neighbours
        Remove the wall between the current cell and the chosen cell
        Invoke the routine recursively for the chosen cell which is invoked 
         once for any initial cell in the area.
*/


/*Iterative implementation
A disadvantage of the first approach is a large depth of recursion – in the 
worst case, the routine may need to recur on every cell of the area being 
processed, which may exceed the maximum recursion stack depth in many 
environments. As a solution, the same backtracking method can be implemented 
with an explicit stack, which is usually allowed to grow much bigger with no 
harm.

    Choose the initial cell, mark it as visited and push it to the stack
    While the stack is not empty
        Pop a cell from the stack and make it a current cell
        If the current cell has any neighbours which have not been visited
            Push the current cell to the stack
            Choose one of the unvisited neighbours
            Remove the wall between the current cell and the chosen cell
            Mark the chosen cell as visited and push it to the stack
*/

namespace mazeGenerator
{
    internal class DepthFirst_Iterative : IMazeCreation
    {

        static bool UnvisitedNeighborsDictIsAllTrue(Dictionary<int, bool> dict)
        {
            bool allTrue = true;
            foreach(var key in dict.Keys)
            {
                if (dict[key] == false)
                    allTrue = false;
            }
            return allTrue;
        }

        public IMazeStorage CreateMaze(IMazeStorage maze)
        {
            //Console.WriteLine("DepthFirst_Iterative.CreateMaze says: In progress");
            int counter = 0; //TMP

            (int rows, int cols) = maze.GetRowsAndColumns();

            Stack<Cell> cells = new();

            //Make a dictionary to track which cells have been "visited"
            Dictionary<string, CellsAndWalls> maze_dict = maze.GetDict();
            var dictKeys = maze_dict.Keys;
            int dictLength = maze_dict.Keys.ToArray().Length;
            Dictionary<string, bool> visited = new();

            //Initialize the 'visited' dictionary to false
            foreach (var k in maze_dict.Keys)
            {
                visited[k] = false;
            }

            //..and commence the maze-creation algorithm:
            /*  "Choose the initial cell, mark it as visited and push it to the stack
                While the stack is not empty
                    Pop a cell from the stack and make it a current cell
                    If the current cell has any neighbours which have not been visited
                        Push the current cell to the stack
                        Choose one of the unvisited neighbours
                        Remove the wall between the current cell and the chosen cell
                        Mark the chosen cell as visited and push it to the stack" */

            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int current_row = rnd.Next(1, rows + 1);
            int current_col = rnd.Next(1, cols + 1);

            string str = $"r{current_row}c{current_col}";
            visited[str] = true;
            cells.Push(maze_dict[str].cell);

            int rnd_neighbor; // = 0;
            while (cells.Count > 0)
            {
                counter++;  //TMP
                rnd_neighbor = rnd.Next(1, 5); //1..5 to get a [1..4] value (I think)
                Cell currentCell = cells.Pop();
                str = $"r{currentCell.row}c{currentCell.col}";
                
                //We'll define "neighbors" as: up, down, left, right
                string str_up = $"r{currentCell.row - 1}c{currentCell.col}";
                string str_down = $"r{currentCell.row + 1}c{currentCell.col}";
                string str_left = $"r{currentCell.row}c{currentCell.col - 1}";
                string str_right = $"r{currentCell.row}c{currentCell.col + 1}";

                //stuff to track the unvisted neighbors, for each interation of the following while loop
                Dictionary<int, bool> unvisitedNeighbors = new() { {1, false},{ 2, false },{ 3, false },{ 4, false } };
                bool neighborVisited = false;

                while (!neighborVisited && !UnvisitedNeighborsDictIsAllTrue(unvisitedNeighbors))
                {
                    //check that at least one of the strings (/keys) exists and has visited==false
                    switch (rnd_neighbor)
                    {
                        case 1:
                            if (visited.ContainsKey(str_up) && visited[str_up] == false)
                            {
                                cells.Push(currentCell);
                                currentCell = maze_dict[str_up].cell;
                                maze_dict[str_up].wallBelow = null;
                                visited[str_up] = true;
                                cells.Push(currentCell);
                                str = str_up;
                                //current_row = currentCell.row;
                                //current_col = currentCell.col;
                                neighborVisited = true;
                            }
                            else
                            {
                                unvisitedNeighbors[1] = true;
                                do
                                    rnd_neighbor = rnd.Next(1, 5);  //new roll
                                while (rnd_neighbor == 1);
                            }
                            break;
                        case 2:
                            if (visited.ContainsKey(str_down) && visited[str_down] == false)
                            {
                                cells.Push(currentCell);
                                maze_dict[str].wallBelow = null;
                                currentCell = maze_dict[str_down].cell;
                                visited[str_down] = true;
                                cells.Push(currentCell);
                                str = str_down;
                                //current_row = currentCell.row;
                                //current_col = currentCell.col;
                                neighborVisited = true;
                            }
                            else
                            {
                                unvisitedNeighbors[2] = true;
                                do
                                    rnd_neighbor = rnd.Next(1, 5);  //new roll
                                while (rnd_neighbor == 2);
                            }
                            break;
                        case 3:
                            if (visited.ContainsKey(str_left) && visited[str_left] == false)
                            {
                                cells.Push(currentCell);
                                maze_dict[str_left].wallToTheRight = null;
                                currentCell = maze_dict[str_left].cell;
                                visited[str_left] = true;
                                cells.Push(currentCell);
                                str = str_left;
                                //current_row = currentCell.row;
                                //current_col = currentCell.col;
                                neighborVisited = true;
                            }
                            else
                            {
                                unvisitedNeighbors[3] = true;
                                do
                                    rnd_neighbor = rnd.Next(1, 5);  //new roll
                                while (rnd_neighbor == 3);
                            }
                            break;
                        case 4:
                            if (visited.ContainsKey(str_right) && visited[str_right] == false)
                            {
                                cells.Push(currentCell);
                                maze_dict[str].wallToTheRight = null;
                                currentCell = maze_dict[str_right].cell;
                                visited[str_right] = true;
                                cells.Push(currentCell);
                                str = str_right;
                                //current_row = currentCell.row;
                                //current_col = currentCell.col;
                                neighborVisited = true;
                            }
                            else
                            {
                                unvisitedNeighbors[4] = true;
                                do
                                    rnd_neighbor = rnd.Next(1, 5);  //new roll
                                while (rnd_neighbor == 4);
                            }
                            break;
                        default:
                            throw new Exception("DepthFirst_Iterative.CreateMaze says: rnd_neighbor wasn't in [1,4]");
                    
                    }//END switch (rnd_neighbor)

                }//END while (unvisitedNeighborCount < 4)

                //TMP
                /*Console.WriteLine($"\nAfter {counter} steps:");
                IMazeRenderer render_dict2 = new RenderDictionaryToConsole();
                maze.Render(render_dict2);*/
                //END TMP

            }//END while (cells.Count > 0)

            //TMP
            /*Console.WriteLine("\nFinal result!");
            Console.WriteLine($"\nAfter {counter} steps:");
            IMazeRenderer render_dict = new RenderDictionaryToConsole();
            maze.Render(render_dict);*/

            /*foreach (var k in visited.Keys)
            {
                Console.WriteLine($"visited[{k}] = {visited[k]}");
            }*/
            //END TMP


            return maze;

        }//END CreateMaze()
    }
}
