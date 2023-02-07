namespace mazeGenerator
{
    public interface IMazeSolver
    {
        IMazeSolver Solve(IMazeStorage maze, Player player);

        List<CellsAndWalls> GetShortestPath();
    }
}
