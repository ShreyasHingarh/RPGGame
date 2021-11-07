using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.IO;

namespace RPG_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    // sanderfrenken.github.io for characters
    //sharklab.io
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public static ScreenManager manager;

        public static string Text;
   
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 600; 
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            manager = new ScreenManager();
            manager.AddScreen(ScreenStates.StartingScreen, new GameScreen(GraphicsDevice, Content,Color.Blue,graphics));
        }



        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();
            manager.CurrentScreen.Update(gameTime,mouse,ks);

            Window.Title = Text;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(manager.CurrentScreen.BackGroundColor);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            manager.CurrentScreen.Draw(spriteBatch);

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
