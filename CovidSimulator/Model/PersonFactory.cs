using System;
using System.Drawing;

namespace Model
{
    public static class PersonFactory
    {

        public static Person CreatePerson(PersonTypes type, Point pos, string name = "")
        {
            Person person;

            switch (type)
            {
                case PersonTypes.STUDENT:
                    person = new Student(pos, name);
                    break;
                case PersonTypes.TUTOR:
                    person = new Tutor(pos, name);
                    break;
                case PersonTypes.ADMINISTRATION:
                    person = new Administration(pos, name);
                    break;
                default:
                    throw new Exception("Unknown person type: " + type);
            }

            return person;
        }

    }
}
