﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public class FuzzyRule : IDisposable
    {
        
        public void Dispose()
        {
            m_pAntecedent.Dispose();
            m_pConsequence.Dispose();
        }

        //antecedent (usually a composite of several fuzzy sets and operators)
  private FuzzyTerm  m_pAntecedent;

  //consequence (usually a single fuzzy set, but can be several ANDed together)
  private FuzzyTerm        m_pConsequence;

  //it doesn't make sense to allow clients to copy rules
  //private FuzzyRule(const FuzzyRule&);
  //FuzzyRule& operator=(const FuzzyRule&);




public   FuzzyRule(FuzzyTerm ant,
            FuzzyTerm con){
    m_pAntecedent = ant.Clone();
    m_pConsequence = con.Clone();
  }


  public void SetConfidenceOfConsequentToZero()
  {
      m_pConsequence.ClearDOM();
  }

  //this method updates the DOM (the confidence) of the consequent term with
  //the DOM of the antecedent term. 
  public void Calculate()
  {
    m_pConsequence.ORwithDOM(m_pAntecedent.GetDOM());
  }
    }
}