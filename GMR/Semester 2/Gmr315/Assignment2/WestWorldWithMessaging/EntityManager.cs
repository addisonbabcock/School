#region Using

using System;
using System.Collections.Generic;

#endregion

namespace WestWorldWithMessaging
{
    public class EntityManager
    {
        //to facilitate quick lookup the entities are stored in a std::map, in which
        //pointers to entities are cross referenced by their identifying number
        private static EntityManager instance;
        private readonly Dictionary<int, BaseGameEntity> m_EntityMap;


        private EntityManager()
        {
            m_EntityMap = new Dictionary<int, BaseGameEntity>();
        }


        public static EntityManager Instance()
        {
            if (instance == null)
            {
                instance = new EntityManager();
            }
            return instance;
        }

        //this method stores a pointer to the entity in the std::vector
        //m_Entities at the index position indicated by the entity's ID
        //(makes for faster access)
        public void RegisterEntity(BaseGameEntity NewEntity)
        {
            m_EntityMap.Add(NewEntity.ID(), NewEntity);
        }

        //returns a pointer to the entity with the ID given as a parameter
        public BaseGameEntity GetEntityFromID(int id)
        {
            //find the entity
            BaseGameEntity ent = m_EntityMap[id];

            if (ent == null)
            {
                throw new ArgumentException("Invalid ID given");
            }

            return ent;
        }

        //this method removes the entity from the list
        public void RemoveEntity(BaseGameEntity pEntity)
        {
            m_EntityMap.Remove(pEntity.ID());
        }
    }
}