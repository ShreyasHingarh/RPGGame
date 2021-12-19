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
    class GameScreen : Screen
    {
        public int level;
        public Map map;
        public Player1 player;
        public Texture2D pixel;
        public RepeatRotation repeatRotation;
        public bool canMove = true;
        public List<Vector2> Origin;
        public List<Rectangle> PortalFrames;
        public List<Vector2> Positions;

        SpriteFont spriteFont;
        void SetupPlayer(ContentManager Content)
        {
            Texture2D image = Content.Load<Texture2D>("PersonSpriteSheet");
            player = new Player1( Color.White, new Vector2(100,350), image, 0, Vector2.Zero, Vector2.One, MovementsForPlayer.IdleRight,Content);
        }
        public GameScreen(GraphicsDevice graphics, ContentManager Content, Color backgroundColor,GraphicsDeviceManager Graficos) : base(graphics, Content,backgroundColor,Graficos)
        {
            map = new Map(3, graphics);

            repeatRotation = RepeatRotation.DoNothing;
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.LightGreen });
            SetupPlayer(Content);
           
            level = 0;
            PortalFrames = new List<Rectangle>()
            {
                new Rectangle(387,0,125,245),
                new Rectangle(258,0,125,245),
                new Rectangle(0,0,125,245),
                new Rectangle(129,0,125,245),
            };
            Origin = new List<Vector2>()
            {
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0),
                new Vector2(0,0)
            };
            Positions = new List<Vector2>()
            {
                new Vector2(400-63/2,500 - 122/2),
                new Vector2(250-63/2,200 - 122/2),
                new Vector2(550 - 63/2,300 - 122/2),
                new Vector2(450-63/2,100 - 122/2),
            };
            Texture2D portalTexture = Content.Load<Texture2D>("Portals");

            spriteFont = Content.Load<SpriteFont>("File");
            
            map.MakeMap(Origin, portalTexture, PortalFrames, spriteFont, Positions);
        }
        public override void Update(GameTime gameTime,MouseState mouse,KeyboardState ks)
        {
            
            for (int i = 0; i < map.portals.Count; i++)
            {
                map.portals[i].Update(player, ks, gameTime, ref repeatRotation);
            }

            switch (repeatRotation)
            {
                case RepeatRotation.DoNothing:
                    player.SetUp();
                    break;
                case RepeatRotation.LoopRotation:
                    bool didFinish = player.EnterPortal();

                    canMove = false;
                    if (didFinish)
                    {
                        repeatRotation = RepeatRotation.Stop;
                    }
                    break;
                case RepeatRotation.Stop:
                    player.Scale = Vector2.One;
                    player.Rotation = 0;
                    canMove = true;
                    Game1.manager.AddScreen(ScreenStates.Portal1Screen, new Level1Screen(Graphics, Content,player,Color.LightGreen,GraphicsManager));
                    Game1.manager.SetScreen(ScreenStates.Portal1Screen);


                    break;
            }


            if (canMove)
            {
                player.CheckKeys(ks);
                player.Animate(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite tile in map.Tiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (Portal portal in map.portals)
            {
                portal.DrawPortals(spriteBatch, spriteFont, Graphics, pixel);
            }
            player.DrawPlayer(spriteBatch, Content, Graphics);

        }
    }
}
