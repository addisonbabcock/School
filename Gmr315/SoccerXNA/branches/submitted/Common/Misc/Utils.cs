#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Common.Misc
{
    public static class Utils
    {
        private static readonly Random _random = new Random((int) DateTime.Now.Ticks);
        public static float _gaussianStatic;
        public static bool _useLastForRandomGaussian;

        //returns true if the value is a NaN
        public static bool IsNaN(float val)
        {
            return val != float.NaN;
        }

        public static float DegsToRads(float degs)
        {
            return MathHelper.TwoPi*(degs/360.0f);
        }


        //returns true if the parameter is equal to zero
        public static bool IsZero(float val)
        {
            return ((-float.MinValue < val) && (val < float.MinValue));
        }

        //returns true is the third parameter is in the range described by the
        //first two
        public static bool InRange(float start, float end, float val)
        {
            if (start < end)
            {
                if ((val > start) && (val < end))
                {
                    return true;
                }
                return false;
            }

            if ((val < start) && (val > end))
            {
                return true;
            }
            return false;
        }

        //----------------------------------------------------------------------------
        //  some random number functions.
        //----------------------------------------------------------------------------

        //returns a random integer between x and y
        public static int RandInt(int x, int y)
        {
            return _random.Next()%(y - x + 1) + x;
        }

        //returns a random float between zero and 1
        public static float RandFloat()
        {
            return (float) _random.NextDouble();
        }

        public static float RandInRange(float x, float y)
        {
            return (float) (x + _random.NextDouble()*(y - x));
        }

        //returns a random bool
        public static bool RandBool()
        {
            if (RandInt(0, 1) == 1) return true;

            return false;
        }

        //returns a random float in the range -1 < n < 1
        public static float RandomClamped()
        {
            return (float) (_random.NextDouble() - _random.NextDouble());
        }


        //returns a random number with a normal distribution. See method at
        //http://www.taygeta.com/random/gaussian.html
        public static float RandGaussian()
        {
            return RandGaussian(0.0f, 1.0f);
        }

        public static float RandGaussian(float mean, float standard_deviation)
        {
            float y1;

            if (_useLastForRandomGaussian) /* use value from previous call */
            {
                y1 = _gaussianStatic;
                _useLastForRandomGaussian = false;
            }
            else
            {
                float w, x1, x2;
                do
                {
                    x1 = 2.0f*RandFloat() - 1.0f;
                    x2 = 2.0f*RandFloat() - 1.0f;
                    w = x1*x1 + x2*x2;
                } while (w >= 1.0);

                w = (float) Math.Sqrt((-2.0f*Math.Log(w))/w);
                y1 = x1*w;
                _gaussianStatic = x2*w;
                _useLastForRandomGaussian = true;
            }

            return (mean + y1*standard_deviation);
        }


        //-----------------------------------------------------------------------
        //  
        //  some handy little functions
        //-----------------------------------------------------------------------


        public static float Sigmoid(float input)
        {
            return Sigmoid(input, 1.0f);
        }

        public static float Sigmoid(float input, float response)
        {
            return (1.0f/(1.0f + (float) Math.Exp(-input/response)));
        }

        public static void Clamp(ref float arg, float minVal, float maxVal)
        {
            arg = Clamp(arg, minVal, maxVal);
        }
        public static float Clamp(float arg, float minVal, float maxVal)
        {
            if (minVal >= maxVal)
            {
                throw new Exception("Original code assertion: MaxVal < MinVal!");
            }
            if (arg < minVal)
            {
                return minVal;
            }
            if (arg > maxVal)
            {
                return maxVal;
            }
            return arg;
        }


        //rounds a float up or down depending on its value
        public static int Rounded(float val)
        {
            int integral = (int) val;
            float mantissa = val - integral;

            if (mantissa < 0.5)
            {
                return integral;
            }


            return integral + 1;
        }

        //rounds a float up or down depending on whether its 
        //mantissa is higher or lower than offset
        public static int RoundUnderOffset(float val, float offset)
        {
            int integral = (int) val;
            float mantissa = val - integral;

            if (mantissa < offset)
            {
                return integral;
            }


            return integral + 1;
        }

        //compares two real numbers. Returns true if they are equal
        public static bool isEqual(float a, float b)
        {
            if (Math.Abs(a - b) < 1E-12)
            {
                return true;
            }

            return false;
        }

        public static bool isEqual(double a, double b)
        {
            if (Math.Abs(a - b) < 1E-12)
            {
                return true;
            }

            return false;
        }

        public static float Average(List<float> v)
        {
            float average = 0.0f;
            foreach (float value in v)
            {
                average += value;
            }
            return average/v.Count;
        }

        public static float StandardDeviation(List<float> v)
        {
            float sd = 0.0f;
            float average = Average(v);

            foreach (float value in v)
            {
                sd += (value - average)*(value - average);
            }

            sd = sd/v.Count;

            return (float) Math.Sqrt(sd);
        }
    }
}