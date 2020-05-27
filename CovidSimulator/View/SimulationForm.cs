using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class SimulationForm : Form
    {
        private List<DisplayableElement> grid;
        public event EventHandler onCloseEvent;
        public event EventHandler onShowEvent;

        private List<PictureBox> pictureBoxes;

        public SimulationForm(List<DisplayableElement> grid)
        {
            InitializeComponent();
            this.grid = grid;

            init();
        }

        private void init()
        {
            pictureBoxes = new List<PictureBox>();

            foreach (DisplayableElement element in grid)
            {
                PictureBox pict = new PictureBox
                {
                    Location = new Point(element.position.X * DisplayableElement.SIZE, element.position.Y * DisplayableElement.SIZE),
                    Size = new Size(DisplayableElement.SIZE, DisplayableElement.SIZE)
                };

                if (element.image != null)
                {
                    pict.Image = element.image;
                }
                else
                {
                    pict.BackColor = (Color)element.color;
                }

                pictureBoxes.Add(pict);
            }
        }

        private void drawElements(List<DisplayableElement> elements)
        {
            foreach (DisplayableElement element in elements)
            {
                PictureBox pictureBox = pictureBoxes.Find(x => x.Location.X == element.position.X * DisplayableElement.SIZE && x.Location.Y == element.position.Y * DisplayableElement.SIZE);
                if (pictureBox == null)
                {
                    throw new Exception("Should not be there");
                }
                else if (element.color != null && pictureBox.BackColor != element.color)
                {
                    pictureBox.BackColor = (Color)element.color;
                    pictureBox.Image = null;
                }
                else if (element.image != null && pictureBox.Image != element.image)
                {
                    pictureBox.Image = element.image;
                    pictureBox.BackColor = Color.White;
                }
            }
        }

        public void refresh(List<DisplayableElement> elements)
        {
            drawElements(grid);
            drawElements(elements);
        }

        private void SimulationForm_Shown(object sender, EventArgs e)
        {
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                Invoke(new MethodInvoker(() => { Controls.Add(pictureBox); }));
            }

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            onShowEvent?.Invoke(this, new EventArgs());
        }

        private void SimulationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseEvent?.Invoke(this, new EventArgs());
        }
    }
}
