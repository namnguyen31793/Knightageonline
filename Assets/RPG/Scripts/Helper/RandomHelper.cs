using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnightAge
{
    public class RandomHelper
    {
        protected static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static int NextByte(int value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            byte[] randomNumber = new byte[8];
            rngCsp.GetBytes(randomNumber);
            return Math.Abs(randomNumber[0] % value);
        }

        public static int NextInt(int value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            byte[] randomNumber = new byte[8];
            rngCsp.GetBytes(randomNumber);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0) % value);
        }

        public static long NextLong(long value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            byte[] randomNumber = new byte[8];
            rngCsp.GetBytes(randomNumber);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(randomNumber);
            return Math.Abs(BitConverter.ToInt64(randomNumber, 0) % value);
        }

        public static float NextFloat(float low, float high)
        {
            if (high <= low) return high;
            Random rand = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            double value = rand.NextDouble() * (high - low) + low;
            return (float)value;
        }

        public static int NextInt(int low, int high)
        {
            if (high <= low) return high;
            Random rand = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            int value = rand.Next() * (high - low) + low;
            return value;
        }

        public static bool IsDuplicated(IEnumerable<int> collection)
        {
            if (collection == null || collection.Count() < 2) return false;
            var distinctCards = collection.Distinct();
            return distinctCards.Count() < collection.Count();
        }
    }

}
