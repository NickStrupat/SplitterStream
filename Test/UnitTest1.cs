using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplitterStreams.Test {
	[TestClass]
	public class UnitTest1 {
		[TestMethod]
		public void TestMethod1() {
			const String text = "This is some text";
			var bytes = Encoding.Default.GetBytes(text);
			using (var source = new MemoryStream(bytes))
			using (var destination1 = new MemoryStream())
			using (var destination2 = new MemoryStream())
			using (var splitter = new SplitterStream(destination1, destination2))
			using (var destination1Reader = new StreamReader(destination1))
			using (var destination2Reader = new StreamReader(destination2)) {
				source.CopyTo(splitter);

				destination1.Position = 0;
				destination2.Position = 0;

				var text1 = destination1Reader.ReadToEnd();
				var text2 = destination2Reader.ReadToEnd();

				Assert.AreEqual(text, text1);
				Assert.AreEqual(text, text2);
			}
		}
	}
}
