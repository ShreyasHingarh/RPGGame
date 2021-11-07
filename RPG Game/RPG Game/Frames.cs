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
    class Frames
    {
        public Vector2 Origin;
        public Rectangle Frame;

        public Frames(int x,int y,int width, int height, Vector2 origin)
        {
            Frame = new Rectangle(x, y, width, height);
            Origin = origin;
        }
    }
}
