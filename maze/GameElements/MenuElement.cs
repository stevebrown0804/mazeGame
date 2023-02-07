using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mazeGame.GameElements
{
    internal class MenuElement : GameElement
    {
        //internal Vector2 coords;
        /*Texture2D texture;
        Rectangle rect;*/
        //internal Color color;

        //per-element stuff
        internal string text = "";
        //...

        /*enum ElementType
        {
            unset = 0,
            Text,
            Cell,
            Wall,
            Player,
            ShortestPath,
            BreadcrumbTrail,
            Hint
        }
        ElementType eleType;*/

        internal MenuElement(string text, Vector2 vec, Color color)
        {
            this.text = text;
            coords = vec;
            this.color = color;
        }
    }
}
