using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace mazeGame
{
    internal enum CallType
    {
        //unset = 0,
        Vector2,
        Rectangle
    }

    internal class GameElement
    {
        internal Vector2 coords;
        internal Texture2D texture;
        internal Rectangle rect;
        internal Color color;

        internal CallType callType;
    }
}
