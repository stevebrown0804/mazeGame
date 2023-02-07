using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace mazeGame
{
    internal class GameElement
    {
        internal Vector2 coords;
        internal Texture2D texture;
        internal Rectangle rect;
        internal Color color;

        //per-element stuff
        //string text = "";
        //...

        /*enum ElementType        //NOTE: not in use atm; using separate lists instead
        {
            unset = 0,
            Text,
            Cell,
            Wall,
            Player,
            ShortestPath,
            BreadcrumbTrail,
            Hint
        }*/
        //internal ElementType eleType;
    }
}
