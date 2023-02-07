namespace mazeGenerator
{
    public class MazeStorage_Dictionary : IMazeStorage
    {
        public int rows, cols;
        public Dictionary<string, CellsAndWalls>? maze_dict;

        public MazeStorage_Dictionary(int rows, int cols, Dictionary<string, CellsAndWalls> maze_dict)
        {
            this.rows = rows;
            this.cols = cols;
            this.maze_dict = maze_dict;
        }

        public MazeStorage_Dictionary(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;

            maze_dict = new Dictionary<string, CellsAndWalls>();

            //Create a dictionary-based maze (with all walls, aside from the outside)

            for (int i = 1; i <= rows; i++)
                for (int j = 1; j <= cols; j++)
                {
                    //Create the cell
                    Cell cell = new(i, j);

                    //...and create the walls, as needed
                    Wall? wallBelow = null;
                    Wall? wallToTheRight = null;

                    if (j != cols & i != rows)
                    {
                        //Console.WriteLine($"({i},{j}): i != rows & j != cols");  //TMP
                        //Add both a horizatal and vertical wall
                        wallToTheRight = new Wall(i, j, Wall.WallDirection.vertical);
                        //Console.WriteLine($"Wall (r{i}c{j}, {Wall.WallDirection.vertical}) added to walls");

                        wallBelow = new Wall(i, j, Wall.WallDirection.horizontal);
                        //Console.WriteLine($"Wall (r{i}c{i}, {Wall.WallDirection.horizontal}) added to walls");
                    }
                    else if (j != cols & i == rows)
                    {
                        //Console.WriteLine($"({i},{j}): i == rows & j != cols");  //TMP
                        //Add a vertical wall but no horizontal wall
                        wallToTheRight = new Wall(i, j, Wall.WallDirection.vertical);
                        //Console.WriteLine($"Wall (r{i}c{j}, {Wall.WallDirection.vertical}) added to walls");
                    }
                    else if (j == cols & i != rows)
                    {
                        //Console.WriteLine($"({i},{j}): i != rows & j == cols");  //TMP
                        //Add a horizontal wall but no vertical wall
                        wallBelow = new Wall(i, j, Wall.WallDirection.horizontal);
                        //Console.WriteLine($"Wall (r{i}c{j}, {Wall.WallDirection.horizontal}) added to walls");

                    }
                    else if (j == cols & i == rows)
                    {
                        //Console.WriteLine($"({i},{j}): i == rows & j == cols");  //TMP
                        //Console.WriteLine("(No Walls forthcoming!)");
                        //Don't add either
                    }

                    //Then put it in the dictionary
                    /*string str = $"r{i}c{j}";
                    Console.WriteLine($"Putting {str} in the dictionary.");*/
                    maze_dict[$"r{i}c{j}"] = new CellsAndWalls(cell, wallBelow, wallToTheRight);
                }
        }

        public IMazeStorage CreateMaze(IMazeCreation mazeCreator)
        {
            if (mazeCreator == null)
                throw new Exception("MazeStorage_Dictionary.CreateMaze says: mazeCreator is null");

            return mazeCreator.CreateMaze(this);
        }

        public Dictionary<string, CellsAndWalls> GetDict()
        {
            if(maze_dict == null)
                throw new Exception("MazeStorage_Dictionary.GetDict says: maze_dict is null");

            return maze_dict;
        }

        public (int, int) GetRowsAndColumns()
        {
            return (rows, cols);
        }

        public void Render(IMazeRenderer renderer, IMazeSolver? solver = null)
        {
            if (solver == null)
                renderer.Render(this);
            else
                renderer.Render(this, solver);
        }
    }
}
