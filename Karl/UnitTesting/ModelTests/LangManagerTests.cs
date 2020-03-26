using Karl.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.ModelTests
{
	public class LangManagerTests
	{
		[Fact]
		public void ChooseLangTest()
		{
			//setup
			var manager = LangManager.SingletonLangManager;
			//var mock = new Mock<FileInfo>();
			var test = new Lang_NEW(new FileInfo("test"));
			manager.LangMap.Add("test", test);
			//test
			Assert.True(manager.ChooseLang("test"));
			Assert.Equal(test, manager.CurrentLang);
		}

		internal class Lang_NEW : Lang
		{
			public Lang_NEW(FileInfo file) : base(file) { }
			public override void Init() { }
		}
	}
}
