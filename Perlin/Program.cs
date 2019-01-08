using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//https://flafla2.github.io/2014/08/09/perlinnoise.html
namespace Perlin
{
    class Program
    {
        static void Main(string[] args)
        {
            var bm = new Bitmap(500, 500);
            List<double> noiseVals = new List<double>();

            var rand = new Random();
            for (var i = 0; i < 500; i++)
            {
                for (var j = 0; j < 500; j++)
                {
                    var wiggleX = .5; rand.NextDouble();
                    var wiggleY = .5; rand.NextDouble();
                    var noise = new Perlin(i + wiggleX, j + wiggleY, 1).NoiseValue;
                    int maxColor = 255;// 16777215;
                    int noiseColor = (int)(maxColor* ((noise)>.5?0:1));
                    //var noiseHexColor = int.Parse((0xFF + noiseColor).ToString("X"),System.Globalization.NumberStyles.HexNumber);
                    bm.SetPixel(i,j,Color.FromArgb(125, noiseColor, noiseColor, noiseColor));

                }
            }
            
            bm.Save("test.png");
            ;
        }
    }

    public class Perlin
    {

        private List<int> permutation = new List<int>() {
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

        public double NoiseValue;
        public Perlin(double x, double y, double z)
        {
            permutation.AddRange(new List<int>(permutation));

            //upper, leftmost cord
            int cubeX = (int) x % 255;
            int cubeY = (int) y % 255;
            int cubeZ = (int) z % 255;

            //location in cube
            var xf = x - (int) x;
            var yf = y - (int) y;
            var zf = z - (int) z;

            //for lerp
            double xu = fade(xf);
            double yv = fade(yf);
            double zw = fade(zf);

            var x1 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY, cubeZ), xf,yf,zf), 
                        PerlinGradient(
                            PerlinHash(cubeX+1, cubeY, cubeZ),xf - 1,yf,zf), 
                        xu);
            var x2 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY+1, cubeZ), xf, yf -1, zf),
                        PerlinGradient(
                            PerlinHash(cubeX + 1, cubeY+1, cubeZ), xf - 1, yf -1, zf),
                        xu);
            var y1 = Lerp(x1, x2, yv);

            x1 = Lerp(
                        PerlinGradient(
                            PerlinHash(cubeX, cubeY, cubeZ + 1), xf, yf, zf -1),
                        PerlinGradient(
                            PerlinHash(cubeX + 1, cubeY, cubeZ + 1), xf - 1, yf, zf -1 ),
                        xu);
            x2 = Lerp(
            PerlinGradient(
                PerlinHash(cubeX, cubeY + 1, cubeZ + 1), xf, yf - 1, zf -1),
            PerlinGradient(
                PerlinHash(cubeX + 1, cubeY + 1, cubeZ +1), xf - 1, yf - 1, zf -1),
            xu);

            var y2 = Lerp(x1, x2, yv);

            NoiseValue = (Lerp(y1,y2,zw)+1)/2;
        }



        public double Lerp(double a, double b, double x)
        {
            return a + x * (b - a);
        }

        //for smoothing, moves vales towards 0,1??
        public double fade(double t)
        {
            return t*t*t*(t*(t*6 - 15) + 10);
        }

        public int PerlinHash(int x, int y, int z)
        {
            var step1 =  permutation[x] + y;
            var step2 = permutation[step1];
            return permutation[step2] + z;
        }

        //hash is our random seed based on cube, x,y,z our position in the cube
        public double PerlinGradient(int hash, double x, double y, double z)
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
    }

    public class Vector3
    {
        public int X;
        public int Y;
        public int Z;
        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
