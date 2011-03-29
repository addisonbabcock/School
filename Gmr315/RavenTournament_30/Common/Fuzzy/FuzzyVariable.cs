using System;
using System.IO;
using Common.Misc;
using MemberSets = System.Collections.Generic.Dictionary<string, Common.Fuzzy.FuzzySet>;

namespace Common.Fuzzy
{
    public class FuzzyVariable : IDisposable
    {
        private float m_dMaxRange;
        private float m_dMinRange;
        private MemberSets m_MemberSets;


        //this method is called with the upper and lower bound of a set each time a
        //new set is added to adjust the upper and lower range values accordingly

        public FuzzyVariable()
        {
            m_dMinRange = 0.0f;
            m_dMaxRange = 0.0f;
            m_MemberSets = new MemberSets();
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (FuzzySet set in m_MemberSets.Values)
            {
                set.Dispose();
            }
            m_MemberSets.Clear();
            m_MemberSets = null;
        }

        #endregion

        private void AdjustRangeToFit(float min, float max)
        {
            if (min < m_dMinRange) m_dMinRange = min;
            if (max > m_dMaxRange) m_dMaxRange = max;
        }

        //the following methods create instances of the sets named in the method
        //name and add them to the member set map. Each time a set of any type is
        //added the m_dMinRange and m_dMaxRange are adjusted accordingly. All of the
        //methods return a proxy class representing the newly created instance. This
        //proxy set can be used as an operand when creating the rule base.
        public FzSet AddLeftShoulderSet(string name, float minBound, float peak, float maxBound)
        {
            m_MemberSets[name] = new FuzzySet_LeftShoulder(peak, peak - minBound, maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);
        }

        public FzSet AddRightShoulderSet(string name, float minBound, float peak, float maxBound)
        {
            m_MemberSets[name] = new FuzzySet_RightShoulder(peak, peak - minBound, maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);
        }

        public FzSet AddTriangularSet(string name,
                                      float minBound,
                                      float peak,
                                      float maxBound)
        {
            m_MemberSets[name] = new FuzzySet_Triangle(peak,
                                                       peak - minBound,
                                                       maxBound - peak);
            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);
        }


        public FzSet AddSingletonSet(string name,
                                     float minBound,
                                     float peak,
                                     float maxBound)
        {
            m_MemberSets[name] = new FuzzySet_Singleton(peak,
                                                        peak - minBound,
                                                        maxBound - peak);

            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);
        }


        //fuzzify a value by calculating its DOM in each of this variable's subsets
        //--------------------------- Fuzzify -----------------------------------------
        //
        //  takes a crisp value and calculates its degree of membership for each set
        //  in the variable.
        //-----------------------------------------------------------------------------
        public void Fuzzify(float val)
        {
            if (val < m_dMinRange || val > m_dMaxRange)
            {
                throw new ArgumentOutOfRangeException("val", val, "value out of range");
            }

            //for each set in the flv calculate the DOM for the given value
            foreach (FuzzySet curSet in m_MemberSets.Values)
            {
                curSet.SetDOM(curSet.CalculateDOM(val));
            }
        }


        //defuzzify the variable using the max average method
        //--------------------------- DeFuzzifyMaxAv ----------------------------------
        //
        // defuzzifies the value by averaging the maxima of the sets that have fired
        //
        // OUTPUT = sum (maxima * DOM) / sum (DOMs) 
        //-----------------------------------------------------------------------------
        public float DeFuzzifyMaxAv()
        {
            float bottom = 0.0f;
            float top = 0.0f;


            foreach (FuzzySet curSet in m_MemberSets.Values)
            {
                bottom += curSet.GetDOM();

                top += curSet.GetRepresentativeVal()*curSet.GetDOM();
            }

            //make sure bottom is not equal to zero
            if (Utils.isEqual(0, bottom)) return 0.0f;

            return top/bottom;
        }

        //defuzzify the variable using the centroid method
        public float DeFuzzifyCentroid(int NumSamples)
        {
            //calculate the step size
            float StepSize = (m_dMaxRange - m_dMinRange)/NumSamples;

            float TotalArea = 0.0f;
            float SumOfMoments = 0.0f;

            //step through the range of this variable in increments equal to StepSize
            //adding up the contribution (lower of CalculateDOM or the actual DOM of this
            //variable's fuzzified value) for each subset. This gives an approximation of
            //the total area of the fuzzy manifold.(This is similar to how the area under
            //a curve is calculated using calculus... the heights of lots of 'slices' are
            //summed to give the total area.)
            //
            //in addition the moment of each slice is calculated and summed. Dividing
            //the total area by the sum of the moments gives the centroid. (Just like
            //calculating the center of mass of an object)
            for (int samp = 1; samp <= NumSamples; ++samp)
            {
                //for each set get the contribution to the area. This is the lower of the 
                //value returned from CalculateDOM or the actual DOM of the fuzzified 
                //value itself   
                foreach (FuzzySet curSet in m_MemberSets.Values)
                {
                    float contribution =
                        Math.Min(curSet.CalculateDOM(m_dMinRange + samp*StepSize),
                                 curSet.GetDOM());

                    TotalArea += contribution;

                    SumOfMoments += (m_dMinRange + samp*StepSize)*contribution;
                }
            }

            //make sure total area is not equal to zero
            if (Utils.isEqual(0, TotalArea)) return 0.0f;

            return (SumOfMoments/TotalArea);
        }


        public void WriteDOMs(StreamWriter os)
        {
            foreach (string key in m_MemberSets.Keys)
            {
                os.WriteLine(key + " is " + m_MemberSets[key].GetDOM());
            }

            os.WriteLine("Min Range: " + m_dMinRange);
            os.WriteLine("Max Range: " + m_dMaxRange);
        }
    }
}