using Controller;
using Model;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View;
using System.Threading;

namespace Main
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EntityLoader loader = new EntityLoader();
            loader.LoadFromFile("map.txt");

            List<DisplayableElement> displayableGrid = EntityDisplayConverter.ToDisplayableElements(loader.grid.SelectMany(node => node).ToList());
            SimulationForm simulation = new SimulationForm(displayableGrid);

            SimulationController controller = new SimulationController(loader.grid, loader.movableEntities, loader.rooms);


            // close event from the GUI
            // simulation.onCloseEvent += (object sender, EventArgs e) => controller.Dispose_();
            // close event from the console
            // AppDomain.CurrentDomain.ProcessExit += new EventHandler((object sender, EventArgs e) => controller.Dispose_());

            Thread controllerThread = new Thread(controller.start);
            controllerThread.Start();

            Application.Run(simulation);
        }
    }
}
