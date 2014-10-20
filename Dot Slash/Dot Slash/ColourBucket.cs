using System;
using System.Drawing;

namespace Dot_Slash
{
	/// <summary>
	/// Class used to represent different colours and their values.
	/// </summary>
	public class ColourBucket
	{
		//				   Colour Bins
		// Colour		INT			    HEX		R	G	B       H   S   V
		//==================================================================================
		//White			16777215		FFFFFF		255	255	255     0   0   100
		//Red			16711680		FF0000		255	0	0       0   100 100
		//Orange		16753920		FFA500		255	165	0       39  100 100 
		//Gold			16766720		FFD700		255	215	0       51  100 100
		//Yellow		16776960		FFFF00		255	255	0       60  100 100
		//Blue			255			0000FF		0	0	255     240 100 100
		//Beige			16119260		F5F5DC		245	245	220     60  10  96  
		//Violet		15631086		EE82EE		238	130	238     300 45  93 
		//Turquoise		4251856			40E0D0		64	224	208     174 71  87
		//Bronze		13467442		CD7F32		205	127	50      30  75  80
		//Silver		12632256		C0C0C0		192	192	192     0   0   75 
		//Grey			8421504			808080		128	128	128     0   0   50
		//Maroon		8388608			800000		128	0	0       0   100 50 
		//Green			32768			008000		0	128	0       120 100 50
		//Brown			4796700			49311C		73	49	28      28  61  28 
		//Charcoal		3355443			333333		51	51	51      0   0   20
		//Black			0			000000		0	0	0       0   0   0

		// Colour		INT			HEX		R	G	B       H   S   V
		//==================================================================================
		//Blue			255			0000FF		0	0	255     240 100 100
		//Yellow		16776960		FFFF00		255	255	0       60  100 100
		//Gold			16766720		FFD700		255	215	0       51  100 100
		//Orange		16753920		FFA500		255	165	0       39  100 100 
		//Red			16711680		FF0000		255	0	0       0   100 100
		//Maroon		8388608			800000		128	0	0       0   100 50 
		//Green			32768			008000		0	128	0       120 100 50
		//Bronze		13467442		CD7F32		205	127	50      30  75  80
		//Turquoise		4251856			40E0D0		64	224	208     174 71  87
		//Brown			4796700			49311C		73	49	28      28  61  28
		//Violet		15631086		EE82EE		238	130	238     300 45  93 
		//Beige			16119260		F5F5DC		245	245	220     60  10  96  
		//White			16777215		FFFFFF		255	255	255     0   0   100
		//Silver		12632256		C0C0C0		192	192	192     0   0   75 
		//Grey			8421504			808080		128	128	128     0   0   50
		//Charcoal		3355443			333333		51	51	51      0   0   20
		//Black			0			000000		0	0	0       0   0   0

		// Colour		INT			    HEX		    R	G	B       H   S   V
		//==================================================================================
		//Violet		15631086		EE82EE		238	130	238     300 45  93 
		//Blue			255			0000FF		0	0	255     240 100 100
		//Turquoise		4251856			40E0D0		64	224	208     174 71  87
		//Green			32768			008000		0	128	0       120 100 50
		//Yellow		16776960		FFFF00		255	255	0       60  100 100
		//Beige			16119260		F5F5DC		245	245	220     60  10  96
		//Gold			16766720		FFD700		255	215	0       51  100 100 
		//Orange		16753920		FFA500		255	165	0       39  100 100 
		//Bronze		13467442		CD7F32		205	127	50      30  75  80
		//Brown			4796700			49311C		73	49	28      28  61  28
		//Red			16711680		FF0000		255	0	0       0   100 100
		//Maroon		8388608			800000		128	0	0       0   100 50
		//White			16777215		FFFFFF		255	255	255     0   0   100
		//Silver		12632256		C0C0C0		192	192	192     0   0   75 
		//Grey			8421504			808080		128	128	128     0   0   50
		//Charcoal		3355443			333333		51	51	51      0   0   20
		//Black			0			000000		0	0	0       0   0   0

		private string colourName;
		/// <summary>
		/// Returns the colour name.
		/// </summary>
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

		/// <summary>
		/// The hex value of the colour represented as a string with relevant get and set methods
		/// </summary>
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

		/// <summary>
		/// The Red value of the colour (according to the RGB colour model) in the range of [0,255]
		/// </summary>
		public int r;
		/// <summary>
		/// The Green value of the colour (according to the RGB colour model) in the range of [0,255]
		/// </summary>
		public int g;
		/// <summary>
		/// The Blue value of the colour (according to the RGB colour model) in the range of [0,255]
		/// </summary>
		public int b;

		/// <summary>
		/// The Hue value of the colour (according to the HSV colour model) that has a range of [0,360)
		/// </summary>
		public double h;
		/// <summary>
		/// The Saturation value of the colour (according to the HSV colour model) that has a range of [0,1]
		/// </summary>
		public double s;
		/// <summary>
		/// The Value value of the colour (according to the HSV colour model) that has a range of [0,1]
		/// </summary>
		public double v;

		/// <summary>
		/// The constructor that creates a ColourBucket object
		/// </summary>
		/// <param name="name">The name of the colour</param>
		/// <param name="red">The red value of the colour (RGB)</param>
		/// <param name="green">The green value of the colour (RGB)</param>
		/// <param name="blue">The blue value of the colour (RGB)</param>
		/// <param name="hex">The hex value of the colour represented as a string</param>
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
			v = (max / 255d);
		}
	}
}
