using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    class Frame
    {
        public Vector2 Origin;
        public Rectangle Rect;
        public TimeSpan Delay;

        public Frame(int x,int y,int width, int height, Vector2 origin, TimeSpan delay)
        {
            Rect = new Rectangle(x, y, width, height);
            Origin = origin;
            Delay = delay;
        }

        public Frame(int x, int y, int width, int height, Vector2 origin)
            : this(x, y, width, height, origin, TimeSpan.Zero)
        {

        }

        public Frame(int x, int y, int width, int height, OriginType originType)
            : this(x, y, width, height, originType == OriginType.Zero ? Vector2.Zero : new Vector2(width / 2, height / 2), TimeSpan.Zero)
        {

        }


    }
}
