using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_22_Exercise1
{
    class CalculateChange
    {
        private int mPennies;
        private int mNickels;
        private int mDimes;
        private int mQuarters;

        public CalculateChange(
            int _p, int _n,
            int _d, int _q)
        {
            mPennies = _p;
            mNickels = _n;
            mDimes = _d;
            mQuarters = _q;
        }

        public int CalculateTotalChange()
        {
            int totalAmount = 0;

            totalAmount += mPennies;
            totalAmount += mNickels * 5;
            totalAmount += mDimes * 10;
            totalAmount += mQuarters * 25;

            return totalAmount;
        }
    }
}
