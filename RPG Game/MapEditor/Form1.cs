using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class Form1 : Form
    {

        Button AddImageToBox;
        ComboBox ChangeColor;
        Label label;
        PictureBox Screen;
        Dictionary<string,Image> stringToImage;
        PictureBox picture;
        Bitmap map;
        Graphics gfx;
        bool isFileOpen = false;
        public Form1()
        {
            InitializeComponent();
            Width = 900;
            Height = 900;
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            picture = new PictureBox();
            picture.Size = Properties.Resources.grass_tile_2.Size;
            picture.Image = Properties.Resources.grass_tile_2;
            Controls.Add(picture);

            ChangeColor = new ComboBox
            {
                Location = new Point(Width - 200, 0),
                Size = new Size(80, 30),
                Text = "Change texture"
            };
            ChangeColor.Items.Add("Building1");
            ChangeColor.Items.Add("Building2");
            ChangeColor.Items.Add("HPath");
            ChangeColor.Items.Add("VPath");
            ChangeColor.Items.Add("Grasspath");
            ChangeColor.SelectedValueChanged += ChangeColor_SelectedValueChanged;
            Controls.Add(ChangeColor);

            stringToImage = new Dictionary<string, Image>
            {
                { "Building1",Properties.Resources.Building1},
                { "Building2",Properties.Resources.Building2},
                { "HPath",Properties.Resources.HorizontalPath },
                { "VPath",Properties.Resources.VerticalPath},
                { "Grasspath",Properties.Resources.grass_tile_2}
            };


            map = new Bitmap(Width,Height);
            Screen = new PictureBox();
            Screen.Location = new Point(0,0);
            Screen.Image = map;
            Screen.Size = new Size(700,  700);
            Controls.Add(Screen);

            AddImageToBox = new Button
            {
                Size = new Size(100,50),
                Location = new Point(Width - 200,Height - 100),
                Text = "Add Image",
            };
            AddImageToBox.Click += AddImageToBox_Click;
            Controls.Add(AddImageToBox);

            gfx = Graphics.FromImage(Screen.Image);
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
            if(result == DialogResult.OK)
            {
                Bitmap image = new Bitmap(dialog.FileName);
                ChangeColor.Items.Add(dialog.SafeFileName);
                stringToImage.Add(dialog.SafeFileName,image);
                isFileOpen = false;
            }
            else if(result == DialogResult.Cancel)
            {
                isFileOpen = false;
            }
        }

        private void ChangeColor_SelectedValueChanged(object sender, EventArgs e)
        {
            picture.Image = stringToImage[(string)ChangeColor.Items[ChangeColor.SelectedIndex]];
            picture.Size = stringToImage[(string)ChangeColor.Items[ChangeColor.SelectedIndex]].Size;
        }

        private void ChangeTimer_Tick(object sender, EventArgs e)
        {
            if(!isFileOpen)
            {
                Rectangle mouse = new Rectangle(PointToClient(Cursor.Position),new Size(0,0));
                Rectangle BoundryForScreen = new Rectangle(Screen.Location, Screen.Size);
                Rectangle pictureBoundry = new Rectangle(picture.Location,picture.Size);
                if(BoundryForScreen.Contains(mouse))
                {
                    
                    if (MouseButtons == MouseButtons.Left)
                    { 
                        if (picture.Location.X + picture.Width <= BoundryForScreen.Width && picture.Location.Y + picture.Height <= BoundryForScreen.Height)
                        {
                            gfx.DrawImage(picture.Image, pictureBoundry);
                            
                        }
                        
                    }
                        picture.Location = mouse.Location; 
                    
                    
                }
                
                label.Text = $"{mouse.X},{mouse.Y}";
            }
        }
       
    }
}
