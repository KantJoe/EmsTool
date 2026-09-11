using org.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace org.Utils.Global
{
    public class LivingTracingContext
    {
        public static LivingTracingContext Instance { get; private set; }

        public List<LivingTracedPoint> Points { get; private set; }

        private LivingTracingContext()
        {
            Points = [];
        }

        static LivingTracingContext()
        {
            Instance = new LivingTracingContext();
        }


        public void Initialize()
        {
            Points.Clear();
        }
    }
}
