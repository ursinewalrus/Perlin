using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
namespace PerlinControls
{
    public class PerlinVarsModel
    {
        private IDatabase DB;
        public PerlinVarsModel()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            DB = redis.GetDatabase();
        }

        public double RedValueMultiplier
        {
            get
            {
                if (!DB.KeyExists("Perlin-RedNoiseMultiplier"))
                {
                    DB.StringSet("Perlin-RedNoiseMultiplier", 1);
                    return 1.0;
                }
                return (double) DB.StringGet("Perlin-RedNoiseMultiplier");
            }
            set
            {
                DB.StringSet("Perlin-RedNoiseMultiplier", value);
            }
        }
        public double GreenValueMultiplier
        {
            get
            {
                if (!DB.KeyExists("Perlin-GreenNoiseMultiplier"))
                {
                    DB.StringSet("Perlin-GreenNoiseMultiplier", 1);
                    return 1.0;
                }
                return (double)DB.StringGet("Perlin-GreenNoiseMultiplier");
            }
            set
            {
                DB.StringSet("Perlin-GreenNoiseMultiplier", value);
            }
        }
        public double BlueValueMultiplier
        {
            get
            {
                if (!DB.KeyExists("Perlin-BlueNoiseMultiplier"))
                {
                    DB.StringSet("Perlin-BlueNoiseMultiplier", 1);
                    return 1.0;
                }
                return (double)DB.StringGet("Perlin-BlueNoiseMultiplier");
            }
            set
            {
                DB.StringSet("Perlin-BlueNoiseMultiplier", value);
            }
        }
        public int PerlinOctaves
        {
            get
            {
                if (!DB.KeyExists("Perlin-Octaves"))
                {
                    DB.StringSet("Perlin-Octaves", 25);
                    return 25;
                }
                return (int)DB.StringGet("Perlin-Octaves");
            }
            set
            {
                DB.StringSet("Perlin-Octaves", value);
            }
        }
        public double PerlinPersistence
        {
            get
            {
                if (!DB.KeyExists("Perlin-Persistence"))
                {
                    DB.StringSet("Perlin-Persistence", .25);
                    return .25;
                }
                return (double)DB.StringGet("Perlin-Persistence");
            }
            set
            {
                DB.StringSet("Perlin-Persistence", value);
            }
        }
        public int PerlinFrequency
        {
            get
            {
                if (!DB.KeyExists("Perlin-Frequency"))
                {
                    DB.StringSet("Perlin-Frequency", 4);
                    return 4;
                }
                return (int)DB.StringGet("Perlin-Frequency");
            }
            set
            {
                DB.StringSet("Perlin-Frequency", value);
            }
        }
        public int PerlinAmplitude
        {
            get
            {
                if (!DB.KeyExists("Perlin-Amplitude"))
                {
                    DB.StringSet("Perlin-Amplitude", 32);
                    return 32;
                }
                return (int)DB.StringGet("Perlin-Amplitude");
            }
            set
            {
                DB.StringSet("Perlin-Amplitude", value);
            }
        }

    }
}
