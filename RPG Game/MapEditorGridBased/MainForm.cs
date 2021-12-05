using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MapEditorGridBased.Properties;

using Newtonsoft.Json;

using PictureLibrary;

namespace MapEditorGridBased
{
    public partial class MainForm : Form
    {
        Button AddImageToBox;
        ComboBox ChangeImage;
        PictureBox Screen;
        Bitmap map;
        Graphics gfx;
        PictureClassForLibrary picture;
        Label label;
        Dictionary<string, string> ImageTypeToFilePath;
        List<PictureClassForLibrary> AllPictures;
        bool isFileOpen = false;
        int xAxis = 0;
        int y = 0;
        

      

        void SetupGroundCover()
        {
            Image image = Properties.Resources.grass_tile_2;
            for (int i = 0; i < 720 / image.Height; i++)
            {
                for (int x = 0; x < 720 / image.Width; x++)
                {
                    PictureBox tempPicture = new PictureBox()
                    {
                        Location = new Point(xAxis, y),
                        Size = new Size(image.Width, image.Height),
                        Image = image
                    };


                    gfx.DrawImage(tempPicture.Image, tempPicture.Location);

                    xAxis = xAxis + image.Width;
                }
                y += image.Height;
                xAxis = 0;
            }
        }
        public MainForm()
        {

            InitializeComponent();
            Width = 900;
            Height = 900;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            map = new Bitmap(Width, Height);
            Screen = new PictureBox();
            Screen.Location = new Point(0, 0);
            Screen.Image = map;
            Screen.Size = new Size(720, 720);
            Controls.Add(Screen);
            gfx = Graphics.FromImage(Screen.Image);
            SetupGroundCover();
            string text = System.IO.File.ReadAllText("AllImages.json");
            string TextForPictures = System.IO.File.ReadAllText(@"AllDifferentPictures.json");
            AllPictures = JsonConvert.DeserializeObject<List<PictureClassForLibrary>>(TextForPictures);
            picture = new PictureClassForLibrary("GrassTile",0,0, Properties.Resources.grass_tile_2.Width,Properties.Resources.grass_tile_2.Height);
            
            ImageTypeToFilePath = JsonConvert.DeserializeObject<Dictionary<string,string>>(text);
            

            ChangeImage = new ComboBox
            {
                Location = new Point(Width - 200, 0),
                Size = new Size(80, 30),
                Text = "Change texture"
            };
            foreach(var images in ImageTypeToFilePath)
            {
                ChangeImage.Items.Add(images.Key);
            }
            ChangeImage.SelectedValueChanged += ChangeColor_SelectedValueChanged;
            Controls.Add(ChangeImage);

            

            AddImageToBox = new Button
            {
                Size = new Size(100, 50),
                Location = new Point(Width - 200, Height - 100),
                Text = "Add Image",
            };
            AddImageToBox.Click += AddImageToBox_Click;
            Controls.Add(AddImageToBox);

           
            
            label = new Label()
            {
                Location = new Point(Width - 200, Height - 200),
                Size = new Size(100, 100),
                Text = $"{MousePosition.X},{MousePosition.Y}"
            };
            Controls.Add(label);
        }

        private void AddImageToBox_Click(object sender, EventArgs e)
        {
            isFileOpen = true;
            OpenFileDialog dialog = new OpenFileDialog();

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Bitmap image = new Bitmap(dialog.FileName);
                ChangeImage.Items.Add(dialog.SafeFileName);
                ImageTypeToFilePath.Add(dialog.SafeFileName, $"{dialog.FileName}");
                string images = JsonConvert.SerializeObject(ImageTypeToFilePath);
                System.IO.File.WriteAllText(@"C:\Users\Shreyas.Hingarh\Documents\Visual Studio 2019\Projects\RPG Game\MapEditorGridBased\AllImages.json", images);
                ChangeImage.SelectedIndex = ChangeImage.Items.Count-1;
                isFileOpen = false;
            }
            else if (result == DialogResult.Cancel)
            {
                isFileOpen = false;
            }
        }

        private void ChangeColor_SelectedValueChanged(object sender, EventArgs e)
        {

            picture.ImageType = (string)ChangeImage.Items[ChangeImage.SelectedIndex];
            string imageToGive = ImageTypeToFilePath[(string)ChangeImage.Items[ChangeImage.SelectedIndex]];

            Bitmap image = new Bitmap(imageToGive);
            picture.Size = image.Size;

        }

        private void MakeImageTimer_Tick(object sender, EventArgs e)
        {
            
            if (!isFileOpen && ChangeImage.SelectedIndex != -1 && AllPictures.Count >= 0)
            {
                
                Rectangle mouse = new Rectangle(PointToClient(Cursor.Position), new Size(0, 0));
                Rectangle BoundryForScreen = new Rectangle(Screen.Location, Screen.Size);

                Rectangle pictureBoundry = new Rectangle(picture.Position,picture.Size);
                if (MouseButtons == MouseButtons.Right && AllPictures.Count >0)
                {
                    AllPictures.Remove(AllPictures[AllPictures.Count - 1]);    
                }
                else if (BoundryForScreen.Contains(mouse))
                {
                    if (MouseButtons == MouseButtons.Left)
                    {
                        if (picture.Position.X + picture.Size.Width <= BoundryForScreen.Width && picture.Position.Y + picture.Size.Height <= BoundryForScreen.Height)
                        {
                            PictureClassForLibrary setPicture = new PictureClassForLibrary(picture.ImageType,picture.Position.X,picture.Position.Y,picture.Size.Width,picture.Size.Height);                          
                            AllPictures.Add(setPicture);
                        }
                    }
                    picture.Position = mouse.Location;
                }
                string picturesToAdd = JsonConvert.SerializeObject(AllPictures);
                System.IO.File.WriteAllText(@"C:\Users\Shreyas.Hingarh\Documents\Visual Studio 2019\Projects\RPG Game\MapEditorGridBased\AllDifferentPictures.json", picturesToAdd);
                gfx.Clear(Color.White);
                xAxis = 0;
                y = 0;
                 
                SetupGroundCover();

                label.Text = $"{mouse.X},{mouse.Y}";
            }
            foreach (var picture in AllPictures)
            {
                Bitmap image = new Bitmap(ImageTypeToFilePath[picture.ImageType]);
                gfx.DrawImage(image, picture.Position.X, picture.Position.Y, picture.Size.Width, picture.Size.Height);
            }
            Screen.Image = map;

        }
    }
}
