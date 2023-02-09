using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mazeGame.GameElements.Base_classes
{
    internal enum RenderType
    {
        unset = 0,
        UI,
        Text
    }

    internal class TextElement : GameElement
    {
        internal string text = "";

        internal RenderType renderType;
    }
}
