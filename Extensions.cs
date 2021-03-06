using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame2
{
    static class Extensions
    {
        /// <summary>
        /// this function takes a vector2 and returns the angle as radians of the vector 2
        /// with 0,0 as the top left corner, the x value along the horizontal axis
        /// and the y value along the vertical axis. The angle is angle of the line that forms between
        /// 0,0 and the interection between x and y lines
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float ToAngleCar(this Vector2 vector)
        {
            return (float)Math.Atan2(-vector.Y, vector.X);
        }

        /// <summary>
        /// returns a float between a given min and max value
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static float NextFloat(this Random rand, float minValue, float maxValue)
        {
            // generate a random double between 0 and 1 then convert it to a float and multiply by
            // the difference between the max and min values to get a delta that lies within the min and
            // max values then add the minvalue to get a number that lies within the 2 values
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }

        // scales a vector to a given length
        public static Vector2 ScaleTo(this Vector2 vector, float length)
        {
            // multiplying the given vector by the fraction of it's length
            return vector * (length / vector.Length());
        }
    }
}
