using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FzVery : FuzzyTerm
    {
        private FuzzySet m_Set;

        public FzVery(FzSet ft)
        {

            m_Set = ft.m_Set;
        }

        public override float GetDOM()
        {
            return m_Set.GetDOM() * m_Set.GetDOM();
        }

        public override FuzzyTerm Clone() { return new FzVery(new FzSet(m_Set)); }

        public override void ClearDOM() { m_Set.ClearDOM(); }
        public override void ORwithDOM(float val) { m_Set.ORwithDOM(val * val); }
    }
}
