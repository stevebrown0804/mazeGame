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
        internal int score;

        Dictionary<string, bool> visitedCells;

        public Player(IMazeStorage maze) 
        {
            this.maze = maze;
            goal = new Position(maze);
            startingPoint = new Position(); //(1,1)
            position = new Position(startingPoint.row, startingPoint.col);
            score = 0;                                                                                      //IN PROGRESS

            visitedCells = new();
            ResetVisitedCellsDictionary();  //reset(/initialize)
        }

        public static bool IsMoveAllowed(IMazeStorage mazeStorage, Player player, int targetRow, int targetCol)
        {
            int row = player.position.row;
            int col = player.position.col;
            (int maze_rows, int maze_cols) = mazeStorage.GetRowsAndColumns();

            //first we'll check for out-of-bounds conditions
            if (targetRow == 0 || targetRow == maze_rows + 1 || targetCol == 0 || targetCol == maze_cols + 1)
                return false;

            //Then we'll check for walls
            var dict = mazeStorage.GetDict();
            string str_position = $"r{row}c{col}";
            string str_target = $"r{targetRow}c{targetCol}";
            if (targetRow - row == 1) //targetRow = row + 1, ie 'down'
            {
                if (dict[str_position].wallBelow != null)
                    return false;
            }                
            else if (targetRow - row == -1) //targetRow = row - 1, ie 'up'
            {
                if (dict[str_target].wallBelow != null)
                    return false;
            }
            else if (targetCol - col == 1) //targetCol = col + 1, ie 'right'
            {
                if (dict[str_position].wallToTheRight != null)
                    return false;
            }   
            else if (targetCol - col == -1) //targetCol = col - 1, ie 'left'
            {
                if (dict[str_target].wallToTheRight != null)
                    return false;
            }
            
            //Otherwise...
            return true;
        }//END IsMoveAllowed()

        public bool IsAtGoal()
        {
            return goal.row == position.row && goal.col == position.col;
        }

        public (int, int) GetGoalPosition()
        {
            return (goal.row, goal.col);
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

        public Dictionary<string, bool> GetVisitedCellsDict()
        {
            return visitedCells;
        }

        public void SetCellAsVisited(int row, int col)
        {
            string str = $"r{row}c{col}";
            visitedCells[str] = true;
        }

        public void ResetVisitedCellsDictionary()
        {
            foreach (string k in maze.GetDict().Keys)
            {
                visitedCells[k] = false;
            }
        }

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int score)
        {
            this.score = score;
        }

    } //END class Player 
}
