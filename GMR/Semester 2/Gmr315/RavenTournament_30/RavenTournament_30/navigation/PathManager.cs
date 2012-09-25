using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.navigation
{
    public class PathManager : IDisposable
    {

public void Dispose()
{
    m_SearchRequests.Clear();
    m_SearchRequests = null;
}

        //a container of all the active search requests
        private List<Raven_PathPlanner> m_SearchRequests;

        //this is the total number of search cycles allocated to the manager. 
        //Each update-step these are divided equally amongst all registered path
        //requests
        private int m_iNumSearchCyclesPerUpdate;



        public PathManager(int NumCyclesPerUpdate)
        {
            m_iNumSearchCyclesPerUpdate = NumCyclesPerUpdate;
            m_SearchRequests = new List<Raven_PathPlanner>();
        }

        ///////////////////////////////////////////////////////////////////////////////
        //------------------------- UpdateSearches ------------------------------------
        //
        //  This method iterates through all the active path planning requests 
        //  updating their searches until the user specified total number of search
        //  cycles has been satisfied.
        //
        //  If a path is found or the search is unsuccessful the relevant agent is
        //  notified accordingly by Telegram
        //-----------------------------------------------------------------------------
        //every time this is called the total amount of search cycles available will
        //be shared out equally between all the active path requests. If a search
        //completes successfully or fails the method will notify the relevant bot
        public void UpdateSearches()
        {
            if (m_SearchRequests.Count == 0) return;

            int NumCyclesRemaining = m_iNumSearchCyclesPerUpdate;

            //iterate through the search requests until either all requests have been
            //fulfilled or there are no search cycles remaining for this update-step.
            int iteratorLocation = 0;
            
            while (NumCyclesRemaining-- > 0 && m_SearchRequests.Count != 0)
            {
                Raven_PathPlanner curPath = m_SearchRequests[iteratorLocation];
                //make one search cycle of this path request
                int result = curPath.CycleOnce();

                //if the search has terminated remove from the list
                if ((result == (int)TimeSliceResult.target_found) || (result == (int)TimeSliceResult.target_not_found))
                {
                    m_SearchRequests.Remove(curPath);
                    if (iteratorLocation == m_SearchRequests.Count)
                    {
                        iteratorLocation = 0;
                    }
                    //remove this path from the path list
                }
                //move on to the next
                else
                {
                    ++iteratorLocation;
                    if (iteratorLocation == m_SearchRequests.Count)
                    {
                        iteratorLocation = 0;
                    }
                }



            } //end while

        }

        //--------------------------- Register ----------------------------------------
        //
        //  this is called to register a search with the manager.
        //-----------------------------------------------------------------------------
        //a path planner should call this method to register a search with the 
        //manager. (The method checks to ensure the path planner is only registered
        //once)
        public void Register(Raven_PathPlanner pPathPlanner)
        {
            if (!m_SearchRequests.Contains(pPathPlanner))
            {
                //add to the list
                m_SearchRequests.Add(pPathPlanner);
            }
        }

        public void UnRegister(Raven_PathPlanner pPathPlanner)
        {
            m_SearchRequests.Remove(pPathPlanner);

        }

        //returns the amount of path requests currently active.
        public int GetNumActiveSearches() { return m_SearchRequests.Count(); }



    }
}
