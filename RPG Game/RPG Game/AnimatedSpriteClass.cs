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

    enum GeneralMovementTypes
    {
        Moving,
        Idle,
        Attacking,
        None
    }

    class AnimatedSpriteClass<T> : Sprite
        where T : Enum
    {

        public T Movements { get; set; }
        public Dictionary<T, Action> SometimesRanActions { get; set; }
        public Dictionary<T, TimeSpan> DifferentTimes { get; set; }
        public Dictionary<T, int> DifferentNumberOfFrames { get; set; }
        public Dictionary<T, Action> AlwaysRunningActions { get; set; }
        public Dictionary<T, List<Frame>> DifferentTypesOfFrames { get; set; }

        public virtual GeneralMovementTypes MovementType { get; set; }
        
        TimeSpan ElaspedTime = TimeSpan.Zero;
        public int CurrentFrameIndex{ get; set;}
        
        public Frame CurrentFrame { get; set; }
        public int NumberOfHearts { get; set; }
        public Rectangle HeartRectangle { get; set; }
        public  Vector2 TopLeftConner 
        {
            get
            {
                return new Vector2(Position.X - Origin.X, Position.Y - Origin.Y);
            }
        }

     
        public AnimatedSpriteClass(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale, T defaultState,int numOfHearts, ContentManager content)
                                      : base(tint, position, image, rotation, origin, scale)
        {
           
            NumberOfHearts = numOfHearts;
            Movements = defaultState;
            CurrentFrameIndex = 0;
            
        }

      
        public void Animate(GameTime gameTime)
        {

            ElaspedTime += gameTime.ElapsedGameTime;
            if (ElaspedTime >= DifferentTimes[Movements])
            {
                Origin = DifferentTypesOfFrames[Movements][CurrentFrameIndex].Origin;
                CurrentFrameIndex++;

                AlwaysRunningActions[Movements]();
                
                if (CurrentFrameIndex >= DifferentNumberOfFrames[Movements])
                {
                    if (SometimesRanActions.Count != 0 )
                    {
                        SometimesRanActions[Movements]();
                    }
                    CurrentFrameIndex = 0;

                }
                ElaspedTime = TimeSpan.Zero;
            }
        }
        
        public virtual void DrawAnimation(SpriteBatch spriteBatch,ContentManager content)
        {
            CurrentFrame = DifferentTypesOfFrames[Movements][CurrentFrameIndex];
            SourceRectangle = CurrentFrame.Rect;
            spriteBatch.Draw(Image, Position, SourceRectangle, Tint, Rotation, CurrentFrame.Origin, Scale, Effects, LayerDepth);
           
        }

    }
}
