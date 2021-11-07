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
    class Portal : Sprite
    {
        string access = "";
        public int Level;
        SpriteFont Font;
        bool canIncreaseTime = false;

        TimeSpan elaspsedTime = TimeSpan.Zero;
        TimeSpan TimeWanted = TimeSpan.FromSeconds(5);
        public Portal(Vector2 Position, Vector2 scale,Color tint, float rotation, Vector2 origin, Texture2D portalImage,Rectangle sourceRectangle, int level, SpriteFont font)
            : base(tint,Position, portalImage,rotation,origin,scale)
        {
            SourceRectangle = sourceRectangle;
            Level = level;
            Font = font;
            
        }

        public void Update(Player1 player, KeyboardState ks,GameTime gameTime,ref RepeatRotation repeatRotation)
        {
            
            if (HitBox.Value.Intersects(player.HitBox.Value) && ks.IsKeyDown(Keys.Enter))
            {
                if(Level > player.level)
                {
                    access = "Cannot Enter, Complete Previous Levels";
                    canIncreaseTime = true;
                }
                else if(Level == player.level)
                {
                    access = "Access Good";
                    player.level++;
                    repeatRotation = RepeatRotation.LoopRotation;
                    Level++;
                    canIncreaseTime = true;
                }
                else if(Level < player.level)
                {
                    access = "Already Completed, Go to the next levels";
                    canIncreaseTime = true;
                }
                
                
            }
            if (canIncreaseTime)
            {
                elaspsedTime += gameTime.ElapsedGameTime;
            }
            if (elaspsedTime >= TimeWanted)
            {
                access = "";
                canIncreaseTime = false;
                elaspsedTime = TimeSpan.Zero;
            }
            

            //if it intersects
            // then if yes then check the current level and display either cannot enter yet or completed already,
            // if the player goes to the correct portal then rn display your in
        }
        public void DrawPortals(SpriteBatch spriteBatch,SpriteFont font,GraphicsDevice graphicsDevice,Texture2D pixel)
        {
            spriteBatch.Draw(Image, new Vector2(Position.X ,Position.Y ), SourceRectangle, Tint, Rotation, Origin, Scale, Effects, LayerDepth);
            
            if(access != "")
            {
                spriteBatch.Draw(pixel, new Vector2(0,0), null, Color.White, 0, Vector2.Zero,  font.MeasureString(access), SpriteEffects.None, 0);
            }
            
            spriteBatch.DrawString(Font, access, new Vector2(0,0), Color.Black); 
        }
    }
}
