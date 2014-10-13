using System;
using System.Drawing;

namespace Dot_Slash
{
    /// <summary>
    /// Class used to represent different colours and their values.
    /// </summary>
    class ColourBucket
    {
        //				   Colour Bins
        // Colour		INT			HEX		R	G	B
        //==================================================================================
        //White			16777215		FFFFFF		255	255	255
        //Silver		12632256		C0C0C0		192	192	192
        //Grey			8421504			808080		128	128	128
        //Black			0			    000000		0	0	0
        //Blue			255			    0000FF		0	0	255
        //Turquoise		4251856			40E0D0		64	224	208
        //Green			32768			008000		0	128	0
        //Yellow		16776960		FFFF00		255	255	0
        //Gold			16766720		FFD700		255	215	0
        //Orange		16753920		FFA500		255	165	0
        //Brown			4796700			49311C		73	49	28
        //Red			16711680		FF0000		255	0	0
        //Maroon		8388608			800000		128	0	0
        //Violet		15631086		EE82EE		238	130	238
        //Beige			16119260		F5F5DC		245	245	220
        //Bronze		13467442		CD7F32		205	127	50
        //Charcoal		3355443			333333		51	51	51

        private string colourName;
        /// <summary>
        /// Returns the colour name.
        /// </summary>
        public string ColourName;

	    private string hexValue;
        /// <summary>
        /// Returns the hexadecimal value of the colour.
        /// </summary>
        public string HexValue
        {
            get
            {
                return hexValue;
            }
            set
            {
                hexValue = value;
            }
        }

        private int r;
        /// <summary>
        /// Returns the R value of the colour.
        /// </summary>
        public int R
        {
            get
            {
                return r;
            }
            set
            {
                r = value;
            }
        }

        private int g;
        /// <summary>
        /// Returns the G value of the colour.
        /// </summary>
        public  int G
        {
            get
            {
                return g;
            }
            set
            {
                g = value;
            }
        }

        private int b;
        /// <summary>
        /// Returns the B value of the colour.
        /// </summary>
        public int B
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
            }
        }

        private double h;
        /// <summary>
        /// Returns the hue value of the colour.
        /// </summary>
        public double H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }

        private double s;
        /// <summary>
        /// Returns the saturation value of the colour.
        /// </summary>
        public double S
        {
            get
            {
                return s;
            }
            set
            {
                s = value;
            }
        }

        private double v;
        /// <summary>
        /// Returns the value of the colour.
        /// </summary>
        public double V
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }

        /// <summary>
        /// Initialises a new instance of the ColourBucket class with specified colour name, RGB and hexadeicmal values.
        /// </summary>
        /// <param name="name"></param>Colour name.
        /// <param name="red"></param>R value of the colour.
        /// <param name="green"></param>G value of the colour.
        /// <param name="blue"></param>B value of the colour.
        /// <param name="hex"></param>Hexadecimal value of the colour.
        public ColourBucket(string name, int red, int green, int blue, string hex)
        {
            colourName = name;
            r = red;
            g = green;
            b = blue;
            hexValue = hex;

            //calculate the HSV values of the colour
            double max = Math.Max(r / 255d, Math.Max(g / 255d, b / 255d));
            double min = Math.Min(r / 255d, Math.Min(g / 255d, b / 255d));
            double difference = max - min;

            h = Color.FromArgb(r, g, b).GetHue();
            s = (max == 0) ? 0 : ((difference / max)*100);
            v = max*100;
        }
    }
}
