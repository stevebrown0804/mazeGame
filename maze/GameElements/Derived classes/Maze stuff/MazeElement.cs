using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mazeGame.GameElements.Derived_classes
{
    internal class MazeElement : GameElement
    {
        //Reminder:
        /*internal Vector2 coords;
        internal Texture2D texture;
        internal Rectangle rect;
        internal Color color;*/

        internal enum ElementType
        {
            unset = 0,
            //Text,
            Cell,
            Wall,
            Player,
            ShortestPath,
            BreadcrumbTrail,
            Hint
        }
    }
}
