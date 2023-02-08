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
                (this.row, this.col) = maze.GetRowsAndColumns();                
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
            position = new Position(startingPoint.row, startingPoint.col);
        }

        public bool IsAtGoal()
        {
            return goal.row == position.row && goal.col == position.col;
        }

        public (int, int) GetPosition()
        {
            return (position.row, position.col);
        }

        public void SetPosition(int row, int col)
        {
            position.row = row;
            position.col = col;
        }

        public void ResetPosition()
        {
            position.row = startingPoint.row;
            position.col = startingPoint.col;
        }
    }    
}
