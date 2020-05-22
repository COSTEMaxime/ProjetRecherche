using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EntityLoader
    {
        public List<List<Entity>> grid { get; private set; }
        public List<MovableEntity> movableEntities { get; private set; }
        public List<Room> rooms { get; private set; }

        public void LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
