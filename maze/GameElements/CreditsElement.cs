using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mazeGame.GameElements
{
    internal class CreditsElement : GameElement
    {
        internal string text = "";

        internal CreditsElement(string text, Vector2 vec, Color color)
        {
            this.text = text;
            coords = vec;
            this.color = color;
        }
    }
}
