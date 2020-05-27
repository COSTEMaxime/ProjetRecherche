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
                if (person.AsMoved) { continue; }

                if (person.Path.Count == 0)
                {
                    // try to find a new objective
                    if (person.WantToMove())
                    {
                        Room destination = person.SelectDestination(ListPossibleRooms(person.Type));
                        person.Path = pathFinder.Pathfinding(person.Position, destination.Position);
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
                            if (other.Path.Peek() == nextPosition && !other.AsMoved)
                            {
                                // swap
                                other.Position = other.Path.Pop();
                                person.Position = person.Path.Pop();
                                break;
                            }
                        }
                        else
                        {
                            // wait
                            person.AsMoved = true;
                            break;
                        }
                    }

                    if (!person.AsMoved)
                    {
                        person.Position = person.Path.Pop();
                    }
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
