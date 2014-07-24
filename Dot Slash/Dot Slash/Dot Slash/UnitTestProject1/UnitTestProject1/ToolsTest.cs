using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot_Slash;
using System.IO;

namespace UnitTestProject1
{
	[TestClass]
	public class ToolsTest
	{
		[TestMethod]
		public void getimagesTest()
		{
			Tools tool = new Tools();
			String[] extensions = {"jpg", "jpeg", "bmp", "png", "pgm" };
			String[] files = Tools.getImages("testGetImages", extensions);
			String[] directoryFiles = Directory.GetFiles("testGetImages/");
			Assert.AreEqual(directoryFiles.Length, files.Length, "Not all images were collected as expected");
		}
	}
}
