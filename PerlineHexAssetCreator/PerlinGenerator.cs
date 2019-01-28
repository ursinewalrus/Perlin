using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinGenerator
{
    public static class PerlinGenerator
    {

        private static List<int> permutation = new List<int>() {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        static PerlinGenerator()
        {
            permutation.AddRange(new List<int>(permutation));
        }

        public static int[,] Map3DNoiseArrayToImage(int gradients, double[,,] noiseVals)
        {
            int maxColor = 255;
            double gradientFractions = 1.0 / (double)gradients;

            var height = noiseVals.GetLength(0);
            var width = noiseVals.GetLength(1);
            var depth = noiseVals.GetLength(2);
            int[,] colorVals = new int[height, width];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    for (int d = 0; d < depth; d++)
                    {
                        double noiseVal = noiseVals[h, w, d];

                        var colorSnapTo = Math.Floor(noiseVal / gradientFractions);
                        //int noiseColor = (int)(maxColor * Perlin.Fade(colorSnapTo * gradientFractions));
                        int noiseColor = (int)(maxColor * colorSnapTo * gradientFractions);
                        //maybe set something so you can just do r,g,b or transparency
                        colorVals[h, w] = noiseColor;

                    }
                }
            }
            return colorVals;
        }
        //basically gens a greyscale, give arg to change that
        //http://developer.download.nvidia.com/books/HTML/gpugems/gpugems_ch05.html
        public static double[,,] GenerateNoiseDimensions(int height = 100, int width = 100, int depth = 1, int octaves = 1, double persistence = .25, double frequency = 1, double amplitude = 1)
        {
            double[,,] noiseDims = new double[height, width, depth];
            for (int h = 1; h <= height; h++)
            {
                for (int w = 1; w <= width; w++)
                {
                    for (int d = 1; d <= depth; d++)
                    {
                        var noiseTotal = 0.0;
                        var maxNoiseTotal = 0.0;
                        var freq = frequency;
                        var amp = amplitude;
                        for (int o = 0; o < octaves; o++)
                        {
                            noiseTotal +=
                               GetNoiseValue((double)h / (double)height * freq,
                                              (double)w / (double)width * freq,
                                              (double)d / (double)depth * freq) * amp;
                            maxNoiseTotal += amp;
                            amp *= persistence;
                            freq *= 2;


                        }
                        noiseDims[h - 1, w - 1, d - 1] = noiseTotal / maxNoiseTotal;

                    }
                }
            }
            return noiseDims;
        }

        static double GetNoiseValue(double x, double y, double z)
        {

            //upper, leftmost cord
            int cubeX = (int)x % 255;
            int cubeY = (int)y % 255;
            int cubeZ = (int)z % 255;

            //location in cube
            var xf = x - (int)x;
            var yf = y - (int)y;
            var zf = z - (int)z;

            //for lerp
            double xu = Fade(xf);
            double yv = Fade(yf);
            double zw = Fade(zf);

            var x1 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY, cubeZ), xf, yf, zf),
                        PerlinGradient(
                            PerlinHash(cubeX + 1, cubeY, cubeZ), xf - 1, yf, zf),
                        xu);
            var x2 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY + 1, cubeZ), xf, yf - 1, zf),
                        PerlinGradient(
                            PerlinHash(cubeX + 1, cubeY + 1, cubeZ), xf - 1, yf - 1, zf),
                        xu);
            var y1 = Lerp(x1, x2, yv);

            x1 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY, cubeZ + 1), xf, yf, zf - 1),
                        PerlinGradient(
                            PerlinHash(cubeX + 1, cubeY, cubeZ + 1), xf - 1, yf, zf - 1),
                        xu);
            x2 = Lerp(
            PerlinGradient(
                PerlinHash(cubeX, cubeY + 1, cubeZ + 1), xf, yf - 1, zf - 1),
            PerlinGradient(
                PerlinHash(cubeX + 1, cubeY + 1, cubeZ + 1), xf - 1, yf - 1, zf - 1),
            xu);

            var y2 = Lerp(x1, x2, yv);

            return (Lerp(y1, y2, zw) + 1) / 2;
        }

        static double Lerp(double a, double b, double x)
        {
            return a + x * (b - a);
        }

        //for smoothing, moves vales towards 0,1??
        public static double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static double FadeFifth(double t)
        {
            return 6 * (Math.Pow(t, 6) - 15 * Math.Pow(t, 4) + 3 * Math.Pow(t, 3));
        }
        static int PerlinHash(int x, int y, int z)
        {
            var step1 = permutation[x] + y;
            var step2 = permutation[step1];
            return permutation[step2] + z;
        }

        //hash is our random seed based on cube, x,y,z our position in the cube
        static double PerlinGradient(int hash, double x, double y, double z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }

        public static int[,] Multiply2dArray(int[,] array, double multiplier)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for(var j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = Math.Min(255,(int)(array[i, j] * multiplier));
                }
            }
            return array;
        }
    }
}
