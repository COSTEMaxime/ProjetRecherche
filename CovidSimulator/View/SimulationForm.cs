using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class SimulationForm : Form
    {
        private List<DisplayableElement> grid;
        public event EventHandler onCloseEvent;

        public SimulationForm(List<DisplayableElement> grid)
        {
            InitializeComponent();
            this.grid = grid;
        }

        public void drawElements(List<DisplayableElement> elements)
        {
            Invoke(new MethodInvoker(() => { Controls.Clear(); }));

            foreach (DisplayableElement element in elements)
            {
                PictureBox pict = new PictureBox
                {
                    Location = new Point(element.position.X * DisplayableElement.SIZE, element.position.Y * DisplayableElement.SIZE),
                    Size = new Size(DisplayableElement.SIZE, DisplayableElement.SIZE),
                    BackColor = element.color
                };

                this.Invoke(new MethodInvoker(() => { Controls.Add(pict); }));
            }
        }

        private void SimulationForm_Shown(object sender, EventArgs e)
        {
            drawElements(grid);
        }

        private void SimulationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            onCloseEvent?.Invoke(this, new EventArgs());
        }
    }
}
