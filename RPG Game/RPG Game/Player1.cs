using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace RPG_Game
{
    
    class Player1 : AnimatedSpriteClass<MovementsForPlayer>
    {
        Texture2D HeartImage;
        public override GeneralMovementTypes MovementType
        {
            get
            {
                if (Movements == MovementsForPlayer.IdleDown || Movements == MovementsForPlayer.IdleRight || Movements == MovementsForPlayer.IdleUp || Movements == MovementsForPlayer.IdleLeft)
                {
                    return GeneralMovementTypes.Idle;
                }
                if (Movements == MovementsForPlayer.MoveDown || Movements == MovementsForPlayer.MoveLeft || Movements == MovementsForPlayer.MoveRight || Movements == MovementsForPlayer.MoveUp)
                {
                    return GeneralMovementTypes.Moving;
                }
                if (Movements == MovementsForPlayer.SwingDown || Movements == MovementsForPlayer.SwingUp || Movements == MovementsForPlayer.SwingLeft || Movements == MovementsForPlayer.SwingUp)
                {
                    return GeneralMovementTypes.Attacking;
                }
                return GeneralMovementTypes.None;
            }
        }
        Keys lastKey = Keys.None;
        public bool isIntersecting { get; set; } 
        public int level = 0;

        
        float lerpPercentage = 0f;
        float lerpIncrement = 0.01f;
        MovementsForPlayer DefaultState;
        
        public Dictionary<Keys, MovementsForPlayer> 
            DifferentKeys { get; set; }
        public Dictionary<Keys, Action> ActionsForEachKey { get; set; }
        public Dictionary<MovementsForPlayer, MovementsForPlayer> OppositeDirections { get; set; }
       
        Vector2 initialLerpScale;

        public Rectangle HouseHitBox;

        Stopwatch watch = new Stopwatch();
        public Player1(Color tint, Vector2 position, Texture2D image, float rotation, Vector2 origin, Vector2 scale, MovementsForPlayer defaultState,ContentManager content)
                           : base(tint, position, image, rotation, origin, scale,defaultState,15,content)
        {
          
            HeartImage = content.Load<Texture2D>("Heart");
            isIntersecting = false;
            #region define frames
            List<Frame> RightFrames = new List<Frame>()
            {
                new Frame(14,717,29,49,  OriginType.Center),
                new Frame(78,718,31,48,  OriginType.Center),
                new Frame(142,717,29,48, OriginType.Center),
                new Frame(205,717,30,48, OriginType.Center),
                new Frame(268,717,31,49, OriginType.Center),
                new Frame(331,717,33,48, OriginType.Center),
                new Frame(395,716,32,49, OriginType.Center),
                new Frame(460,716,31,49, OriginType.Center),
                new Frame(525,716,30,49, OriginType.Center),
            };
            List<Frame> LeftFrames = new List<Frame>()
            {
                new Frame(21,589,29,49,  OriginType.Center),
                new Frame(83,590,31,48,  OriginType.Center),
                new Frame(149,589,29,48, OriginType.Center),
                new Frame(213,589,30,48, OriginType.Center),
                new Frame(277,589,31,49, OriginType.Center),
                new Frame(340,590,33,48, OriginType.Center),
                new Frame(405,589,32,49, OriginType.Center),
                new Frame(469,589,31,49, OriginType.Center),
                new Frame(533,589,30,49, OriginType.Center),
            };
            List<Frame> UpFrames = new List<Frame>()
            {
                new Frame(10,525,37,48,  OriginType.Center),
                new Frame(74,525,37,48,  OriginType.Center),
                new Frame(138,525,37,49, OriginType.Center),
                new Frame(202,526,37,49, OriginType.Center),
                new Frame(267,525,37,49, OriginType.Center),
                new Frame(331,525,37,48, OriginType.Center),
                new Frame(395,525,37,49, OriginType.Center),
                new Frame(460,526,37,49, OriginType.Center),
                new Frame(523,525,37,50, OriginType.Center),
            };
            List<Frame> DownFrames = new List<Frame>()
            {
                new Frame(17,652,35,50,  OriginType.Center),
                new Frame(81,652,35,50,  OriginType.Center),
                new Frame(145,652,35,51, OriginType.Center),
                new Frame(210,653,35,50, OriginType.Center),
                new Frame(273,652,35,51, OriginType.Center),
                new Frame(337,652,35,50, OriginType.Center),
                new Frame(401,652,35,51, OriginType.Center),
                new Frame(465,653,34,50, OriginType.Center),
                new Frame(529,652,35,51, OriginType.Center),
            };

            List<Frame> IdleRightFrames = new List<Frame>()
            {
                RightFrames[0]
            };
            List<Frame> IdleLeftFrames = new List<Frame>()
            {
                LeftFrames[0]
            };
            List<Frame> IdleUpFrames = new List<Frame>()
            {
               UpFrames[0]
            };
            List<Frame> IdleDownFrames = new List<Frame>()
            {
               DownFrames[0]
            };
           
            List<Frame> AttackUpFrames = new List<Frame>()
            {

                new Frame(72, 1422, 39, 48,   new Vector2(24,27),TimeSpan.FromSeconds(.75)),
                new Frame(255, 1423, 47, 48,  new Vector2(33,28),TimeSpan.FromSeconds(.75)),
                new Frame(448, 1423, 45, 48,  new Vector2(31,27),TimeSpan.FromSeconds(.75)),
                new Frame(629, 1422, 58, 49,  new Vector2(44,27),TimeSpan.FromSeconds(.75)),
                new Frame(823, 1401, 85, 71,  new Vector2(46,45),TimeSpan.FromSeconds(.75)),
                new Frame(1042, 1404, 78, 70, new Vector2(15,42),TimeSpan.FromSeconds(.75))
            };                                                                        
            List<Frame> AttackDownFrames = new List<Frame>                            
            {                                                                         
                new Frame(72,1804,39,53,   new Vector2(24,29),   TimeSpan.FromSeconds(.75)),
                new Frame(255,1804,47,50,  new Vector2(33,29),   TimeSpan.FromSeconds(.75)),
                new Frame(448,1806,45,48,  new Vector2(31,28),   TimeSpan.FromSeconds(.75)),
                new Frame(632,1805,55,49,  new Vector2(42,29),   TimeSpan.FromSeconds(.75)),
                new Frame(823,1804,79,69,  new Vector2(44,29),   TimeSpan.FromSeconds(.75)),
                new Frame(1043,1804,76,66, new Vector2(16,29),   TimeSpan.FromSeconds(.75))
            };                                                                        
            List<Frame> AttackLeftFrames = new List<Frame>                            
            {                                                                         
                new Frame(52,1613,55,51,new Vector2  (44,28),    TimeSpan.FromSeconds(.75)),
                new Frame(267,1615,31,47,new Vector2 (19,28),    TimeSpan.FromSeconds(.75)),
                new Frame(466,1613,36,48, new Vector2(11,28),    TimeSpan.FromSeconds(.75)),
                new Frame(646,1613,57,49, new Vector2(23,28),    TimeSpan.FromSeconds(.75)),
                new Frame(783,1613,93,49,new Vector2 (77,28),    TimeSpan.FromSeconds(.75)),
                new Frame(978,1613,90,49,new Vector2 (75,28),    TimeSpan.FromSeconds(.75)),
            };                                                                        
            List<Frame> AttackRightFrames = new List<Frame>                           
            {                                                                         
                new Frame(85,1997,55,51  ,new Vector2(11,28),    TimeSpan.FromSeconds(.75)),
                new Frame(279,1999,31,47 ,new Vector2(12,28),    TimeSpan.FromSeconds(.75)),
                new Frame(458,1997,36,49, new Vector2(24,28),    TimeSpan.FromSeconds(.75)),
                new Frame(641,1997,57,49, new Vector2(34,28),    TimeSpan.FromSeconds(.75)),
                new Frame(852,1997,93,48, new Vector2(16,28),    TimeSpan.FromSeconds(.75)),
                new Frame(1044,1997,89,49,new Vector2(16,28),    TimeSpan.FromSeconds(.75)),
            };
            #endregion
            #region dictionaries
            DifferentTimes = new Dictionary<MovementsForPlayer, TimeSpan>()
            {
                { MovementsForPlayer.MoveUp, TimeSpan.FromMilliseconds(100) },
                { MovementsForPlayer.MoveDown, TimeSpan.FromMilliseconds(100) },
                { MovementsForPlayer.MoveLeft, TimeSpan.FromMilliseconds(100) },
                { MovementsForPlayer.MoveRight, TimeSpan.FromMilliseconds(100) },
                { MovementsForPlayer.IdleRight, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.IdleLeft, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.IdleDown, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.IdleUp, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.SwingUp, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.SwingDown, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.SwingLeft, TimeSpan.FromMilliseconds(90) },
                { MovementsForPlayer.SwingRight, TimeSpan.FromMilliseconds(90) },
            };
            DifferentNumberOfFrames = new Dictionary<MovementsForPlayer, int>()
            {
                { MovementsForPlayer.MoveUp, UpFrames.Count},
                { MovementsForPlayer.MoveDown, DownFrames.Count},
                { MovementsForPlayer.MoveLeft, LeftFrames.Count},
                { MovementsForPlayer.MoveRight, RightFrames.Count},
                { MovementsForPlayer.IdleRight, IdleRightFrames.Count},
                { MovementsForPlayer.IdleLeft, IdleLeftFrames.Count},
                { MovementsForPlayer.IdleUp, IdleUpFrames.Count},
                { MovementsForPlayer.IdleDown, IdleDownFrames.Count},
                { MovementsForPlayer.SwingUp, AttackUpFrames.Count},
                { MovementsForPlayer.SwingDown, AttackDownFrames.Count},
                { MovementsForPlayer.SwingLeft, AttackLeftFrames.Count},
                { MovementsForPlayer.SwingRight, AttackRightFrames.Count},
            };
            int increment = 8;
            AlwaysRunningActions = new Dictionary<MovementsForPlayer, Action>
            {
                {MovementsForPlayer.MoveUp, () =>{Position = new Vector2    (Position.X,Position.Y - increment);} },
                {MovementsForPlayer.MoveDown, () => { Position = new Vector2(Position.X,Position.Y + increment);} },
                {MovementsForPlayer.MoveLeft, () =>{Position = new Vector2  (Position.X - increment,Position.Y);} },
                {MovementsForPlayer.MoveRight, () => {Position = new Vector2 (Position.X + increment,Position.Y);} },
                {MovementsForPlayer.IdleRight, () => { } },
                {MovementsForPlayer.IdleLeft, () => { } },
                {MovementsForPlayer.IdleDown, () => { } },
                {MovementsForPlayer.IdleUp, () => { } },
                { MovementsForPlayer.SwingUp, () => { } },
                { MovementsForPlayer.SwingDown, () => { } },
                { MovementsForPlayer.SwingLeft, () => { } },
                { MovementsForPlayer.SwingRight, () => { } },
            };
            DifferentTypesOfFrames = new Dictionary<MovementsForPlayer, List<Frame>>()
            {
                {MovementsForPlayer.MoveUp, UpFrames },
                {MovementsForPlayer.MoveDown, DownFrames },
                {MovementsForPlayer.MoveLeft, LeftFrames },
                {MovementsForPlayer.MoveRight, RightFrames },
                {MovementsForPlayer.IdleRight, IdleRightFrames},
                {MovementsForPlayer.IdleLeft, IdleLeftFrames},
                {MovementsForPlayer.IdleDown, IdleDownFrames},
                {MovementsForPlayer.IdleUp, IdleUpFrames},
                { MovementsForPlayer.SwingUp, AttackUpFrames },
                { MovementsForPlayer.SwingDown, AttackDownFrames },
                { MovementsForPlayer.SwingLeft, AttackLeftFrames },
                { MovementsForPlayer.SwingRight, AttackRightFrames },
            };
            DifferentKeys = new Dictionary<Keys, MovementsForPlayer>()
            {
                { Keys.W, MovementsForPlayer.MoveUp},
                { Keys.S, MovementsForPlayer.MoveDown},
                { Keys.A, MovementsForPlayer.MoveLeft},
                { Keys.D, MovementsForPlayer.MoveRight},
            };
            ActionsForEachKey = new Dictionary<Keys, Action>()
            {
                {
                    Keys.W, () => 
                    {
                        Effects = SpriteEffects.None;
                        HouseHitBox = new Rectangle((int)(Position.X - CurrentFrame.Origin.X * scale.X),(int)(Position.Y - CurrentFrame.Origin.Y) + SourceRectangle.Value.Height/2,SourceRectangle.Value.Width, increment);
                    } 
                },
                {
                    Keys.S, () => 
                    {   
                        Effects = SpriteEffects.None;

                        HouseHitBox = new Rectangle((int)(Position.X - CurrentFrame.Origin.X * scale.X), (int)((Position.Y - CurrentFrame.Origin.Y - increment) + SourceRectangle.Value.Height),SourceRectangle.Value.Width,increment);
                    } 
                },
                {
                    Keys.A, () => 
                    {
                        Effects = SpriteEffects.None;
                        HouseHitBox = new Rectangle((int)(Position.X  - ((CurrentFrame.Origin.X * scale.X) )),(int)Position.Y,increment,SourceRectangle.Value.Height /2);
                    } 
                },
                {
                    Keys.D, () => 
                    {
                        Effects = SpriteEffects.None;
                        HouseHitBox = new Rectangle((int)(Position.X - (CurrentFrame.Origin.X * scale.X) + SourceRectangle.Value.Width - increment),(int)Position.Y,increment,SourceRectangle.Value.Height/2);
                    } 
                },
            };
            SometimesRanActions = new Dictionary<MovementsForPlayer, Action>()
            {
                { MovementsForPlayer.MoveLeft, () => {Movements = MovementsForPlayer.IdleLeft; } },
                { MovementsForPlayer.MoveDown, () => {Movements = MovementsForPlayer.IdleDown; } },
                { MovementsForPlayer.MoveUp, () =>   {Movements = MovementsForPlayer.IdleUp;   } },
                { MovementsForPlayer.MoveRight, () =>{Movements = MovementsForPlayer.IdleRight; }},
                { MovementsForPlayer.IdleRight,() => {Movements = MovementsForPlayer.IdleRight; }},
                { MovementsForPlayer.IdleLeft,() =>  {Movements = MovementsForPlayer.IdleLeft; }},
                { MovementsForPlayer.IdleUp,() =>    {Movements = MovementsForPlayer.IdleUp;   }},
                { MovementsForPlayer.IdleDown,() =>  {Movements = MovementsForPlayer.IdleDown; }},
                { MovementsForPlayer.SwingUp, () =>
                {
                   Movements = MovementsForPlayer.IdleUp;
                }},
                { MovementsForPlayer.SwingDown, () =>
                {

                   Movements = MovementsForPlayer.IdleDown;
                }},
                { MovementsForPlayer.SwingLeft, () =>
                {

                   Movements = MovementsForPlayer.IdleLeft;
                }},
                { MovementsForPlayer.SwingRight, () =>
                {

                   Movements = MovementsForPlayer.IdleRight;
                } }
            };
            OppositeDirections = new Dictionary<MovementsForPlayer, MovementsForPlayer>()
            {
                { MovementsForPlayer.MoveLeft ,  MovementsForPlayer.MoveRight   },
                { MovementsForPlayer.MoveDown ,  MovementsForPlayer.MoveUp   },
                { MovementsForPlayer.MoveUp   ,  MovementsForPlayer.MoveDown     },
                { MovementsForPlayer.MoveRight,  MovementsForPlayer.MoveLeft },
            };
            #endregion
            DefaultState = defaultState;
            Movements = DefaultState;

        }
        MovementsForPlayer returnAMovement(MovementsForPlayer lastMovement)
        {
            if (lastMovement == MovementsForPlayer.MoveUp)
            {
                return MovementsForPlayer.IdleUp;
            }
            else if (lastMovement == MovementsForPlayer.MoveDown)
            {
                return MovementsForPlayer.IdleDown;
            }
            else if (lastMovement == MovementsForPlayer.MoveRight)
            {
                return MovementsForPlayer.IdleRight;
            }
            else if (lastMovement == MovementsForPlayer.MoveLeft)
            {
                return MovementsForPlayer.IdleLeft;
            }
            return MovementsForPlayer.IdleDown;
        }
        public void CheckKeys(KeyboardState keyboardState)
        {
            var pressedKeys = keyboardState.GetPressedKeys();
            bool wasSuccesful = false;
            Keys key = Keys.None;
            for (int i = 0; i < pressedKeys.Length; i++)
            {
                
                key = pressedKeys[i];

                if (DifferentKeys.ContainsKey(key) == false)
                {
                    continue;
                }

                var currentMovement = DifferentKeys[key];
                var action = ActionsForEachKey[key];
                
                Movements = currentMovement;
                action();
                if (key != lastKey)
                {
                    CurrentFrameIndex = 0;
                }
                lastKey = key;
                wasSuccesful = true;
               
                break;
            }
            bool CheckMovements = Movements == MovementsForPlayer.MoveUp || Movements == MovementsForPlayer.MoveDown
                || Movements == MovementsForPlayer.MoveLeft || Movements == MovementsForPlayer.MoveRight;
            if (wasSuccesful == false && CheckMovements)
            {
                CurrentFrameIndex = 0;

                Movements = returnAMovement(Movements);
               
            }
            
        }
        public void CheckMouse(MouseState mouseState,GraphicsDevice Graphics,int cooldown)
        {
            watch.Start();
            if(watch.ElapsedMilliseconds >= cooldown)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.X > 0 && mouseState.Y > 0 && mouseState.X < Graphics.Viewport.Width && mouseState.Y < Graphics.Viewport.Height)
                {
                    if (Movements == MovementsForPlayer.IdleUp)
                    {
                        Movements = MovementsForPlayer.SwingUp;
                    }
                    else if (Movements == MovementsForPlayer.IdleDown)
                    {
                        Movements = MovementsForPlayer.SwingDown;
                    }
                    else if (Movements == MovementsForPlayer.IdleRight)
                    {
                        Movements = MovementsForPlayer.SwingRight;
                    }
                    else if (Movements == MovementsForPlayer.IdleLeft)
                    {
                        Movements = MovementsForPlayer.SwingLeft;
                    }
                    watch.Restart();
                }
            }
        }

        public void SetUp()
        {
            initialLerpScale = Scale;
        }
        public bool EnterPortal()
        {
            if (lerpPercentage >= 1f)
            {
                Scale = Vector2.Zero;
                return true;
            }

            Rotation += 0.1f;

            Scale = Vector2.Lerp(initialLerpScale, new Vector2(0,0), lerpPercentage);
            lerpPercentage += lerpIncrement;
            return false;
        }
        public void DrawPlayer(SpriteBatch spriteBatch, ContentManager content,GraphicsDevice graphics)
        {
            Vector2 scaleForHeart = new Vector2(0.5f);
            DrawAnimation(spriteBatch, content);
            
            HeartRectangle = new Rectangle(0, (int)(graphics.Viewport.Height - HeartImage.Height * scaleForHeart.Y), (int)(HeartImage.Width * scaleForHeart.X * NumberOfHearts)
                , (int)HeartImage.Height);

            for (int i = 0; i < NumberOfHearts; i++)
            {
                if (HeartImage == null) continue;
                spriteBatch.Draw(HeartImage, new Vector2(HeartRectangle.X + i * HeartImage.Width * scaleForHeart.X, HeartRectangle.Y), null, Color.White, 0, Vector2.Zero, scaleForHeart, SpriteEffects.None, 0);
            }
        }
    }
}
