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

		[TestMethod]
		public void changeExtensionsTest()
		{
			Tools tool = new Tools();
			String path = "testExtensions/";
			String[] files = Directory.GetFiles(path, "*.jpg");
			tool.changeExtension(path, "jpg", "vec");
			String[] newFiles = Directory.GetFiles(path, "*.vec");
			Assert.AreEqual(files.Length, newFiles.Length, "Not all extensions were changed");
		}
	}
}
