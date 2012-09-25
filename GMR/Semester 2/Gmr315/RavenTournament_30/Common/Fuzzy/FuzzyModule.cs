using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VarMap = System.Collections.Generic.Dictionary<string, Common.Fuzzy.FuzzyVariable>;
namespace Common.Fuzzy
{
    public class FuzzyModule
    {

        public FuzzyModule()
        {
            m_Variables = new VarMap();
            m_Rules = new List<FuzzyRule>();
        }

        //you must pass one of these values to the defuzzify method. This module
        //only supports the MaxAv and centroid methods.
        public enum DefuzzifyMethod { max_av, centroid };

        //when calculating the centroid of the fuzzy manifold this value is used
        //to determine how many cross-sections should be sampled
        public const int NumSamples = 15;



        //a map of all the fuzzy variables this module uses
        private VarMap m_Variables;

        //a vector containing all the fuzzy rules
        private List<FuzzyRule> m_Rules;


        //zeros the DOMs of the consequents of each rule. Used by Defuzzify()
        private void SetConfidencesOfConsequentsToZero()
        {
            foreach (FuzzyRule curRule in m_Rules)
            {
                curRule.SetConfidenceOfConsequentToZero();
            }
        }




        ~FuzzyModule()
        {
            foreach (FuzzyVariable curVar in m_Variables.Values)
            {
                curVar.Dispose();
            }
            m_Variables.Clear();
            m_Variables = null;

            foreach (FuzzyRule curRule in m_Rules)
            {
                curRule.Dispose();
            }
            m_Rules.Clear();
            m_Rules = null;
        }

        //creates a new 'empty' fuzzy variable and returns a reference to it.
        public FuzzyVariable CreateFLV(string VarName)
        {
            m_Variables[VarName] = new FuzzyVariable(); ;

            return m_Variables[VarName];
        }

        //adds a rule to the module
        public void AddRule(ref FuzzyTerm antecedent, ref FuzzyTerm consequence)
        {
            m_Rules.Add(new FuzzyRule(antecedent, consequence));
        }

        //this method calls the Fuzzify method of the named FLV 
        public void Fuzzify(string NameOfFLV, float val)
        {
            if (!m_Variables.ContainsKey(NameOfFLV))
            {
                throw new ArgumentException("Key Not found", "NameOfFLV");
            }

            m_Variables[NameOfFLV].Fuzzify(val);
        }


        //given a fuzzy variable and a deffuzification method this returns a 
        //crisp value
        public float DeFuzzify(string key)
        {
            return DeFuzzify(key, DefuzzifyMethod.max_av);
        }
        public float DeFuzzify(string key,
                                  DefuzzifyMethod method)
        {
            if (!m_Variables.ContainsKey(key))
            {
                throw new ArgumentException("Key Not found", "key");
            }


            //clear the DOMs of all the consequents of all the rules
            SetConfidencesOfConsequentsToZero();

            //process the rules
            foreach (FuzzyRule curRule in m_Rules)
            {
                curRule.Calculate();
            }

            //now defuzzify the resultant conclusion using the specified method
            switch (method)
            {
                case DefuzzifyMethod.centroid:

                    return m_Variables[key].DeFuzzifyCentroid(NumSamples);

                case DefuzzifyMethod.max_av:

                    return m_Variables[key].DeFuzzifyMaxAv();
            }

            return 0;
        }


        //writes the DOMs of all the variables in the module to an output stream
        public void WriteAllDOMs(StreamWriter os)
        {
            os.WriteLine();
            os.WriteLine();
            foreach (string key in m_Variables.Keys)
            {
                os.WriteLine("--------------------------- ");
                os.WriteLine(key);
                m_Variables[key].WriteDOMs(os);
                os.WriteLine();
            }


        }
    }
}
