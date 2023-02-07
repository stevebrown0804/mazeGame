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
            //Console.WriteLine("In progress: setting up the menu.  Q. Where will this be displayed, if anywhere?");

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
        }
    }
}
