using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Misc;

namespace Common.Fuzzy
{
    public class FuzzySet_LeftShoulder : FuzzySet
    {


        //the values that define the shape of this FLV
        private float m_dPeakPoint;
        private float m_dRightOffset;
        private float m_dLeftOffset;



        public FuzzySet_LeftShoulder(float peak,
                                     float LeftOffset,
                                     float RightOffset) :

            base(((peak - LeftOffset) + peak) / 2)
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

            //find DOM if right of center
            if ((val >= m_dPeakPoint) && (val < (m_dPeakPoint + m_dRightOffset)))
            {
                float grad = 1.0f / -m_dRightOffset;

                return grad * (val - m_dPeakPoint) + 1.0f;
            }

            //find DOM if left of center
            if ((val < m_dPeakPoint) && (val >= m_dPeakPoint - m_dLeftOffset))
            {
                return 1.0f;
            }

            //out of range of this FLV, return zero


            return 0.0f;


        }
    }
}
