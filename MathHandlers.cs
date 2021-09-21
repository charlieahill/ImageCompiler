using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompiler
{
    public static class MathHandlers
    {
        public static int Maximum(int[] input)
        {
            int maximum = input[0];

            foreach (int item in input)
                if (item > maximum)
                    maximum = item;

            return maximum;
        }
        public static int Minimum(int[] input)
        {
            int minimum = input[0];

            foreach (int item in input)
                if (item < minimum)
                    minimum = item;

            return minimum;
        }
    }
}
