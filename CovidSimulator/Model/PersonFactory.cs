using System;
using System.Drawing;

namespace Model
{
    public static class PersonFactory
    {

        public static Person CreatePerson(PersonTypes type, Point pos, string name = "", Room mainRoom = null)
        {
            Person person;

            switch (type)
            {
                case PersonTypes.STUDENT:
                    person = new Student(pos, name, mainRoom);
                    break;
                case PersonTypes.TUTOR:
                    person = new Tutor(pos, name, mainRoom);
                    break;
                case PersonTypes.ADMINISTRATION:
                    person = new Administration(pos, name, mainRoom);
                    break;
                default:
                    throw new Exception("Unknown person type: " + type);
            }

            return person;
        }

    }
}
