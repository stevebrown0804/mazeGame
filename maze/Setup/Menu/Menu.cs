using System;
using mazeGame;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mazeGame.GameElements;

namespace mazeGame.Menu
{
    internal class Menu
    {
        internal void SetupMenu(List<MenuElement> menuElements)
        {
            MenuElement el = new("F1: 5x5 maze", new Vector2(100, 100), Color.Black);
            menuElements.Add(el);
            el = new("F2: 10x10 maze", new Vector2(100, 150), Color.Black);
            menuElements.Add(el);
            el = new("F3: 15x15 maze", new Vector2(100, 200), Color.Black);
            menuElements.Add(el);
            el = new("F4: 20x20 maze", new Vector2(100, 250), Color.Black);
            menuElements.Add(el);
            el = new("F5: Display High Scores", new Vector2(100, 300), Color.Black);
            menuElements.Add(el);
            el = new("F6: Display credits", new Vector2(100, 350), Color.Black);
            menuElements.Add(el);
            el = new("Escape (or Alt-F4): Quit", new Vector2(100, 400), Color.Black);
            menuElements.Add(el);

            //Lines for the in-game keys: ((b)readcrumb trail, (p)ath and (h)int)
            el = new("In-game toggles:", new Vector2(100, 500), Color.Black);
            menuElements.Add(el);
            el = new("b - Breadcrumb trail", new Vector2(100, 550), Color.Black);
            menuElements.Add(el);
            el = new("h - Hint", new Vector2(100, 600), Color.Black);
            menuElements.Add(el);
            el = new("p - Shortest path", new Vector2(100, 650), Color.Black);
            menuElements.Add(el);
        }
    }
}
