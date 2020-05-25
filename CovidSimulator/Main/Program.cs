using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using View;

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

            foreach (var item in loader.grid)
            {
                foreach (var node in item)
                {
                    Console.Write(node.walkingDirection.ToString());
                }
                Console.WriteLine("");
            }

            Console.WriteLine(loader.movableEntities.First().Name);
            Console.WriteLine(loader.rooms.First().Name);

            List<DisplayableElement> displayableGrid = EntityDisplayConverter.ToDisplayableElements(loader.grid.SelectMany(node => node).ToList<IPosition>());
            SimulationForm simulationForm = new SimulationForm(displayableGrid);

            SimulationController controller = new SimulationController(simulationForm, loader.grid, loader.movableEntities, loader.rooms);


            // close event from the GUI
            simulationForm.onCloseEvent += (object sender, EventArgs e) => controller.Dispose();
            // close event from the console
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((object sender, EventArgs e) => controller.Dispose());

            Thread controllerThread = new Thread(controller.start);
            simulationForm.onShowEvent += new EventHandler((object sender, EventArgs e) => controllerThread.Start());

            Application.Run(simulationForm);
        }
    }
}
