#region Using

using System;
using Common.Misc;

#endregion

namespace Common.Time
{
    public class Regulator : IDisposable
    {
        //the time period between updates 
        private readonly float m_dUpdatePeriod;

        //the next time the regulator allows code flow
        private long m_dwNextUpdateTime;


        public Regulator(float NumUpdatesPerSecondRqd)
        {
            double seconds = Utils.RandFloat();

            m_dwNextUpdateTime = DateTime.Now.AddSeconds(seconds).Ticks;

            if (NumUpdatesPerSecondRqd > 0)
            {
                m_dUpdatePeriod = 1000.0f/NumUpdatesPerSecondRqd;
            }

            else if (NumUpdatesPerSecondRqd.Equals(0.0))
            {
                m_dUpdatePeriod = 0.0f;
            }

            else if (NumUpdatesPerSecondRqd < 0)
            {
                m_dUpdatePeriod = -1;
            }
        }


        //returns true if the current time exceeds m_dwNextUpdateTime

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        public bool isReady()
        {
            //if a regulator is instantiated with a zero freq then it goes into
            //stealth mode (doesn't regulate)
            if (m_dUpdatePeriod.Equals(0.0)) return true;

            //if the regulator is instantiated with a negative freq then it will
            //never allow the code to flow
            if (m_dUpdatePeriod < 0) return false;

            long CurrentTime = DateTime.Now.Ticks;

            //the number of milliseconds the update period can vary per required
            //update-step. This is here to make sure any multiple clients of this class
            //have their updates spread evenly
            const float UpdatePeriodVariator = 10.0f;

            if (CurrentTime >= m_dwNextUpdateTime)
            {
                double seconds = Utils.RandInRange(-UpdatePeriodVariator, UpdatePeriodVariator) + m_dUpdatePeriod;
                m_dwNextUpdateTime =
                     DateTime.Now.AddMilliseconds(seconds).Ticks;

                return true;
            }

            return false;
        }
    }
}