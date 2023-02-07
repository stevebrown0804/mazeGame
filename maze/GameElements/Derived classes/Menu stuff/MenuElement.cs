using mazeGame.GameElements.Base_classes;
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
    internal class MenuElement : TextElement //GameElement
    {
        //per-element stuff
        //internal string text = "";
        //...

        internal MenuElement(string text, Vector2 vec, Color color) {
            this.text = text;
            coords = vec;
            this.color = color;
        }
    }
}
