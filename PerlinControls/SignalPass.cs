using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinControls
{
    public class SignalPass
    {
        public enum ReInit
        {
            None,
            Color,
            NumGradients,
            Lines,
            All
        }
        public ReInit InitState = ReInit.All;
    }
}
