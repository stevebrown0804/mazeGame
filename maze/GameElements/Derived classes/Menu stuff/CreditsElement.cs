using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mazeGame.GameElements.Base_classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mazeGame.GameElements
{
    internal class CreditsElement : TextElement //GameElement
    {
        //internal string text = "";

        internal CreditsElement(string text, Vector2 vec, Color color)
        {
            this.text = text;
            coords = vec;
            this.color = color;
        }
    }
}
