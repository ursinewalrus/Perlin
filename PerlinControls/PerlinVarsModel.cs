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


    }
}
