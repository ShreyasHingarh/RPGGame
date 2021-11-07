using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    public abstract class Screen
    {
        public GraphicsDevice Graphics { get; set; }
        public ContentManager Content { get; set; }
        public Color BackGroundColor { get; set; }
        public GraphicsDeviceManager GraphicsManager { get; set; }
        public Screen(GraphicsDevice graphics, ContentManager content, Color color,GraphicsDeviceManager Graficos)
        {
            Graphics = graphics;
            Content = content;
            GraphicsManager = Graficos;
            BackGroundColor = color;
        }

        public abstract void Update(GameTime gameTime,MouseState mouse, KeyboardState ks);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
