using System;
using System.Drawing;

namespace Dot_Slash
{
    class ColourBucket
    {
        //				   Colour Bins
        // Colour		INT			HEX		R	G	B
        //==================================================================================
        //White			16777215		FFFFFF		255	255	255
        //Silver		12632256		C0C0C0		192	192	192
        //Grey			8421504			808080		128	128	128
        //Black			0			000000		0	0	0
        //Blue			255			0000FF		0	0	255
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
        public string ColourName
        {
            get
            {
                return colourName;
            }
            set
            {
                colourName = value;
            }
        }
	    private string hexValue;
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

        public int r;
        public int g;
        public int b;

        public double h;
        public double s;
        public double v;

        public ColourBucket(string name, int red, int green, int blue, string hex)
        {
	        hexValue = hex;
            colourName = name;
            r = red;
            g = green;
            b = blue;

            int max = Math.Max(r, Math.Max(g, b));
            int min = Math.Min(r, Math.Min(g, b));

            h = Color.FromArgb(r, g, b).GetHue();
            s = (max == 0) ? 0 : 1d - (1d * min / max);
            v = max / 255d;
        }
    }
}
