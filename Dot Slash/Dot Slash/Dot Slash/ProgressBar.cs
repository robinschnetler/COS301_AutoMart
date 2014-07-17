using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot_Slash
{
	class ProgressBar
	{
		public ProgressBar()
		{

		}

		public void UpdateProgress(int complete, int maxVal, int barSize, char progressCharacter)
		{
			Console.CursorVisible = false;
			int left = Console.CursorLeft;
			decimal perc = (decimal)complete / (decimal)maxVal;
			int chars = (int)Math.Floor(perc / ((decimal)1 / (decimal)barSize));
			string p1 = String.Empty, p2 = String.Empty;

			for (int i = 0; i < chars; i++) p1 += progressCharacter;
			for (int i = 0; i < barSize - chars; i++) p2 += progressCharacter;

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(p1);
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write(p2);

			Console.ResetColor();
			Console.Write(" {0}%", (perc * 100).ToString("N2"));
			Console.CursorLeft = left;
		}
	}
}
