using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class Sprite
    {
        public Color Tint { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle? SourceRectangle { get; set; }

        public Texture2D Image { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects Effects { get; set; }
        public float LayerDepth { get; set; }


        public Sprite(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale)
        {
            Tint = tint;
            Position = position;
            Image = image;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;
            Effects = SpriteEffects.None;
            LayerDepth = 0;

            SourceRectangle = null;
        }

        public virtual Rectangle? HitBox
        {
            get
            {
                if(SourceRectangle == null)
                {
                    return new Rectangle((int)(Position.X - Origin.X * Scale.X), (int)(Position.Y - Origin.Y * Scale.Y), (int)(Image.Width * Scale.X), (int)(Scale.Y * Image.Height));
                }
                return new Rectangle((int)(Position.X - Origin.X * Scale.X), (int)(Position.Y - Origin.Y * Scale.Y), (int)(SourceRectangle.Value.Width * Scale.X), (int)(SourceRectangle.Value.Height * Scale.Y));
            }
            set { }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Image == null)
            {
                return;
            }
            spriteBatch.Draw(Image, Position, SourceRectangle, Tint, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}
