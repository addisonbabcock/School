using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_22_Exercise3
{
    class PayCalc
    {
        public static double CalculateGrossPay(double hours, double rate)
        {
            double grossPay = 0.0;

            if (hours > 40.0)
            {
                if (hours > 50.0)
                {
                    grossPay = 40.0 * rate + 10.0 * rate * 1.5 + ((hours - 50) * rate * 2.0);
                }
                else
                {
                    grossPay = 40.0 * rate + (hours - 40.0) * rate * 1.5;
                }
            }
            else
            {
                grossPay = hours * rate;
            }

            return grossPay;
        }
    }
}
