using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            Hint,
            Goal
            //TextPanel
        }

         //Cornstructors
        internal MazeElement(Texture2D t, CallType callType, Rectangle r, Color c)
        {
            texture = t;
            //coords = null;
            rect = r;
            color = c;
            this.callType = callType;
        }

        internal MazeElement(Texture2D t, CallType callType, Vector2 v, Color c)
        {
            texture = t;
            coords = v;
            //rect = null;
            color = c;
            this.callType = callType;
        }
    }
}
