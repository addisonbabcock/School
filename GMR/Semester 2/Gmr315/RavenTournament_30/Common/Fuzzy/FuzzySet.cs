using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public abstract class FuzzySet : IDisposable
    {
        public void Dispose()
        {
        }

        //this will hold the degree of membership of a given value in this set 
        protected float m_dDOM;

        //this is the maximum of the set's membership function. For instamce, if
        //the set is triangular then this will be the peak point of the triangular.
        //if the set has a plateau then this value will be the mid point of the 
        //plateau. This value is set in the constructor to avoid run-time
        //calculation of mid-point values.
        protected float m_dRepresentativeValue;



        public FuzzySet(float RepVal)
        {
            m_dDOM = 0.0f;
            m_dRepresentativeValue = RepVal;
        }

        //return the degree of membership in this set of the given value. NOTE,
        //this does not set m_dDOM to the DOM of the value passed as the parameter.
        //This is because the centroid defuzzification method also uses this method
        //to determine the DOMs of the values it uses as its sample points.
        public abstract float CalculateDOM(float val);

        //if this fuzzy set is part of a consequent FLV, and it is fired by a rule 
        //then this method sets the DOM (in this context, the DOM represents a
        //confidence level)to the maximum of the parameter value or the set's 
        //existing m_dDOM value
        public void ORwithDOM(float val) { if (val > m_dDOM) m_dDOM = val; }

        //accessor methods
        public float GetRepresentativeVal() { return m_dRepresentativeValue; }

        public void ClearDOM() { m_dDOM = 0.0f; }
        public float GetDOM() { return m_dDOM; }
        public void SetDOM(float val)
        {
            if (val > 1 || val < 0)
            {
                throw new Exception("invalid value");
            }
            m_dDOM = val;
        }
    }
}
