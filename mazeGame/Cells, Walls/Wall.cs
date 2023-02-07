namespace mazeGenerator
{
    public class Wall
    {
        //RECALL THE DESIGN CHOICE: Walls are to the right of and below cells
        // That is,the horizontal wall at maze[0][0] will be to the right the cell at maze[0][0]
        //  and the vertical wall at maze[0][0] will be below the cell at maze[0][0]

        public enum WallDirection
        {
            //unset,
            horizontal,
            vertical
        }

        public int row, col;
        public WallDirection direction;

        public Wall(int row, int col, WallDirection direction)
        {
            this.row = row; this.col = col;
            this.direction = direction;
        }
    }
}
