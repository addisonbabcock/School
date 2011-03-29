using System;
using Common._2D;
using Common.Game;
using Microsoft.Xna.Framework;

namespace Common.Triggers
{
    public abstract class Trigger<T> : BaseGameEntity, IDisposable where T : BaseGameEntity
    {
        /// <summary>
        ///it's convenient to be able to deactivate certain types of triggers
        ///on an event. Therefore a trigger can only be triggered when this
        ///value is true (respawning triggers make good use of this facility)
        /// </summary>
        private bool m_bActive;

        /// <summary>
        /// if this is true the trigger will be removed from the game
        /// </summary>
        private bool m_bRemoveFromGame;

        /// <summary>
        ///some types of trigger are twinned with a graph node. This enables
        ///the pathfinding component of an AI to search a navgraph for a specific
        ///type of trigger.
        /// </summary>
        private int m_iGraphNodeIndex;

        /// <summary>
        /// Every trigger owns a trigger region. If an entity comes within this 
        /// region the trigger is activated
        /// </summary>
        private TriggerRegion m_pRegionOfInfluence;

        public Trigger(int id) : base(id)
        {
            m_bRemoveFromGame = false;
            m_bActive = true;
            m_iGraphNodeIndex = -1;
            m_pRegionOfInfluence = null;
        }


        protected void SetGraphNodeIndex(int idx)
        {
            m_iGraphNodeIndex = idx;
        }

        protected void SetToBeRemovedFromGame()
        {
            m_bRemoveFromGame = true;
        }

        protected void SetInactive()
        {
            m_bActive = false;
        }

        protected void SetActive()
        {
            m_bActive = true;
        }

        //returns true if the entity given by a position and bounding radius is
        //overlapping the trigger region
        protected bool isTouchingTrigger(Vector2 EntityPos, float EntityRadius)
        {
            if (m_pRegionOfInfluence != null)
            {
                return m_pRegionOfInfluence.isTouching(EntityPos, EntityRadius);
            }

            return false;
        }

        //child classes use one of these methods to initialize the trigger region
        protected void AddCircularTriggerRegion(Vector2 center, float radius)
        {
            //if this replaces an existing region, tidy up memory
            if (m_pRegionOfInfluence != null)
            {
                m_pRegionOfInfluence.Dispose();
            }

            m_pRegionOfInfluence = new TriggerRegion_Circle(center, radius);
        }

        protected void AddRectangularTriggerRegion(Vector2 TopLeft, Vector2 BottomRight)
        {
            //if this replaces an existing region, tidy up memory
            if (m_pRegionOfInfluence != null)
            {
                m_pRegionOfInfluence.Dispose();
            }

            m_pRegionOfInfluence = new TriggerRegion_Rectangle(TopLeft, BottomRight);
        }


        ~Trigger()
        {
            m_pRegionOfInfluence.Dispose();
        }

        public abstract void Dispose();

        //when this is called the trigger determines if the entity is within the
        //trigger's region of influence. If it is then the trigger will be 
        //triggered and the appropriate action will be taken.
        public abstract void Try(T entity);

        public int GraphNodeIndex()
        {
            return m_iGraphNodeIndex;
        }

        public bool isToBeRemoved()
        {
            return m_bRemoveFromGame;
        }

        public bool isActive()
        {
            return m_bActive;
        }

        public abstract void Render(PrimitiveBatch batch);
    }
}