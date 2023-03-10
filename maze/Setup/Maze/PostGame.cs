using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mazeGame.GameElements.Base_classes;
using mazeGame.GameElements.Derived_classes.Maze_stuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using maze;

namespace mazeGame.Setup.Maze
{
    internal class PostGame
    {
        internal void SetUpPostGame(List<MazeTextElement> postGameElements, MazeGame mazeGame)
        {
            MazeTextElement el;
            el = new(RenderType.UI, CallType.Rectangle, mazeGame.black1x1, new Rectangle(100, 200, 700, 600), Color.White);
            postGameElements.Add(el);
            el = new(RenderType.Text, "Congratulations", 
                                     new Vector2(200, 300), Color.White);
            postGameElements.Add(el);

            el = new(RenderType.Text, $"Your score was {mazeGame.maze.player.GetScore()}",
                                     new Vector2(200, 400), Color.White);
            postGameElements.Add(el);
            el = new(RenderType.Text, $"Your time was {mazeGame.maze.elapsedTime.ToString()}",
                                     new Vector2(200, 500), Color.White);
            postGameElements.Add(el);

            el = new(RenderType.Text, "Press Escape to return to menu", 
                     new Vector2(200, 700), Color.White);
            postGameElements.Add(el);
        }
    }
}
