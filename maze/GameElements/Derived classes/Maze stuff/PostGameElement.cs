using mazeGame.GameElements.Base_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using maze;

namespace mazeGame.GameElements.Derived_classes.Maze_stuff
{
    internal class PostGameElement : TextElement
    {
        internal PostGameElement(RenderType renderType, string text, Vector2 vec, Color color)
        {
            if (renderType == RenderType.UI)
                throw new Exception("RenderType.UI needs to be accompanied ty a CallType, [Vector2/Rectangle] and a Color");

            this.text = text;
            coords = vec;
            this.color = color;
            this.renderType = renderType;
            //this.callType = callType;
        }

        internal PostGameElement(RenderType renderType, CallType callType, Texture2D texture, Vector2 vec, Color color)
        {
            if (renderType == RenderType.Text)
                throw new Exception("RenderType.Text needs to be accompanied by a string, Vector2 and color");

            if (callType == CallType.Rectangle)
                throw new Exception("CallType.Rectangle needs to be accompanied by a Rectangle parameter.");

            this.texture = texture;
            this.coords = vec;
            this.color = color;
            this.renderType = renderType;
            this.callType = callType;
        }

        internal PostGameElement(RenderType renderType, CallType callType, Texture2D texture, Rectangle rect, Color color)
        {
            if (renderType == RenderType.Text)
                throw new Exception("RenderType.Text needs to be accompanied by a string, Vector2 and color");

            if (callType == CallType.Vector2)
                throw new Exception("CallType.Vector2 needs to be accompanied by a Vector2 parameter.");

            this.texture = texture;
            this.rect = rect;
            this.color = color;
            this.renderType = renderType;
            this.callType = callType;
        }
    }
}
