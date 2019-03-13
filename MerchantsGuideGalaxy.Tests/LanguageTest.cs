using MerchantsGuideGalaxy.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MerchantsGuideGalaxy.Tests
{
    [TestClass]
    public class LanguageTest
    {
        private const string ERROR = "I have no idea what you are talking about";

        [TestMethod]
        public void RomanToInt1()
        {
            var languageService = LanguageService.Processar("X");
            Assert.IsTrue(languageService == "10");
        }

        [TestMethod]
        public void RomanToInt2()
        {
            var languageService = LanguageService.Processar("IX");
            Assert.IsTrue(languageService == "9");
        }

        [TestMethod]
        public void RomanToInt3()
        {
            var languageService = LanguageService.Processar("IIX");
            Assert.IsTrue(languageService == ERROR);
        }
    }
}
