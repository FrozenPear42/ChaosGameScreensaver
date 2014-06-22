using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ChaosGameScreensaver
{

    [Serializable]
    class RenderData
    {
        public int N { set; get; }
        public double F { set; get; }

        public RenderData(int n, double f)
        {
            this.N = n;
            this.F = f;
        }

    }
}
