#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Common.Game
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


        //this method stores a pointer to the entity in theList
        //m_Entities at the index position indicated by the entity's ID
        //(makes for faster access)
        public void RegisterEntity(BaseGameEntity NewEntity)
        {
            m_EntityMap.Add(NewEntity.ID(), NewEntity);
        }

        //returns a pointer to the entity with the ID given as a parameter
        public BaseGameEntity GetEntityFromID(int id)
        {
            if (m_EntityMap.ContainsKey(id))
            {
                return m_EntityMap[id];
            }
            throw new ArgumentException("Invalid Id");
        }

        //this method removes the entity from the list
        public void RemoveEntity(BaseGameEntity pEntity)
        {
            if (m_EntityMap.ContainsKey(pEntity.ID()))
            {
                m_EntityMap.Remove(pEntity.ID());
            }
        }

        //clears all entities from the entity map
        public void Reset()
        {
            m_EntityMap.Clear();
        }
    }
}