using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Particles.MyMath;

namespace Particles
{
    [TypeConverter(typeof(ColorCreatorConverter))]
    [Serializable]
    public class ShapeCreator
    {
        public float Factor { get; set; }
        public float Scale { get; set; }
        public float[] Scales { get; set; } = new float[0];
        public float[] Factors { get; set; } = new float[0];
        public byte Count { get; set; }//CountEdges
        public bool MoreScales { get; set; }
        public bool MoreFactors { get; set; }
        public PointF[] GetPoints(PointF loc, SizeF hs, float angle)
        {
            var arr = new PointF[Count];
            float scale = Scale, factor = Factor;
            bool ms = MoreScales, mf = MoreFactors;
            int fl = Factors.Length, sl = Scales.Length;
            for (int i = 0; i < Count; i++)
            {
                if (mf && fl != 0) factor = Factors[i % fl];
                if (ms && sl != 0) scale = Scales[i % sl];
                angle += factor;
                arr[i] = new PointF(loc.X + scale * hs.Width * cos(angle), loc.Y + scale * hs.Height * sin(angle));
            }
            return arr;
        }
        public static ShapeCreator Hexagon => new ShapeCreator()
        {
            Count = 6,
            Factor = 60,
            MoreFactors = false,
            MoreScales = false,
            Scale = 1
        };
        public static ShapeCreator Star => new ShapeCreator()
        {
            Count = 10,
            Factor = 36,
            MoreFactors = false,
            MoreScales = true,
            Scales = new float[] { 1, 0.5f },
            Scale = 1
        };
        public static ShapeCreator Triangle => new ShapeCreator()
        {
            Scale = 1,
            Factor = 120,
            Count = 3
        };
        public static ShapeCreator Custom { get; set; } = new ShapeCreator()
        {
            Count=6, 
            MoreScales = true,
            MoreFactors = true,
            Factors = new float[]
            {-159.8437f, 1.141014f, 55.82161f, 122.6639f, -1.630135f, 58.495f},
            Scales = new float[]
            {0.9984096f, 0.1177614f, 0.4921749f, 1f, 0.1193802f, 0.4912077f}
        };
        public static ShapeCreator Snowflake { get; set; } = new ShapeCreator()
        {
            Count = 120,
            MoreScales = true,
            MoreFactors = true,
            Factors = new float[]
            {7.23483658f, 12.33908f, -15.55457f, -3.426033f, 22.10273f, 1.766273f, -18.17706f, 2.89576f, 16.04114f, 0.9516754f, 7.151558f, 1.42144f, 15.53991f, 3.054047f, -17.46178f, 2.619545f, 20.79412f, -2.183952f, -14.03624f, 16.92752f},
            Scales = new float[]
            {0.1342521f, 0.3032863f, 0.4126647f, 0.6804561f, 0.4515012f, 0.6252407f, 0.8380331f, 0.954203f, 0.7495596f, 0.998445f, 1f, 0.7516297f, 0.9629558f, 0.8479859f, 0.627721f, 0.4549295f, 0.6926767f, 0.4238097f, 0.3083669f, 0.1453655f}
        };
        public static ShapeCreator Navalny { get; set; } = new ShapeCreator()
        {
            Count = 12,
            MoreScales = true,
            MoreFactors = true,
            Factors = new float[]
            { -225, 255.9638f, -19.65382f, 33.69006f, -90, 33.69007f, -19.65383f, -104.0362f, -26.56505f, 26.56505f, -90, 26.56505f },
            Scales = new float[]
            {0.7276069f, 1, 0.8744746f, 0.2425356f, 0.2425356f, 0.8744746f, 1, 0.7276069f, 0.5423262f, 0.2425356f, 0.2425356f, 0.5423262f}
        };
    }
}
/*
 * MUSTHAVE!
 {45, -45, -45, -45, -45, 135, -225, 315}
 {1, 0.7071068f, 1, 0.7071068f, 1, 0.7071068f, 1, 0.7071068f}
     */
