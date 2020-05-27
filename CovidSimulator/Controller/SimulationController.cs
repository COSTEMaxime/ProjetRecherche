using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using View;

namespace Controller
{
    public class SimulationController : IDisposable
    {
        private List<List<Node>> grid;
        private List<Person> movableEntities;
        private List<Room> rooms;
        private bool disposedValue;

        private PathFinder pathFinder;

        private SimulationForm simulationForm;

        public System.Timers.Timer timer { get; private set; }

        public SimulationController(SimulationForm simulationForm, List<List<Node>> grid, List<Person> movableEntities, List<Room> rooms)
        {
            this.simulationForm = simulationForm;

            this.grid = grid;
            this.movableEntities = movableEntities;
            this.rooms = rooms;

            this.pathFinder = new PathFinder(grid);

            timer = new System.Timers.Timer(500);
            timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick");

            update();
            stats();
            redraw();
        }

        public void start()
        {
            timer.Start();
            redraw();
            while (true) { }
        }

        private void update()
        {
            foreach (Person person in movableEntities) { person.AsMoved = false; }

            foreach (Person person in movableEntities)
            {
                Console.WriteLine(person.Name + " turn");
                if (person.AsMoved) { continue; }

                if (person.Path.Count == 0)
                {
                    // try to find a new objective
                    if (person.WantToMove())
                    {
                        Console.WriteLine(person.Name + " will choose a new direction");
                        Room destination = person.SelectDestination(ListPossibleRooms(person.Type));
                        person.Path = pathFinder.Pathfinding(person.Position, destination.Position);

                        Room source = rooms.Find(r => r.Position == person.Position);
                        if (source != null)
                        {
                            source.LeaveRoom(person);
                        }
                    }
                    else { person.AsMoved = true; }
                }
                else
                {
                    Point nextPosition = person.Path.Peek();

                    foreach (Person other in movableEntities)
                    {
                        if (other == person) { continue; }

                        if (other.Position == nextPosition)
                        {
                            if (rooms.Find(r => r.Position == nextPosition) != null)
                            {
                                // go inside a room
                                break;
                            }
                            else if (other.Path.Peek() == person.Position && !other.AsMoved)
                            {
                                Console.WriteLine("SWAP");
                                // swap
                                other.Position = other.Path.Pop();
                                person.Position = person.Path.Pop();
                                break;
                            }
                            else
                            {
                                // wait
                                person.AsMoved = true;
                                break;
                            }
                        }
                    }

                    if (!person.AsMoved)
                    {
                        person.Position = person.Path.Pop();
                    }

                    if (person.Path.Count == 0)
                    {
                        Room room = rooms.Find(r => r.Position == person.Position);
                        room.EnterRoom(person);
                    }
                }
            }
        }

        private void stats()
        {
            foreach (Person person in movableEntities)
            {
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X, person.Position.Y + 1)) != null)
                {
                    Console.WriteLine("Too close !");
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X, person.Position.Y - 1)) != null)
                {
                    Console.WriteLine("Too close !");
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X + 1, person.Position.Y)) != null)
                {
                    Console.WriteLine("Too close !");
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X - 1, person.Position.Y)) != null)
                {
                    Console.WriteLine("Too close !");
                }
            }
        }

        private void redraw()
        {
            List<DisplayableElement> elements = EntityDisplayConverter.ToDisplayableElements(rooms.ToList<IPosition>());
            elements.AddRange(EntityDisplayConverter.ToDisplayableElements(movableEntities.ToList<IPosition>()));
            simulationForm.refresh(elements);
        }

        private List<Room> ListPossibleRooms (PersonTypes type)
        {
            return rooms.FindAll(room => room.Allowed.Contains(type));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Stop();
                    timer.Elapsed -= OnTimedEvent;
                    timer.Dispose();
                    // TODO: supprimer l'état managé (objets managés)
                }

                // TODO: libérer les ressources non managées (objets non managés) et substituer le finaliseur
                // TODO: affecter aux grands champs une valeur null
                disposedValue = true;

                Environment.Exit(0);
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~SimulationController()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
