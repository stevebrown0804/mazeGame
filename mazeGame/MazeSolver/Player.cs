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

        IMazeStorage maze;
        internal Position goal;
        internal Position startingPoint;
        internal Position position;

        public Player(IMazeStorage maze) 
        {
            this.maze = maze;
            goal = new Position(maze);
            startingPoint = new Position(); //(1,1)
            position = new Position();
        }

        public bool IsAtGoal()
        {
            return goal.row == position.row && goal.col == position.col;
        }
    }    
}
