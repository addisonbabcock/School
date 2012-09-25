using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FuzzySet_Singleton : FuzzySet
    {


        //the values that define the shape of this FLV
        private float m_dMidPoint;
        private float m_dLeftOffset;
        private float m_dRightOffset;



        public FuzzySet_Singleton(float mid,
                             float lft,
                             float rgt)
            : base(mid)
        {
            m_dMidPoint = mid;
            m_dLeftOffset = lft;
            m_dRightOffset = rgt;
        }

        //this method calculates the degree of membership for a particular value
        public override float CalculateDOM(float val)
        {
            if ((val >= m_dMidPoint - m_dLeftOffset) &&
             (val <= m_dMidPoint + m_dRightOffset))
            {
                return 1.0f;
            }

            //out of range of this FLV, return zero
            return 0.0f;

        }
    }
}
