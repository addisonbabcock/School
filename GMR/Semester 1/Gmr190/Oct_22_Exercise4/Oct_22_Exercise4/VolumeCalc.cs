using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_22_Exercise4
{
    class VolumeCalc
    {
        public static double CalcConeVolume(double radius, double height)
        {
            return CalcCylinderVolume(radius, height) / 3;
        }

        public static double CalcCylinderVolume(double radius, double height)
        {
            return Math.PI * radius * radius * height;
        }
    }
}
