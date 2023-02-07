namespace mazeGenerator
{
    public class Player
    {
        internal class Position
        {
            public int row;
            public int col;

            internal Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }

            internal Position()
            {
                row = 1;
                col = 1;
            }

            internal Position(IMazeStorage maze)
            {
                (row, col) = maze.GetRowsAndColumns();
            }
        }

        /*internal class Goal
        {
            public Position startingPoint;
            public Position endPoint;

            internal Goal(IMazeStorage maze)
            {
                startingPoint = new Position(1, 1);  //assume all mazes start at (1,1)
                endPoint = new Position(maze);
            }
        }*/

        IMazeStorage maze;
        internal /*Goal*/ Position goal;
        internal Position startingPoint;
        internal Position position;

        public Player(IMazeStorage maze) 
        {
            this.maze = maze;
            //goal = new(maze);
            goal = new Position(maze);
            startingPoint = new Position(); //(1,1)
            position = new Position();
        }
    }    
}
