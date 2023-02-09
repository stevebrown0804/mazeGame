namespace mazeGenerator
{
    public enum SolverStartingPoint
    {
        PlayerPosition,
        PlayerStartingPoint,
        //r1c1
    }

    public interface IMazeSolver
    {
        IMazeSolver Solve(IMazeStorage maze, Player player, SolverStartingPoint startingPoint);

        List<CellsAndWalls> GetShortestPath();
    }
}
