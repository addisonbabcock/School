using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FzSet : FuzzyTerm
    {
        internal FuzzySet m_Set;

        public FzSet(FuzzySet fs)
        {

            m_Set = fs;
        }

        public override FuzzyTerm Clone() { return new FzSet(m_Set); }
        public override float GetDOM() { return m_Set.GetDOM(); }
        public override void ClearDOM() { m_Set.ClearDOM(); }
        public override void ORwithDOM(float val) { m_Set.ORwithDOM(val); }
    }
}
