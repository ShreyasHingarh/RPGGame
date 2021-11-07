using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    class Map
    {
        Random gen;
        public List<Portal> portals;
        int seed;
        Texture2D pixel;
        public Sprite[,] Tiles;
        List<Color> SpecifiedColors;
        int TilesSize;

        int Width;
        int Height;

        public Map(int tileSize, GraphicsDevice graphicsDevice)
        {

            Width = graphicsDevice.Viewport.Width;
            Height = graphicsDevice.Viewport.Height;


            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            TilesSize = tileSize;
            seed = 97;
            Tiles = new Sprite[graphicsDevice.Viewport.Width / TilesSize, graphicsDevice.Viewport.Height / TilesSize];
            gen = new Random(seed);
            for (int y = 0; y < Tiles.GetLength(0); y++)
            {
                for (int x = 0; x < Tiles.GetLength(1); x++)
                {
                    Tiles[y, x] = new Sprite(Color.White, new Vector2(TilesSize * x, TilesSize * y), null, 0, Vector2.Zero, new Vector2(TilesSize, TilesSize));
                }
            }
            SpecifiedColors = new List<Color>
            {
                Color.GhostWhite,
                Color.DarkOliveGreen,
                Color.DarkBlue,
                Color.Green,
                Color.Red
            };
            portals = new List<Portal>();
        }

        bool IsInBounds(TwoDIndex current)
        {
            return current.X >= 0 && current.X < Tiles.GetLength(1) && current.Y < Tiles.GetLength(0) && current.Y >= 0;
        }
        public void FloodFill(int x, int y, Color color, double chanceToFill, double chanceToDecay)
        {
            List<TwoDIndex> filler = new List<TwoDIndex>();
            filler.Add(new TwoDIndex(x / TilesSize, y / TilesSize));
            while (filler.Count > 0)
            {
                TwoDIndex current = filler[0];
                filler.Remove(current);


                if (IsInBounds(current))
                {
                    Sprite currentSprite = Tiles[current.Y, current.X];
                    if (currentSprite.Image == null)
                    {
                        TwoDIndex right = new TwoDIndex(current.X + 1, current.Y);
                        TwoDIndex left = new TwoDIndex(current.X - 1, current.Y);
                        TwoDIndex up = new TwoDIndex(current.X, current.Y - 1);
                        TwoDIndex down = new TwoDIndex(current.X, current.Y + 1);



                        double coinToss = gen.NextDouble();
                        if (coinToss < chanceToFill)
                        {
                            filler.Add(right);
                            filler.Add(left);
                            filler.Add(up);
                            filler.Add(down);
                        }
                        currentSprite.Image = pixel;
                        currentSprite.Tint = color;

                        chanceToFill *= chanceToDecay;
                    }
                }
            }


        }
        public void FillAllSquares()
        {
            Color PreviousColor = Tiles[0, 0].Tint;
            for (int y = 0; y < Tiles.GetLength(0); y++)
            {
                for (int x = 0; x < Tiles.GetLength(1); x++)
                {
                    if (Tiles[y, x].Image == null)
                    {
                        Tiles[y, x].Tint = PreviousColor;
                        Tiles[y, x].Image = pixel;
                    }
                    if (Tiles[y, x].Tint != null)
                    {
                        PreviousColor = Tiles[y, x].Tint;
                    }


                }
            }
        }
        public void MakeMap(List<Vector2> Origins, Texture2D image, List<Rectangle> sourceRectangle, SpriteFont font, List<Vector2> Positions)
        {
            int xPos;
            int yPos;
            for (int i = 0; i < SpecifiedColors.Count; i++)
            {
                do
                {
                    xPos = gen.Next(0, Width);
                    yPos = gen.Next(0, Height);
                } while (Tiles[yPos / TilesSize, xPos / TilesSize].Image != null);

                FloodFill(xPos, yPos, SpecifiedColors[i], 1, 0.9999);
                if (i != 0)
                {
                    portals.Add(new Portal(new Vector2(Positions[i - 1].X, Positions[i - 1].Y), new Vector2(0.5f, 0.5f), Color.White, 0, Origins[i - 1], image, sourceRectangle[i - 1], i - 1, font));
                }
            }

            FillAllSquares();
        }
    }
}
