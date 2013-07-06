﻿using NUnit.Framework;

namespace Candor
{
    [TestFixture]
    public class LexicalIncrementAsciiAlphaTests
    {
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_1234()
        {
            var s = "1234";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_8999999999999999999()
        {
            var s = "8999999999999999999";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_10()
        {
            var s = "10";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_99()
        {
            var s = "99";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_099()
        {
            var s = "099";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase__99()
        {
            var s = " 99";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_99b()
        {
            var s = "99b";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_99z()
        {
            var s = "99z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_099z()
        {
            var s = "099z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_NoCase_098z()
        {
            var s = "098z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);
        }
        [Test]
        public void LexicalIncrement_NoCase_Shiz()
        {
            var s = "Shiz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);

            Assert.AreEqual("ShJa", s2);
        }
        [Test]
        public void LexicalIncrement_NoCase_ShJz()
        {
            var s = "ShJz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);

            Assert.AreEqual("ShKa", s2);
        }
        [Test]
        public void LexicalIncrement_NoCase_Sha()
        {
            var s = "Sha";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);

            Assert.AreEqual("ShB", s2);
        }
        [Test]
        public void LexicalIncrement_NoCase_zzz()
        {
            var s = "zzz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);

            Assert.AreEqual("zzza", s2);
        }
        [Test]
        public void LexicalIncrement_NoCase__zzz()
        {
            var s = " zzz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), true);

            Assert.AreEqual("aaaa", s2);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_1234()
        {
            var s = "1234";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_8999999999999999999()
        {
            var s = "8999999999999999999";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_10()
        {
            var s = "10";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_99()
        {
            var s = "99";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_099()
        {
            var s = "099";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase__99()
        {
            var s = " 99";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_99b()
        {
            var s = "99b";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_99z()
        {
            var s = "99z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_099z()
        {
            var s = "099z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void LexicalIncrement_YesCase_098z()
        {
            var s = "098z";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);
        }
        [Test]
        public void LexicalIncrement_YesCase_Shiz()
        {
            var s = "Shiz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("ShJA", s2);
        }
        [Test]
        public void LexicalIncrement_YesCase_ShJz()
        {
            var s = "ShJz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("ShjA", s2);
        }
        [Test]
        public void LexicalIncrement_YesCase_Sha()
        {
            var s = "Sha";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("ShB", s2);
        }
        [Test]
        public void LexicalIncrement_YesCase_ShA()
        {
            var s = "ShA";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("Sha", s2);
        }
        [Test]
        public void LexicalIncrement_YesCase_zzz()
        {
            var s = "zzz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("zzzA", s2);
        }
        [Test]
        public void LexicalIncrement_YesCase__zzz()
        {
            var s = " zzz";
            var s2 = s.LexicalIncrement(LexicalCharacterSetType.AsciiAlpha.ToCharacterSet(), false);

            Assert.AreEqual("AAAA", s2);
        }
    }
}
