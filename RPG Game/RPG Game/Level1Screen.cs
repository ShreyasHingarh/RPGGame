using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;
using PictureLibrary;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RPG_Game
{
   
    

    class Level1Screen : Screen
    {

        
        Sprite[,] Background;
        Player1 Player;
        List<HumanEnemy> Humans;
        Texture2D pixel;
        int TileSize = 16;
        SpriteFont Font;
        
        List<PictureClassForLibrary> AllImages;
        List<Sprite> Buildings = new List<Sprite>();
        List<Sprite> Paths = new List<Sprite>();

        Rectangle AttackBoundry;
        Rectangle boundry; 
         void SetupGroundCover()
         {
           Texture2D Image = Content.Load<Texture2D>("GreenSquare");
            for (int y = 0; y < Background.GetLength(0); y++)
            {
                for (int x = 0; x < Background.GetLength(1); x++)
                {
                    Background[y, x] = new Sprite(Microsoft.Xna.Framework.Color.White, new Vector2(TileSize * x, TileSize * y),Image, 0, new Vector2(Image.Width/2,Image.Height/2), new Vector2(1,1));
                    
                }
            }
         }
        public Level1Screen(GraphicsDevice graphics, ContentManager content,Player1 player, Microsoft.Xna.Framework.Color backgroundColor, GraphicsDeviceManager Graficos): base(graphics,content, backgroundColor,Graficos)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[]{ Color.Black * 0.3f });
            GraphicsManager.PreferredBackBufferWidth = 710;
            GraphicsManager.PreferredBackBufferHeight = 710;
            GraphicsManager.ApplyChanges();
            AllImages = JsonConvert.DeserializeObject <List<PictureClassForLibrary>>(File.ReadAllText("AllDifferentPictures.json"));
            Font = Content.Load<SpriteFont>("File");
            Player = player;
            Player.Position = new Vector2(360,360);
            
            int x = 95;
            int y = 160;
            int increment = 65;
            Humans = new List<HumanEnemy>();
            float firstincrement = 0.0023f;
            for (int i = 0; i < 1; i++)
            {
                List<Vector2> EndPositions = new List<Vector2>
                {
                    new Vector2(x,y),
                    new Vector2(615,y),
                    new Vector2(615,550),
                    new Vector2(x,550),
                };
                float LerpIncrement = firstincrement;
                
                Humans.Add(new HumanEnemy(Color.White, new Vector2(x + increment * i , y), content.Load<Texture2D>("HumanEnemy"), 0, Vector2.Zero, Vector2.One, EnemyMovements.IdleRight,content,EndPositions,LerpIncrement,0f + i * 0.125f,Player));
            }
         
            
            Background = new Sprite[ Graphics.Viewport.Height / TileSize + 1, Graphics.Viewport.Width/ TileSize+1];
            SetupGroundCover();

            HashSet<string> differences = new HashSet<string>();

            foreach (var image in AllImages)
            {
                differences.Add(image.ImageType);

            
                if (image.ImageType == "Building1" ||image.ImageType == "Building2" || image.ImageType == "InbetweenBuilding")
                {
                   
                    Buildings.Add(new Sprite(Color.White,new Vector2(image.Position.X,image.Position.Y), Content.Load<Texture2D>($"RPGBackgroundImages/{image.ImageType}"), 0,Vector2.Zero,Vector2.One));
                }
                else if(image.ImageType == "HPath" || image.ImageType == "VPath" ||image.ImageType == "grass-tile-2")
                {
                    Paths.Add(new Sprite(Color.White, new Vector2(image.Position.X, image.Position.Y), Content.Load<Texture2D>($"RPGBackgroundImages/{image.ImageType}"), 0, Vector2.Zero, Vector2.One));
                   
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (Sprite square in Background)
            {
                square.Draw(spriteBatch);
            }
            foreach (var imaje in Paths)
            {
                spriteBatch.Draw(imaje.Image, imaje.Position,Color.White);
              
            }
            foreach (var imaje in Buildings)
            {
                spriteBatch.Draw(imaje.Image, imaje.Position, Color.White);
               
            }
           
            Player.DrawPlayer(spriteBatch,Content,Graphics);

            //if (boundry != null)
            //{
            //    spriteBatch.Draw(pixel, boundry, Color.White * 1.5f);
            //}
            //if (AttackBoundry != null)
            //{
            //    spriteBatch.Draw(pixel, AttackBoundry, Color.White * 1.5f);
            //}
            //spriteBatch.Draw(pixel, Player.HitBox.Value, Color.White * 1.5f);

            foreach (HumanEnemy human in Humans)
            {
                human.DrawAnimation(spriteBatch,Content);
               // spriteBatch.DrawString(Font, $"HMTPStates Case: {human.HMTPStates}\n HAPStates Case: {human.HAPStates}", new Vector2(100, 130), Color.Red);
            }


           
        }
        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState ks)
        {

            Player.isIntersecting = false;
            Player.CheckKeys(ks);
            foreach (var image in Buildings)
            {
                if (image.HitBox.Value.Intersects(Player.HouseHitBox))
                {
                    Player.isIntersecting = true;
                }
            }
            foreach (HumanEnemy human in Humans)
            {
                if (human.HitBox.Value.Intersects(Player.HitBox.Value) || Player.HitBox.Value.Intersects(human.HitBox.Value) 
                    && (Player.MovementType != GeneralMovementTypes.Attacking && human.MovementType != GeneralMovementTypes.Attacking) && )
                {
                    Player.isIntersecting = true;
                    human.CurrentFrameIndex = 0;
                    human.Movements = human.ReturnIdleMovement();
                    
                }
                else
                {
                    human.MovePlaces(ref boundry, ref AttackBoundry);
                }
               
                
                human.Animate(gameTime);
                
            }

            if (Player.isIntersecting == false)
            {
                Player.CheckKeys(ks);
                Player.CheckMouse(mouse,Graphics,1000);
                Player.Animate(gameTime);
            }

            
            

            
        }
    }
}
