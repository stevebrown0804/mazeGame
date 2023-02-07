using mazeGame.GameElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mazeGame.Menu
{
    internal class HighScores
    {
        internal void SetupHighScores(List<HighScoresElement> highScoresElements)
        {
            HighScoresElement el = new("High scores:", new Vector2(100, 100), Color.Black);
            highScoresElements.Add(el);
            el = new("TODO", new Vector2(100, 150), Color.Black);
            highScoresElements.Add(el);
        }
    }
}
