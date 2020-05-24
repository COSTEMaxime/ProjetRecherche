using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class PersonFactory
    {

        public static Person CreatePerson(PersonTypes type, Point pos, string name="")
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
