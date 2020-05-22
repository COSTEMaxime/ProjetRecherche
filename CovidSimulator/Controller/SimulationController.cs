using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Controller
{
    public class SimulationController
    {
        private List<List<Entity>> grid;
        private List<MovableEntity> movableEntities;
        private List<Room> rooms;

        public System.Timers.Timer timer { get; private set; }

        public SimulationController(List<List<Entity>> grid, List<MovableEntity> movableEntities, List<Room> rooms)
        {
            this.grid = grid;
            this.movableEntities = movableEntities;
            this.rooms = rooms;

            timer = new System.Timers.Timer(2000);
            timer.Elapsed += OnTimedEvent;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        public void start()
        {
            timer.Start();
            while (true)
            {
                // Processes all the events in the queue.
                // Application.DoEvents();
            }
        }
    }
}
