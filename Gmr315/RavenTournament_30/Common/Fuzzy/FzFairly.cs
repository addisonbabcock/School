using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FzFairly : FuzzyTerm
    {


        private FuzzySet m_Set;




        public FzFairly(FzSet ft)
        {
            m_Set = ft.m_Set;
        }

        public override float GetDOM()
        {
            return (float)Math.Sqrt(m_Set.GetDOM());
        }

        public override FuzzyTerm Clone() { return new FzFairly(new FzSet(m_Set)); }

        public override void ClearDOM() { m_Set.ClearDOM(); }
        public override void ORwithDOM(float val) { m_Set.ORwithDOM((float)Math.Sqrt(val)); }
    }
}
