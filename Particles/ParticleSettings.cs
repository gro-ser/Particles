using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Particles
{
    [Serializable]
    public class ParticleSettings
    {
        #region BackingField
        internal ColorCreator _colorCreator;
        internal EColorMode _colorMode;
        internal Color _overallColor;
        internal Color _backGroundColor;
        internal float _alpha;
        internal EColorFunction _colorFunction;
        internal bool _clearBuffer;
        internal EShape _shape;
        internal float _speedScale;
        internal float _sizeScale;
        internal float _tension;
        internal ShapeCreator _shapecreator;
        internal ENewLocationMode _newLocationMode;
        internal PointF _newLocationPoint;
        internal PointF _newAngleRange = new PointF(0, 360);
        internal EBounceMode _bounceMode;
        internal EMouseClickAction _mouseClickAction;
        internal bool _showTracingLine;
        internal int _tracingLen = 5;
        internal int _countOfParticles = 200;
        internal bool _resetAngle;
        #endregion

        #region Coloring
        private const string r_coloring = "Coloring";
        [Category(r_coloring)]
        public LinearGradientMode bgStyle { get; set; }
        [Category(r_coloring)]
        public Color bgColorFirst { get; set; } = Color.Red;
        [Category(r_coloring)]
        public Color bgColorSecond { get; set; } = Color.Blue;
        public enum EColorMode
        {
            Overall,
            Own,
            Function,
            RandomOverall,
            RandomOwn
        }
        [TypeConverter(typeof(EnumConverter))]
        [Category(r_coloring)]
        public EColorMode ColorMode
        { get => _colorMode; set => _colorMode = value; }
        [Category(r_coloring), XmlIgnore]
        public Color OverallColor
        { get => _overallColor; set => _overallColor = value; }
        [Browsable(false)]
        public int _OverallColor
        { get => _overallColor.ToArgb(); set => _overallColor = Color.FromArgb(value); }
        [Category(r_coloring), XmlIgnore]
        public Color BackGroundColor
        { get => _backGroundColor; set => _backGroundColor = value; }
        [Browsable(false)]
        public int _BackGroundColor
        { get => _backGroundColor.ToArgb(); set => _backGroundColor = Color.FromArgb(value); }
        public enum EColorFunction
        {
            RGB1,
            RGB2,
            HSV1,
            HSV2,
            RussianFlag,
            UkraineFlag,
            FromImage,
        }
        [Category(r_coloring)]
        public EColorFunction ColorFunction
        {
            get => _colorFunction;
            set
            {
                _colorFunction = value;
                switch (value)
                {
                    case EColorFunction.RGB1: _colorCreator = ColorCreator.RGB1; break;
                    case EColorFunction.RGB2: _colorCreator = ColorCreator.RGB2; break;
                    case EColorFunction.HSV1: _colorCreator = ColorCreator.HSVx; break;
                    case EColorFunction.HSV2: _colorCreator = ColorCreator.HSVy; break;
                    case EColorFunction.RussianFlag: _colorCreator = ColorCreator.RUSf; break;
                    case EColorFunction.UkraineFlag: _colorCreator = ColorCreator.UKRf; break;
                    case EColorFunction.FromImage: _colorCreator = ColorCreator.FIMG; break;
                    default: break;
                }
            }
        }
        [Category(r_coloring)]
        public ColorCreator ColorCreator
        { get => _colorCreator; set => _colorCreator = value; }
        [Category(r_coloring)]
        public float Alpha
        { get => _alpha; set => _alpha = value; }
        #endregion

        #region Drawning
        private const string r_drawning = "Drawning";
        [Category(r_drawning)]
        public bool ClearBuffer
        { get => _clearBuffer; set => _clearBuffer = value; }
        [Category(r_drawning)]
        public int CountOfParticles { get => _countOfParticles; set => _countOfParticles = value; }
        #endregion

        #region Locating
        private const string r_locating = "Locating";
        public enum ENewLocationMode { Center, Random, Zeroes, Point }
        [Category(r_locating)]
        public ENewLocationMode NewLocationMode
        { get => _newLocationMode; set => _newLocationMode = value; }
        [Category(r_locating), TypeConverter(typeof(PointFConverter))]
        public PointF NewLocationPoint
        { get => _newLocationPoint; set => _newLocationPoint = value; }
        [Category(r_locating), TypeConverter(typeof(PointFConverter))]
        public PointF NewAngleRange
        { get => _newAngleRange; set => _newAngleRange = value; }
        [Flags]
        public enum EBounceMode
        { None = 0, ReflectAngle = 1, OppositeSide = 2, ResetTracing = 4 }
        [Category(r_locating)]
        public EBounceMode BounceMode
        { get => _bounceMode; set => _bounceMode = value; }
        [Category(r_locating)]
        public bool ResetAngle
        { get => _resetAngle; set => _resetAngle = value; }
        #endregion

        #region Shaping
        private const string r_shaping = "Shaping";
        public enum EShape
        { Sphere, Rectangle, Hexagon, Star, Triangle, Custom }
        [Category(r_shaping)]
        public EShape Shape
        {
            get => _shape;
            set
            {
                _shape = value;
                switch (value)
                {
                    case EShape.Hexagon:
                        _shapecreator = ShapeCreator.Hexagon;
                        break;
                    case EShape.Star:
                        _shapecreator = ShapeCreator.Star;
                        break;
                    case EShape.Triangle:
                        _shapecreator = ShapeCreator.Triangle;
                        break;
                    case EShape.Custom:
                        _shapecreator = ShapeCreator.Custom;
                        break;
                    default:
                        break;
                }
            }
        }
        [Category(r_shaping)]
        public float SpeedScale
        { get => _speedScale; set => _speedScale = value; }
        [Category(r_shaping)]
        public float SizeScale
        { get => _sizeScale; set => _sizeScale = value; }
        [Category(r_shaping)]
        public float Tension
        { get => _tension; set => _tension = value; }
        [Category(r_shaping)]
        public ShapeCreator Shapecreator
        { get => _shapecreator; set => _shapecreator = value; }
        #endregion

        #region Interacting
        private const string r_interacting = "Interacting";
        public enum EMouseClickAction
        {
            None,
            ChangeAngle,
            RotateParticles,
            AngleToMouse,
            Bubble
        }
        [Category(r_interacting)]
        public EMouseClickAction MouseClickAction
        { get => _mouseClickAction; set => _mouseClickAction = value; }
        #endregion

        #region Tracing
        private const string r_tracing = "Tracing";
        [Category(r_tracing)]
        public bool ShowTracingLine
        { get => _showTracingLine; set => _showTracingLine = value; }
        [Category(r_tracing)]
        public int TracingLen
        { get => _tracingLen; set => _tracingLen = value; }
        #endregion

        #region Debugging

        #endregion

        internal static ParticleSettings Default()
        {
            return new ParticleSettings()
            {
                //Coloring
                ColorMode = EColorMode.Overall,
                OverallColor = Color.White,
                BackGroundColor = Color.Black,
                ClearBuffer = true,
                ColorFunction = EColorFunction.RGB1,
                Alpha = 1,

                //Locating
                NewLocationMode = ENewLocationMode.Random,
                NewLocationPoint = new Point(0, 0),
                BounceMode = EBounceMode.ReflectAngle,

                //Shaping
                Shape = EShape.Sphere,
                SizeScale = 1,
                SpeedScale = 1,
                Shapecreator = ShapeCreator.Hexagon,

                //Interacting
                //MouseClickAction = EMouseClickAction.RotateParticles,

                //Tracing
                ShowTracingLine = false,
                TracingLen = 5,
            };
        }

        public static string Info(object obj, string delimeter = "\r\n")
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder(type.Name).Append(" {").Append(delimeter);
            foreach (System.Reflection.PropertyInfo prop in props)
            {
                sb
                    .Append('\t')
                    .Append(prop.Name)
                    .Append(':')
                    .Append(prop.GetValue(obj))
                    .Append(";")
                    .Append(delimeter);
            }
            return sb.Append('}').ToString();
        }
        public override string ToString()
        {
            return Info(this);
        }

        public void SaveXml()
        {
            XmlSerializer xml = new XmlSerializer(typeof(ParticleSettings));
            FileStream file = File.Open("settings.xml", FileMode.Create);
            xml.Serialize(file, this);
            file.Close();
        }
        public static ParticleSettings LoadXml()
        {
            XmlSerializer xml = new XmlSerializer(typeof(ParticleSettings));
            FileStream file = File.OpenRead("settings.xml");
            ParticleSettings res = (ParticleSettings)xml.Deserialize(file);
            file.Close();
            return res;
        }
        public void SaveBin()
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream file = File.Open("settings.bin", FileMode.Create);
            bin.Serialize(file, this);
            file.Close();
        }
        public static ParticleSettings LoadBin()
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream file = File.OpenRead("settings.bin");
            ParticleSettings res = (ParticleSettings)bin.Deserialize(file);
            res.ColorFunction = res._colorFunction;
            file.Close();
            return res;
        }
    }
}
