/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using mazeGame.GameElements.Base_classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mazeGame.GameElements
{
    internal class HighScoresElement : TextElement
    {
        //internal string text = "";

        internal HighScoresElement(string text, Vector2 vec, Color color)
        {
            this.text = text;
            coords = vec;
            this.color = color;
        }
        //what else?  TBD
    }
}
