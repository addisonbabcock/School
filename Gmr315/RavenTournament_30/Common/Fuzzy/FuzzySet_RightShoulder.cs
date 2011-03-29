using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Misc;

namespace Common.Fuzzy
{
    class FuzzySet_RightShoulder : FuzzySet
    {


        //the values that define the shape of this FLV
        private float m_dPeakPoint;
        private float m_dLeftOffset;
        private float m_dRightOffset;



        public FuzzySet_RightShoulder(float peak,
                                 float LeftOffset,
                                 float RightOffset) :

            base(((peak + RightOffset) + peak) / 2)
        {
            m_dPeakPoint = peak;
            m_dLeftOffset = LeftOffset;
            m_dRightOffset = RightOffset;

        }

        //this method calculates the degree of membership for a particular value
        public override float CalculateDOM(float val)
        {
            //test for the case where the left or right offsets are zero
            //(to prevent divide by zero errors below)
            if ((Utils.isEqual(m_dRightOffset, 0.0) && (Utils.isEqual(m_dPeakPoint, val))) ||
                (Utils.isEqual(m_dLeftOffset, 0.0) && (Utils.isEqual(m_dPeakPoint, val))))
            {
                return 1.0f;
            }

            //find DOM if left of center
            if ((val <= m_dPeakPoint) && (val > (m_dPeakPoint - m_dLeftOffset)))
            {
                float grad = 1.0f / m_dLeftOffset;

                return grad * (val - (m_dPeakPoint - m_dLeftOffset));
            }
            //find DOM if right of center and less than center + right offset
            if ((val > m_dPeakPoint) && (val <= m_dPeakPoint + m_dRightOffset))
            {
                return 1.0f;
            }


            return 0f;

        }
    }
}
