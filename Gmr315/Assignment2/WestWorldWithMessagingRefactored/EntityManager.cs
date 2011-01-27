#region Using

using System;
using System.Collections.Generic;

#endregion

namespace WestWorldWithMessagingRefactored
{
    public class EntityManager
    {
        //to facilitate quick lookup the entities are stored in a std::map, in which
        //pointers to entities are cross referenced by their identifying number
        private static EntityManager _instance;
        private readonly Dictionary<int, BaseGameEntity> _entityMap;


        private EntityManager()
        {
            _entityMap = new Dictionary<int, BaseGameEntity>();
        }


        public static EntityManager Instance {
            get {
            if (_instance == null)
            {
                _instance = new EntityManager();
            }
            return _instance;
                }
        }

        //this method stores a pointer to the entity in the std::vector
        //m_Entities at the index position indicated by the entity's ID
        //(makes for faster access)
        public void RegisterEntity(BaseGameEntity newEntity)
        {
            _entityMap.Add(newEntity.ID, newEntity);
        }

        //returns a pointer to the entity with the ID given as a parameter
        public BaseGameEntity GetEntityFromID(int id)
        {
            //find the entity
            BaseGameEntity ent = _entityMap[id];

            if (ent == null)
            {
                throw new ArgumentException("Invalid ID given");
            }

            return ent;
        }

        //this method removes the entity from the list
        public void RemoveEntity(BaseGameEntity entity)
        {
            _entityMap.Remove(entity.ID);
        }

        public static string GetNameOfEntity(EntityName name)
        {
            switch (name)
            {
                case EntityName.MinerBob:

                    return "Miner Bob";

                case EntityName.Elsa:

                    return "Elsa";

                default:

                    return "UNKNOWN!";
            }
        }

        public static string GetNameOfEntity(int n)
        {
            EntityName name = (EntityName)Enum.Parse(typeof(EntityName), n.ToString());
            return GetNameOfEntity(name);
        }
    }
}