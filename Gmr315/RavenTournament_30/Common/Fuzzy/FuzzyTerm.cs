using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Fuzzy
{
    public abstract class FuzzyTerm : IDisposable
    {
        public void Dispose()
        {
        }

        //all terms must implement a virtual constructor
        public abstract FuzzyTerm Clone();

  //retrieves the degree of membership of the term
        public abstract float GetDOM();

  //clears the degree of membership of the term
        public abstract void ClearDOM();

  //method for updating the DOM of a consequent when a rule fires
        public abstract void ORwithDOM(float val);
    }
}
