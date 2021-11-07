using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    class Enemy : AnimatedSpriteClass<EnemyMovements>
    {
        Vector2 Target;
        Stopwatch watch = new Stopwatch();
        Stopwatch CoolDownWatch = new Stopwatch();
        Dictionary<EnemyMovements, bool> MovementToBool;
        public int state {get;set;}
        public Enemy(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale, EnemyMovements defaultState, int numOfHearts, ContentManager content)
                          : base(tint, position, image, rotation, origin, scale, defaultState, numOfHearts, content)
        {
            state = 0;
            Target = new Vector2();
            MovementToBool = new Dictionary<EnemyMovements, bool>()
            {
                { EnemyMovements.MoveRight, Position.X  >= Target.X},
                { EnemyMovements.MoveLeft,Position.X <= Target.X},
                { EnemyMovements.MoveDown,Position.Y >= Target.Y },
                { EnemyMovements.MoveUp,Position.Y <= Target.Y}
            };
            CoolDownWatch.Start();
        }
        public EnemyMovements ReturnIdleMovement()
        {
            EnemyMovements currentMovement = EnemyMovements.IdleRight;
            
            if (Movements == EnemyMovements.MoveUp || Movements == EnemyMovements.SwingUp || Movements == EnemyMovements.IdleUp)
            {
                currentMovement = EnemyMovements.IdleUp;
            }
            if (Movements == EnemyMovements.MoveDown || Movements == EnemyMovements.SwingDown || Movements == EnemyMovements.IdleDown)
            {
                currentMovement = EnemyMovements.IdleDown;
            }
            if (Movements == EnemyMovements.MoveLeft || Movements == EnemyMovements.SwingLeft || Movements == EnemyMovements.IdleLeft)
            {
                currentMovement = EnemyMovements.IdleLeft;
            }
            if (Movements == EnemyMovements.MoveRight || Movements == EnemyMovements.SwingRight || Movements == EnemyMovements.IdleRight)
            {
                currentMovement = EnemyMovements.IdleRight;
            }
            
            return currentMovement;
        }
       
        public EnemyMovements ReturnAttackMovement(int cooldown)
        {
            var currentMovement = EnemyMovements.None;

            if (CoolDownWatch.ElapsedMilliseconds >= cooldown && state == 0)
            {
                if (Movements != EnemyMovements.SwingDown && Movements != EnemyMovements.SwingRight && Movements != EnemyMovements.SwingLeft && Movements != EnemyMovements.SwingUp)
                {
                    CurrentFrameIndex = 0;
                    if (Movements == EnemyMovements.MoveUp || Movements == EnemyMovements.IdleUp)
                    {
                        currentMovement = EnemyMovements.SwingUp;
                    }
                    else if (Movements == EnemyMovements.MoveDown || Movements == EnemyMovements.IdleDown)
                    {
                        currentMovement = EnemyMovements.SwingDown;
                    }
                    else if (Movements == EnemyMovements.MoveLeft || Movements == EnemyMovements.IdleLeft)
                    {
                        currentMovement = EnemyMovements.SwingLeft;
                    }
                    else if (Movements == EnemyMovements.MoveRight || Movements == EnemyMovements.IdleRight)
                    {
                        currentMovement = EnemyMovements.SwingRight;
                    }
                    
                }
                state = 1;
                CoolDownWatch.Reset();
            }
            if ((Movements != EnemyMovements.IdleDown && Movements != EnemyMovements.IdleRight && Movements != EnemyMovements.IdleLeft && Movements != EnemyMovements.IdleUp) && state == 1)
            {
                watch.Restart();
                CoolDownWatch.Reset();
                state = 2;
            }
            if (watch.ElapsedMilliseconds >= (int)(DifferentTimes[Movements].Milliseconds * DifferentNumberOfFrames[Movements]) + 15 && CoolDownWatch.ElapsedMilliseconds == 0 && state == 2)// make 15 not a magic number
            {
                CurrentFrameIndex = 0;
                currentMovement = ReturnIdleMovement();
                CoolDownWatch.Restart();
                state = 0;
            }
            
           
            return currentMovement;
        }
        public void SetAStraightMovement(Vector2 target)
        {
            bool IsIdle = false;
            float max = float.MinValue;
            
            Target = target;
            if ((int)Position.X + 1 <= target.X)
            {
                if (max <= target.X - (Position.X + 1))
                {
                    max = target.X - (Position.X + 1);
                    Movements = EnemyMovements.MoveRight;
                }

                IsIdle = true;
            }
            if ((int)Position.X - 1 >= target.X)
            {
                if (max <= (Position.X - 1) - target.X)
                {
                    max = (Position.X - 1) - target.X;
                    Movements = EnemyMovements.MoveLeft;
                }
                IsIdle = true;
            }
            if ((int)Position.Y + 1 <= target.Y)
            {
                if (max <= target.Y - (Position.Y + 1))
                {
                    max = target.Y - (Position.Y + 1);
                    Movements = EnemyMovements.MoveDown;
                }
                IsIdle = true;
            }
            if ((int)Position.Y - 1 >= target.Y)
            {
                if (max <= (Position.Y - 1) - target.Y)
                {
                    max = (Position.Y - 1) - target.Y;
                    Movements = EnemyMovements.MoveUp;
                }
                IsIdle = true;
            }
            if (!IsIdle)
            {
                CurrentFrameIndex = 0;
                Movements = EnemyMovements.IdleRight;
            }
           
        }
        
    }

}
