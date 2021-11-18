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
    class HumanEnemy : Enemy

    {
        #region variables
        public float lerpPercentage { get; set; }
        int index = 1;
        double PredictiveDistance = 0;
        public int increment { get; set; }
        Vector2 CurrentStartPosition;
        Player1 player;
        public Vector2 CurrentEndPosition { get; set; }
        List<Vector2> EndPositions;
        Frame previousFrame;
        public float LerpIncrement { get; set; }
        public HumanMovementToPlacesStates HMTPStates = HumanMovementToPlacesStates.FollowingSquarePath;
        public HumanAttackingPlayerStates HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;

        public override GeneralMovementTypes MovementType
        {
            get
            {
                if (Movements == EnemyMovements.IdleDown || Movements == EnemyMovements.IdleRight || Movements == EnemyMovements.IdleUp || Movements == EnemyMovements.IdleLeft)
                {
                    return GeneralMovementTypes.Idle;
                }
                if (Movements == EnemyMovements.MoveDown || Movements == EnemyMovements.MoveLeft || Movements == EnemyMovements.MoveRight || Movements == EnemyMovements.MoveUp)
                {
                    return GeneralMovementTypes.Moving;
                }
                if (Movements == EnemyMovements.SwingDown || Movements == EnemyMovements.SwingUp || Movements == EnemyMovements.SwingLeft || Movements == EnemyMovements.SwingUp)
                {
                    return GeneralMovementTypes.Attacking;
                }
                return GeneralMovementTypes.None;
            }
        }
        public bool wasIntersecting = false;
        Stopwatch watch = new Stopwatch();
        #endregion
        public HumanEnemy(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale, EnemyMovements defaultState, ContentManager Content, List<Vector2> endPositions, float lerpIncrements, float percentage, Player1 Player)
                          : base(tint, position, image, rotation, origin, scale, defaultState, 4, Content)
        {
            Movements = defaultState;

            #region define frames
            List<Frame> UpFrames = new List<Frame>()
            {
                new Frame(9,527,38,46, OriginType.Center),
                new Frame(73,527,38,46, OriginType.Center),
                new Frame(137,527,38,47, OriginType.Center),
                new Frame(201,528,37,47, OriginType.Center),
                new Frame(266,527,37,47, OriginType.Center),
                new Frame(330,527,37,46, OriginType.Center),
                new Frame(394,527,37,47, OriginType.Center),
                new Frame(459,528,36,47, OriginType.Center),
                new Frame(522,527,37,47, OriginType.Center),
            };
            List<Frame> LeftFrames = new List<Frame>()
            {
                new Frame(21,591,30,48, OriginType.Center),
                new Frame(82,592,33,46, OriginType.Center),
                new Frame(148,591,31,46, OriginType.Center),
                new Frame(213,591,31,46, OriginType.Center),
                new Frame(277,591,32,47, OriginType.Center),
                new Frame(340,592,34,46, OriginType.Center),
                new Frame(405,591,33,47, OriginType.Center),
                new Frame(469,591,32,47, OriginType.Center),
                new Frame(533,591,31,47, OriginType.Center),
            };
            List<Frame> DownFrames = new List<Frame>()
            {
            new Frame(10,655,37,49, OriginType.Center),
            new Frame(74,655,37,49, OriginType.Center),
            new Frame(138,655,37,48, OriginType.Center),
            new Frame(202,656,37,47, OriginType.Center),
            new Frame(266,655,37,49, OriginType.Center),
            new Frame(329,655,38,49, OriginType.Center),
            new Frame(394,655,37,48, OriginType.Center),
            new Frame(458,656,36,47, OriginType.Center),
            new Frame(522,655,37,49, OriginType.Center),
            };
            List<Frame> RightFrames = new List<Frame>()
            {

                new Frame(13,719,30,48, OriginType.Center),
                new Frame(77,720,33,46, OriginType.Center),
                new Frame(141,719,31,46, OriginType.Center),
                new Frame(204,719,31,46, OriginType.Center),
                new Frame(267,719,32,47, OriginType.Center),
                new Frame(330,720,34,46, OriginType.Center),
                new Frame(394,719,33,47, OriginType.Center),
                new Frame(459,719,32,47, OriginType.Center),
                new Frame(524,719,31,47, OriginType.Center),
            };
            List<Frame> AttackUpFrames = new List<Frame>()
            {
            new Frame(67,1424,60,46,   new Vector2(29,27),    TimeSpan.FromSeconds(1.25)),
            new Frame(259,1425,54,47,  new Vector2(29,29),    TimeSpan.FromSeconds(1.25)),
            new Frame(457,1424,37,48,  new Vector2(23,25),    TimeSpan.FromSeconds(1.25)),
            new Frame(658,1408,26,64,  new Vector2(14,41),    TimeSpan.FromSeconds(1.25)),
            new Frame(852,1400,25,72,  new Vector2(13,50),    TimeSpan.FromSeconds(1.25)),
            new Frame(1045,1392,24,80, new Vector2(13,59),    TimeSpan.FromSeconds(1.25)),
            new Frame(1236,1400,25,72, new Vector2(14,50),    TimeSpan.FromSeconds(1.25)),
            new Frame(1426,1408,26,64, new Vector2(15,44),    TimeSpan.FromSeconds(1.25)),
            };
            List<Frame> AttackLeftFrames = new List<Frame>()
            {
                new Frame(71,1615,54,48,   new Vector2(26,30),TimeSpan.FromSeconds(1.25)),
                new Frame(247,1615,61,48,  new Vector2(43,30),TimeSpan.FromSeconds(1.25)),
                new Frame(437,1615,65,48,  new Vector2(44,27),TimeSpan.FromSeconds(1.25)),
                new Frame(632,1615,62,49,  new Vector2(42,27),TimeSpan.FromSeconds(1.25)),
                new Frame(816,1615,61,49,  new Vector2(48,28),TimeSpan.FromSeconds(1.25)),
                new Frame(1006,1615,61,49, new Vector2(49,27),TimeSpan.FromSeconds(1.25)),
                new Frame(1202,1615,60,49, new Vector2(46,28),TimeSpan.FromSeconds(1.25)),
                new Frame(1400,1615,62,49, new Vector2(43,27),TimeSpan.FromSeconds(1.25)),
            };
            List<Frame> AttackDownFrames = new List<Frame>()
            {
                new Frame(65,1807,60,49,   new Vector2(31,28),TimeSpan.FromSeconds(1.25)),
                new Frame(263,1807,56,48,  new Vector2(25,26),TimeSpan.FromSeconds(1.25)),
                new Frame(460,1807,39,52,  new Vector2(20,27),TimeSpan.FromSeconds(1.25)),
                new Frame(655,1798,28,60,  new Vector2(17,36),TimeSpan.FromSeconds(1.25)),
                new Frame(850,1806,25,60,  new Vector2(14,31),TimeSpan.FromSeconds(1.25)),
                new Frame(1042,1808,25,66, new Vector2(14,27),TimeSpan.FromSeconds(1.25)),
                new Frame(1234,1806,25,60, new Vector2(14,30),TimeSpan.FromSeconds(1.25)),
                new Frame(1420,1798,31,60, new Vector2(20,36),TimeSpan.FromSeconds(1.25)),
            };
            List<Frame> AttackRightFrames = new List<Frame>()
            {
                new Frame(67,1999,54,48,   new Vector2(29,27),TimeSpan.FromSeconds(1.25)),
                new Frame(268,1999,61,48,  new Vector2(11,27),TimeSpan.FromSeconds(1.25)),
                new Frame(458,1999,65,48,  new Vector2(22,27),TimeSpan.FromSeconds(1.25)),
                new Frame(650,1999,62,49,  new Vector2(22,26),TimeSpan.FromSeconds(1.25)),
                new Frame(851,1999,61,49,  new Vector2(14,25),TimeSpan.FromSeconds(1.25)),
                new Frame(1045,1999,61,49, new Vector2(11,25),TimeSpan.FromSeconds(1.25)),
                new Frame(1234,1999,60,49, new Vector2(15,26),TimeSpan.FromSeconds(1.25)),
                new Frame(1418,1999,62,49, new Vector2(22,26),TimeSpan.FromSeconds(1.25)),
            };

            List<Frame> IdleUpFrames = new List<Frame>
            {
                UpFrames[0]
            };
            List<Frame> IdleDownFrames = new List<Frame>
            {
                DownFrames[0]
            };
            List<Frame> IdleLeftFrames = new List<Frame>
            {
                LeftFrames[0]
            };
            List<Frame> IdleRightFrames = new List<Frame>
            {
                RightFrames[0]
            };
            #endregion
            
            #region dictionaries
            DifferentTimes = new Dictionary<EnemyMovements, TimeSpan>
            {
                {EnemyMovements.MoveUp    ,  TimeSpan.FromMilliseconds(100)},
                {EnemyMovements.MoveDown  ,  TimeSpan.FromMilliseconds(100)},
                {EnemyMovements.MoveLeft  ,  TimeSpan.FromMilliseconds(100)},
                {EnemyMovements.MoveRight , TimeSpan.FromMilliseconds(100)},
                {EnemyMovements.IdleUp    ,    TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.IdleDown  ,  TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.IdleLeft  ,  TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.IdleRight , TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.SwingUp   ,   TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.SwingDown , TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.SwingLeft , TimeSpan.FromMilliseconds(90)},
                {EnemyMovements.SwingRight,TimeSpan.FromMilliseconds(90)}
            };
            DifferentNumberOfFrames = new Dictionary<EnemyMovements, int>
            {
                {EnemyMovements.MoveUp    ,UpFrames.Count},
                {EnemyMovements.MoveDown  ,DownFrames.Count},
                {EnemyMovements.MoveLeft  ,LeftFrames.Count},
                {EnemyMovements.MoveRight ,RightFrames.Count},
                {EnemyMovements.IdleUp    ,IdleUpFrames.Count},
                {EnemyMovements.IdleDown  ,IdleDownFrames.Count},
                {EnemyMovements.IdleLeft  ,IdleLeftFrames.Count},
                {EnemyMovements.IdleRight ,IdleRightFrames.Count},
                {EnemyMovements.SwingUp, AttackUpFrames.Count },
                {EnemyMovements.SwingLeft, AttackLeftFrames.Count},
                {EnemyMovements.SwingDown,AttackDownFrames.Count },
                {EnemyMovements.SwingRight,AttackRightFrames.Count}
            };
            increment = 8;
            AlwaysRunningActions = new Dictionary<EnemyMovements, Action>
            {
                {EnemyMovements.MoveUp    , () => {  Position = new Vector2(Position.X,Position.Y - increment);  } },
                {EnemyMovements.MoveDown  , () => {  Position = new Vector2(Position.X,Position.Y + increment);  } },
                {EnemyMovements.MoveLeft  , () => {  Position = new Vector2(Position.X - increment,Position.Y);  } },
                {EnemyMovements.MoveRight , () => {  Position = new Vector2(Position.X + increment,Position.Y);  }},
                {EnemyMovements.IdleUp    , () => {}},
                {EnemyMovements.IdleDown  , () => {}},
                {EnemyMovements.IdleLeft  , () => {}},
                {EnemyMovements.IdleRight , () => {}},
                {EnemyMovements.SwingUp, () => { } },
                {EnemyMovements.SwingRight, () => { } },
                {EnemyMovements.SwingDown, () => { } },
                {EnemyMovements.SwingLeft, () => { } }
            };
            SometimesRanActions = new Dictionary<EnemyMovements, Action>();
            DifferentTypesOfFrames = new Dictionary<EnemyMovements, List<Frame>>
            {
                {EnemyMovements.MoveUp   ,UpFrames },
                {EnemyMovements.MoveDown ,DownFrames} ,
                {EnemyMovements.MoveLeft ,LeftFrames} ,
                {EnemyMovements.MoveRight,RightFrames} ,
                {EnemyMovements.IdleUp   ,IdleUpFrames} ,
                {EnemyMovements.IdleDown ,IdleDownFrames} ,
                {EnemyMovements.IdleLeft ,IdleLeftFrames} ,
                {EnemyMovements.IdleRight,IdleRightFrames},
                { EnemyMovements.SwingUp,AttackUpFrames},
                { EnemyMovements.SwingLeft,AttackLeftFrames},
                { EnemyMovements.SwingDown,AttackDownFrames},
                { EnemyMovements.SwingRight,AttackRightFrames }
            };
            #endregion
            EndPositions = endPositions;
            LerpIncrement = lerpIncrements;
            lerpPercentage = percentage;
            SetupMovement();
            player = Player;

        }

        public void SetupMovement()
        {
            increment = 0;
            CurrentStartPosition = EndPositions[0];
            CurrentEndPosition = EndPositions[1];
            Movements = EnemyMovements.MoveRight;
        }
        public void MoveEnemyInSquare()
        {
            if (lerpPercentage >= 1f)
            {
                Position = CurrentEndPosition;
                lerpPercentage = 0f;
                CurrentStartPosition = CurrentEndPosition;
                index++;
                if (index >= EndPositions.Count)
                {
                    index = 0;
                }
                CurrentEndPosition = EndPositions[index];
                SetAStraightMovement(CurrentEndPosition);
            }
            Position = Vector2.Lerp(CurrentStartPosition, CurrentEndPosition, lerpPercentage);
            lerpPercentage += LerpIncrement;

        }
        public override void DrawAnimation(SpriteBatch spriteBatch, ContentManager content)
        {
            CurrentFrame = DifferentTypesOfFrames[Movements][CurrentFrameIndex];
            SourceRectangle = CurrentFrame.Rect;
            spriteBatch.Draw(Image, Position, SourceRectangle, Tint, Rotation, CurrentFrame.Origin, Scale, Effects, LayerDepth);
            Texture2D image = content.Load<Texture2D>("Heart");
            if (previousFrame == null) previousFrame = CurrentFrame;
            HeartRectangle = new Rectangle((int)Position.X - (int)CurrentFrame.Origin.X - (int)previousFrame.Origin.X, (int)Position.Y - (int)(CurrentFrame.Origin.Y - (int)previousFrame.Origin.Y), 45, (int)TopLeftConner.Y);

            for (int i = 0; i < NumberOfHearts; i++)
            {
                spriteBatch.Draw(image, new Vector2(HeartRectangle.X + i * image.Width, HeartRectangle.Y), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
            }
            previousFrame = CurrentFrame;
        }

        public void MovePlaces(ref Rectangle boundry, ref Rectangle AttackBoundry)
        {

            #region setupVariables
            int ExpansionSize = 300;
            boundry = new Rectangle((int)player.HitBox.Value.X - ExpansionSize / 2, player.HitBox.Value.Y - ExpansionSize / 2, (int)(player.HitBox.Value.Width + ExpansionSize),
               (int)(player.HitBox.Value.Height + ExpansionSize));
            int AttackSize = 20;
            AttackBoundry = new Rectangle((int)player.HitBox.Value.X - AttackSize / 2, player.HitBox.Value.Y - AttackSize / 2, (int)(player.HitBox.Value.Width + AttackSize), (int)(player.HitBox.Value.Height + AttackSize));
            
            /*
            * if the human does not intersect with the large boundry and the small boundry than it should continue the square movement
            * when the human intersects the large boundry but is not yet at the small boundry, it should start to follow the players position
            * if the human intersects with the small boundry and the player is not moving than start attacking the player dealing damage
            * if the player starts moving again while previously intersecting the human, the human should stop attacking and start following the player until it intersects again
            */
            #endregion
            switch (HMTPStates)
            {
                case HumanMovementToPlacesStates.FigureWhichStateToGoTo:
                 
                    break;

                case HumanMovementToPlacesStates.FollowingSquarePath:

                    MoveEnemyInSquare();
                    if (HitBox.Value.Intersects(boundry) && !HitBox.Value.Intersects(AttackBoundry))
                    {
                        increment = 6;
                        PredictiveDistance = 0;
                        HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                        HMTPStates = HumanMovementToPlacesStates.FollowingPlayersMovements;
                    }
                    break;
                case HumanMovementToPlacesStates.FollowingPlayersMovements:

                    double distanceSquared = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y), 2);
                    switch (HAPStates)
                    {
                        case HumanAttackingPlayerStates.FindPredictiveDistance:

                            if (Movements == EnemyMovements.MoveLeft || Movements == EnemyMovements.MoveRight)
                            {
                                PredictiveDistance = Math.Pow((player.Position.X - Position.X - increment), 2) + Math.Pow((player.Position.Y - Position.Y), 2);
                                HAPStates = HumanAttackingPlayerStates.IsTargetReached;
                            }
                            else if (Movements == EnemyMovements.MoveDown)
                            {
                                PredictiveDistance = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y - increment), 2);
                                HAPStates = HumanAttackingPlayerStates.IsTargetReached;
                            }
                            else if (Movements == EnemyMovements.MoveUp)
                            {
                                PredictiveDistance = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y + increment), 2);
                                HAPStates = HumanAttackingPlayerStates.IsTargetReached;
                            }
                            break;
                        case HumanAttackingPlayerStates.IsTargetReached:
                            if ((PredictiveDistance <= distanceSquared )) //This logic is definitely wrong, need to rethink our situation for switching to idle
                            {
                                watch.Start();
                                HAPStates = HumanAttackingPlayerStates.SetIdle;
                            }
                            else
                            {
                                HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                            }
                            break;
                        case HumanAttackingPlayerStates.SetIdle:
                            if (watch.ElapsedMilliseconds >= 10)
                            {
                                EnemyMovements idle = ReturnIdleMovement();
                                if (idle != EnemyMovements.None)
                                {
                                    CurrentFrameIndex = 0;
                                    Movements = idle;
                                    
                                    HAPStates = HumanAttackingPlayerStates.SetNewMovement;
                                    watch.Restart();
                                }
                            }
                            break;
                        case HumanAttackingPlayerStates.SetNewMovement:
                            if (watch.ElapsedMilliseconds >= 1000)
                            {
                                SetAStraightMovement(new Vector2(player.Position.X, player.Position.Y));
                                wasIntersecting = false;
                                watch.Reset();
                                HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                            }
                            break;
                    }
                    if (!HitBox.Value.Intersects(boundry) && !HitBox.Value.Intersects(AttackBoundry))
                    {
                        increment = 0;
                        SetAStraightMovement(new Vector2(player.Position.X, player.Position.Y));
                        HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                        HMTPStates = HumanMovementToPlacesStates.FollowingSquarePath;
                    }
                    else if(HitBox.Value.Intersects(AttackBoundry) && boundry.Contains(HitBox.Value) && player.MovementType != GeneralMovementTypes.Moving)
                    {
                        HMTPStates = HumanMovementToPlacesStates.AttackingPlayer;
                        HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                    }
                    break;
                case HumanMovementToPlacesStates.AttackingPlayer:
                    EnemyMovements attack = ReturnAttackMovement(2000);
                    if (attack != EnemyMovements.None)
                    {
                        Movements = attack;
                        wasIntersecting = true;
                    }
                    if (HitBox.Value.Intersects(boundry) && !HitBox.Value.Intersects(AttackBoundry))
                    {
                        increment = 6;
                        PredictiveDistance = 0;
                        SetAStraightMovement(new Vector2(player.Position.X, player.Position.Y));
                        HMTPStates = HumanMovementToPlacesStates.FollowingPlayersMovements;
                        HAPStates = HumanAttackingPlayerStates.FindPredictiveDistance;
                    }
                    break;
            }


           
        }
    }
}


//if (HitBox.Value.Intersects(AttackBoundry) && boundry.Contains(HitBox.Value) && player.MovementType != GeneralMovementTypes.Moving)
//{
//    EnemyMovements attack = ReturnAttackMovement(2000);
//    if (attack != EnemyMovements.None)
//    {
//        Movements = attack;
//        wasIntersecting = true;
//        return;
//    }
//}

//else if (HitBox.Value.Intersects(boundry) && !HitBox.Value.Intersects(AttackBoundry))
//{
//    increment = 6;
//    double distanceSquared = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y), 2);
//    double PredictiveDistance = 0;
//    if (Movements == EnemyMovements.MoveLeft || Movements == EnemyMovements.MoveRight)
//    {
//        PredictiveDistance = Math.Pow((player.Position.X - Position.X - increment), 2) + Math.Pow((player.Position.Y - Position.Y), 2);
//    }
//    else if (Movements == EnemyMovements.MoveDown)
//    {
//        PredictiveDistance = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y - increment), 2);
//    }
//    else if (Movements == EnemyMovements.MoveUp)
//    {
//        PredictiveDistance = Math.Pow((player.Position.X - Position.X), 2) + Math.Pow((player.Position.Y - Position.Y + increment), 2);
//    }

//    if (watch.ElapsedMilliseconds >= 1000)
//    {
//        SetAStraightMovement(new Vector2(player.Position.X, player.Position.Y));
//        wasIntersecting = false;
//        watch.Reset();

//    }
//    else if (watch.ElapsedMilliseconds >= 10)
//    {
//        EnemyMovements idle = ReturnIdleMovement();
//        if (idle != EnemyMovements.None)
//        {
//            CurrentFrameIndex = 0;
//            Movements = idle;
//        }
//    }
//    else if ((PredictiveDistance >= distanceSquared && !HitBox.Value.Intersects(AttackBoundry)) || wasIntersecting && player.MovementType == GeneralMovementTypes.Moving)
//    {
//        watch.Start();
//    }

//}
//else if(!HitBox.Value.Intersects(boundry) && !HitBox.Value.Intersects(AttackBoundry))
//{
//    increment = 0;
//    MoveEnemyInSquare();
//}