using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FzAND : FuzzyTerm
    {


        //an instance of this class may AND together up to 4 terms
        private List<FuzzyTerm> m_Terms;



        public new void Dispose()
        {
            foreach (FuzzyTerm curTerm in m_Terms)
            {
                curTerm.Dispose();
            }
            m_Terms.Clear();
            m_Terms = null;

        }

        //copy ctor
        public FzAND(FzAND fa)
        {
            m_Terms = new List<FuzzyTerm>();
            foreach (FuzzyTerm fuzzyTerms in fa.m_Terms)
            {
                m_Terms.Add(fuzzyTerms.Clone());
            }

        }

        //ctors accepting fuzzy terms.
        public FzAND(ref FuzzyTerm op1, ref FuzzyTerm op2)
        {
            m_Terms = new List<FuzzyTerm>();
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
        }
        public FzAND(ref FuzzyTerm op1, ref FuzzyTerm op2, ref FuzzyTerm op3)
        {
            m_Terms = new List<FuzzyTerm>();
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
            m_Terms.Add(op3.Clone());
        }
        public FzAND(ref FuzzyTerm op1, ref FuzzyTerm op2, ref FuzzyTerm op3, ref FuzzyTerm op4)
        {
            m_Terms = new List<FuzzyTerm>();
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
            m_Terms.Add(op3.Clone());
            m_Terms.Add(op4.Clone());
        }

        //virtual ctor
        public override FuzzyTerm Clone() { return new FzAND(this); }

        public override float GetDOM()
        {
            float smallest = float.MaxValue;

            foreach (FuzzyTerm curTerm in m_Terms)
            {
                if (curTerm.GetDOM() < smallest)
                {
                    smallest = curTerm.GetDOM();
                }
            }

            return smallest;
        }

        public override void ClearDOM()
        {
            foreach (FuzzyTerm curTerm in m_Terms)
            {
                curTerm.ClearDOM();
            }
        }

        public override void ORwithDOM(float val)
        {
            foreach (FuzzyTerm curTerm in m_Terms)
            {
                curTerm.ORwithDOM(val);
            }

        }
    }
}
