namespace mazeGenerator
{
    public interface IMazeRenderer
    {
        void Render(IMazeStorage maze, IMazeSolver? solver = null);
    }
}
