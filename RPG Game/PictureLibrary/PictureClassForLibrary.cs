

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PictureLibrary
{
    public class PictureClassForLibrary
    {
        public string ImageType { get; set; }
        public Point Position { get; set; }
        
        public Size Size { get; set; }
        public PictureClassForLibrary(string imageType,int x,int y,int width,int height)
        {
            ImageType = imageType;
            Position = new Point(x, y);
            Size = new Size(width, height);
        }
        public PictureClassForLibrary()
        {

        }
    }
}
