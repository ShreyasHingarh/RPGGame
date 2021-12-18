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
        public Stopwatch AttackingWatch = new Stopwatch();
        public Stopwatch CoolDownWatch = new Stopwatch();
        public bool didPlayerGetHitByEnemy = false;
        
        public StatesForFindingAttackMovement states { get; set; }
        public Enemy(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale, EnemyMovements defaultState, int numOfHearts, ContentManager content)
                          : base(tint, position, image, rotation, origin, scale, defaultState, numOfHearts, content)
        {
            states = StatesForFindingAttackMovement.SwitchToAttack;
            Target = new Vector2();
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

            CurrentFrameIndex = 0;
            return currentMovement;
        }
       
        public EnemyMovements ReturnAttackMovement(int cooldown)
        {
            var currentMovement = EnemyMovements.None;

            if (CoolDownWatch.ElapsedMilliseconds >= cooldown && states == StatesForFindingAttackMovement.SwitchToAttack)
            {
                if (MovementType != GeneralMovementTypes.Attacking)
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
                states = StatesForFindingAttackMovement.StartTimers;
                CoolDownWatch.Restart();
            }
            else if(states == StatesForFindingAttackMovement.SwitchToAttack)
            {
                currentMovement = ReturnIdleMovement();
            }
            if ((MovementType != GeneralMovementTypes.Idle) && states == StatesForFindingAttackMovement.StartTimers)
            {
                AttackingWatch.Restart();
                CoolDownWatch.Reset();
                states = StatesForFindingAttackMovement.SwitchToIdle;
                
            }
            if (AttackingWatch.ElapsedMilliseconds >= (int)(DifferentTimes[Movements].Milliseconds * DifferentNumberOfFrames[Movements]) + 15 && CoolDownWatch.ElapsedMilliseconds == 0 && states == StatesForFindingAttackMovement.SwitchToIdle)// make 15 not a magic number
            {
                CurrentFrameIndex = 0;
                didPlayerGetHitByEnemy = false;
                currentMovement = ReturnIdleMovement();
                CoolDownWatch.Restart();
                states = StatesForFindingAttackMovement.SwitchToAttack;
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
