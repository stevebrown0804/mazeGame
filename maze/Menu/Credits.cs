﻿using mazeGame.GameElements;
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
    internal class Credits
    {
        internal void SetupCredits(List<CreditsElement> creditsElements)
        {
            CreditsElement el = new("Game by Steve Brown.", new Vector2(100, 100), Color.Black);
            creditsElements.Add(el);
            el = new("(Press Escape to return to menu)", new Vector2(100, 150), Color.Black);
            creditsElements.Add(el);
        }
    }
}
