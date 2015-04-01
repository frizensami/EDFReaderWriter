using System;

namespace EDFLibrary.EDFData.Generator
{
    public static class FakeDataGenerator
    {
        /// <summary>
        /// Generator that just returns a random list of numbered samples from int16.min to int16.max if you need to simulate a signal
        /// </summary>
        /// <param name="numSamples"></param>
        /// <returns></returns>
        public static Int16[] generateData(int numSamples)
        {
            Int16[] data = new Int16[numSamples];
            Int16 x = Int16.MinValue;
            bool upwards = true;
            for (int i = 0; i < numSamples; i++)
            {
                if (x == Int16.MaxValue)
                    upwards = false;

                if (x == Int16.MinValue)
                    upwards = true;

                if (upwards)
                    x++;
                else
                    x--;

                data[i] = x;
            }

                return data;
        }
    }
}
