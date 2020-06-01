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
        private List<Person> movableEntities;
        private List<Room> rooms;
        private bool disposedValue;

        private PathFinder pathFinder;
        private SimulationForm simulationForm;
        private System.Timers.Timer timer;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private StatsAggregator statsAggregator;

        // each tick represents 30s
        private static int SIMULATION_MAX_TICK = 8 * 60 * 2;
        // delay between each tick
        private static int TICK_SPEED_MS = 25;
        private int tickCount;

        public SimulationController(SimulationForm simulationForm, List<List<Node>> grid, List<Person> movableEntities, List<Room> rooms)
        {
            this.simulationForm = simulationForm;

            this.movableEntities = movableEntities;
            this.rooms = rooms;

            this.tickCount = 0;

            this.pathFinder = new PathFinder(grid);
            this.statsAggregator = new StatsAggregator();

            timer = new System.Timers.Timer(TICK_SPEED_MS);
            timer.Elapsed += OnTimedEvent;

            logger.Info("Started new simulation");
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (tickCount++ > SIMULATION_MAX_TICK)
            {
                timer.Stop();

                statsAggregator.consoleDisplay();
            }

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
                if (person.AsMoved) { continue; }

                if (person.Path.Count == 0)
                {
                    // try to find a new objective
                    if (person.WantToMove())
                    {
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
                    Person otherPerson = movableEntities.Find(p => p.Position == new Point(person.Position.X, person.Position.Y + 1));
                    logger.Info("{0}, pos: {1}, came too close with {2}, pos: {3}",
                        person.Name,
                        person.Position.X + ";" + person.Position.Y,
                        otherPerson.Name,
                        otherPerson.Position.X + ";" + otherPerson.Position.Y
                    );

                    statsAggregator.AddPeopleTooClose(person, otherPerson);
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X, person.Position.Y - 1)) != null)
                {
                    Person otherPerson = movableEntities.Find(p => p.Position == new Point(person.Position.X, person.Position.Y - 1));
                    logger.Info("{0}, pos: {1}, came too close with {2}, pos: {3}",
                        person.Name,
                        person.Position.X + ";" + person.Position.Y,
                        otherPerson.Name,
                        otherPerson.Position.X + ";" + otherPerson.Position.Y
                    );

                    statsAggregator.AddPeopleTooClose(person, otherPerson);
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X + 1, person.Position.Y)) != null)
                {
                    Person otherPerson = movableEntities.Find(p => p.Position == new Point(person.Position.X + 1, person.Position.Y));
                    logger.Info("{0}, pos: {1}, came too close with {2}, pos: {3}",
                        person.Name,
                        person.Position.X + ";" + person.Position.Y,
                        otherPerson.Name,
                        otherPerson.Position.X + ";" + otherPerson.Position.Y
                    );

                    statsAggregator.AddPeopleTooClose(person, otherPerson);
                }
                if (movableEntities.Find(p => p.Position == new Point(person.Position.X - 1, person.Position.Y)) != null)
                {
                    Person otherPerson = movableEntities.Find(p => p.Position == new Point(person.Position.X - 1, person.Position.Y));
                    logger.Info("{0}, pos: {1}, came too close with {2}, pos: {3}",
                        person.Name,
                        person.Position.X + ";" + person.Position.Y,
                        otherPerson.Name,
                        otherPerson.Position.X + ";" + otherPerson.Position.Y
                    );

                    statsAggregator.AddPeopleTooClose(person, otherPerson);
                }
            }

            foreach (Room room in rooms)
            {
                if (room.NbCurrentPeople > room.NbMaxPeople)
                {
                    statsAggregator.AddOvercrowdedRoom(room);
                }
            }
        }

        private void redraw()
        {
            List<DisplayableElement> elements = EntityDisplayConverter.ToDisplayableElements(rooms.ToList<IPosition>());
            elements.AddRange(EntityDisplayConverter.ToDisplayableElements(movableEntities.ToList<IPosition>()));
            simulationForm.refresh(elements);
        }

        private List<Room> ListPossibleRooms(PersonTypes type)
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
