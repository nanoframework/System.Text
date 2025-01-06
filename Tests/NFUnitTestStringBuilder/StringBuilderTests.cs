//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using System;
using System.Text;

namespace NFUnitTestStringBuilder
{
    [TestClass]
    public class StringBuilderTests
    {
        private static StringBuilder stringBuilder;

        [Setup]
        public void InitializeStringBuild()
        {
            stringBuilder = new StringBuilder();
        }

        [TestMethod]
        public void Test_0_AppendTest_0()
        {
            stringBuilder.Append(true);
            Assert.AreEqual(bool.TrueString, stringBuilder.ToString());
            stringBuilder.Append(false);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString), stringBuilder.ToString());
            stringBuilder.Append(byte.MinValue);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue), stringBuilder.ToString());
            stringBuilder.Append(new char[] { 'x', 'a' });
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa"), stringBuilder.ToString());
            stringBuilder.Append(double.Epsilon);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString()), stringBuilder.ToString());
            stringBuilder.Append(float.Epsilon);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString()), stringBuilder.ToString());
            stringBuilder.Append(int.MaxValue);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue), stringBuilder.ToString());
            stringBuilder.Append(long.MaxValue);
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue, long.MaxValue), stringBuilder.ToString());
            stringBuilder.Append((object)"string");
            Assert.AreEqual(string.Concat(bool.TrueString, bool.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue, long.MaxValue, "string"), stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_1_RemoveTest_0()
        {
            Assert.AreEqual(string.Empty, stringBuilder.Clear().ToString());
            string testString = "0123456789";
            stringBuilder.Append(testString);
            stringBuilder.Remove(0, 1);
            Assert.AreEqual("123456789", stringBuilder.ToString());
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            Assert.AreEqual("12345678", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_2_InsertTest_0()
        {
            stringBuilder.Clear();
            string testString = "0000";
            stringBuilder.Append(testString);
            stringBuilder.Insert(0, "x", 2);
            Assert.AreEqual("xx0000", stringBuilder.ToString());
            stringBuilder.Insert(stringBuilder.Length, "x", 2);
            Assert.AreEqual("xx0000xx", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_0()
        {
            stringBuilder.Clear();
            string testString = "0000";
            stringBuilder.Append(testString);
            stringBuilder.Append("_");
            stringBuilder.Append(testString);
            stringBuilder.Replace(testString, "xx");
            Assert.AreEqual("xx_xx", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_1()
        {
            stringBuilder.Clear();
            string testString = "BEGIN";
            stringBuilder.Append('B');
            stringBuilder.Append('E');
            stringBuilder.Append('G');
            stringBuilder.Append('I');
            stringBuilder.Append('N');
            stringBuilder.Append('_');
            stringBuilder.Append('M');
            stringBuilder.Append('I');
            stringBuilder.Append('D');
            stringBuilder.Append('_');
            stringBuilder.Append('E');
            stringBuilder.Append('N');
            stringBuilder.Append('D');
            stringBuilder.Replace(testString, "xx");
            Assert.AreEqual("xx_MID_END", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_2()
        {
            string testString = "MID";
            stringBuilder.Replace(testString, "xx");
            Assert.AreEqual("xx_xx_END", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_3()
        {
            string testString = "END";
            stringBuilder.Replace(testString, "xx");
            Assert.AreEqual("xx_xx_xx", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_4()
        {
            string testString = "The quick br!wn d#g jumps #ver the lazy cat.";
            stringBuilder = new System.Text.StringBuilder(testString);
            stringBuilder.Replace('#', '!', 15, 29);        // Some '#' -> '!'
            Assert.AreEqual("The quick br!wn d!g jumps !ver the lazy cat.", stringBuilder.ToString());
            stringBuilder.Replace('!', 'o');                // All '!' -> 'o'
            Assert.AreEqual("The quick brown dog jumps over the lazy cat.", stringBuilder.ToString());
            stringBuilder.Replace("cat", "dog");            // All "cat" -> "dog"
            Assert.AreEqual("The quick brown dog jumps over the lazy dog.", stringBuilder.ToString());
            stringBuilder.Replace("dog", "fox", 15, 20);    // Some "dog" -> "fox"
            Assert.AreEqual("The quick brown fox jumps over the lazy dog.", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_5()
        {
            stringBuilder.Clear();
            stringBuilder.Append("12345");
            stringBuilder.Replace("45", "def");
            Assert.AreEqual("123def", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_6()
        {
            stringBuilder.Clear();
            stringBuilder.Append("[{1234}]Test}]");
            stringBuilder.Replace("}]", "}]example");
            Assert.AreEqual("[{1234}]exampleTest}]example", stringBuilder.ToString());
        }

        [TestMethod]
        public void Test_3_ReplaceTest_7()
        {
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                string sRaw, sFind, sReplace;
                GenerateFuzzyParameters(out sRaw, out sFind, out sReplace, random);
                stringBuilder.Clear();
                stringBuilder.Append(sRaw);
                stringBuilder.Replace(sFind, sReplace);
                string sMFOutput = stringBuilder.ToString();
                string sNativeOutput = NativeReplace(sRaw, sFind, sReplace);

                Assert.AreEqual(sNativeOutput, sMFOutput);
            }
        }

        void GenerateFuzzyParameters(out string sRaw, out string sFind, out string sReplace, Random random)
        {
            int cFind = random.Next(1, 4);
            sFind = RandomString(random, 2, 6);
            sReplace = RandomString(random, 4, 10);

            sRaw = string.Empty;
            for (int i = 0; i < cFind; i++)
            {
                if (i > 0 || random.Next() % 5 > 0)
                {
                    sRaw += RandomString(random, 2, 6);
                }

                sRaw += sFind;
            }

            if (random.Next() % 5 > 0)
            {
                sRaw += RandomString(random, 2, 6);
            }
        }

        string RandomString(Random random, int iLenMin, int iLenMax)
        {
            string sChars = "abcdefghijklmnopqrstuvwxyz0123456789{}[]-=+()";
            int length = random.Next(iLenMin, iLenMax);

            string sOutput = string.Empty;
            for (int i = 0; i < length; i++)
            {
                sOutput += sChars[random.Next(0, sChars.Length - 1)];
            }

            return sOutput;
        }

        string NativeReplace(string sRaw, string sFind, string sReplace)
        {
            string sOutput = sRaw;
            int i = 0;

            while (i < sOutput.Length)
            {
                int p = sOutput.IndexOf(sFind, i);
                if (p < 0)
                {
                    break;
                }

                sOutput = sOutput.Substring(0, p) + sReplace + sOutput.Substring(p + sFind.Length);
                i = p + sReplace.Length;
            }

            return sOutput;
        }

        [TestMethod]
        public void Test_4_CapacityTest_0()
        {
            stringBuilder.Length = 0;
            stringBuilder.Capacity = 5;
            Assert.AreEqual(string.Empty, stringBuilder.ToString());
            string testString = "0000";
            stringBuilder.Append(string.Empty);
            stringBuilder.Append(testString);
            stringBuilder.Append(string.Empty);
            //should allocate here
            stringBuilder.Append("_");
            stringBuilder.Append("_");
            //result is true if capacity is > 5
            Assert.IsTrue(stringBuilder.Capacity > 5);
        }

        [TestMethod]
        public void Test_5_ToStringLengthTest_0()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(0, 5);
            Assert.AreEqual("Hello", outStr);
        }

        [TestMethod]
        public void Test_5_ToStringLengthTest_1()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(0, 25);
            Assert.AreEqual("Hello from nanoFramework!", outStr);
        }

        [TestMethod]
        public void Test_5_ToStringStartIndexLengthTest_0()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(11, 13);
            Assert.AreEqual("nanoFramework", outStr);
        }

        [TestMethod]
        public void Test_FixedSize()
        {
            // Arrange
            string testStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string testSize = "testStr SB Variable Size: ";
            var sbvariable = new StringBuilder();
            var sbfixed = new StringBuilder(256);

            // Act
            sbvariable.Append(testSize);
            sbvariable.Append(testStr);

            sbfixed.Append(testSize);
            sbfixed.Append(testStr);

            // Assert
            Assert.AreEqual($"{testSize}{testStr}", sbvariable.ToString());
            Assert.AreEqual($"{testSize}{testStr}", sbfixed.ToString());
        }

        [TestMethod]
        public void Test_FixedSizeInCreasing()
        {
            // Arrange
            string testStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string testStrCompiled = string.Empty;
            var sbfixed = new StringBuilder(256);

            // Act
            sbfixed.Append(testStr);
            testStrCompiled += testStr;

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());

            // Act
            for (int i = 0; i < 20; i++)
            {
                sbfixed.Append(testStr);
                testStrCompiled += testStr;
            }

            // Assertf
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());
        }


        [TestMethod]
        public void Test_FixedSizeInCreasingWithClear()
        {
            // Arrange
            string testStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string testStrCompiled = string.Empty;
            var sbfixed = new StringBuilder(256);

            // Act
            sbfixed.Append(testStr);
            testStrCompiled += testStr;

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());

            // Act
            for (int i = 0; i < 20; i++)
            {
                sbfixed.Clear();
                sbfixed.Append(testStr);
                testStrCompiled = testStr;
            }

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());
        }


        [TestMethod]
        public void Test_FixedSizeInCreasingWithClearAndAppend()
        {
            // Arrange
            string testStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string testStrCompiled = string.Empty;
            var sbfixed = new StringBuilder(256);

            // Act
            sbfixed.Append(testStr);
            testStrCompiled += testStr;

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());

            // Act
            for (int i = 0; i < 20; i++)
            {
                sbfixed.Clear();
                sbfixed.Append(testStr);
                testStrCompiled = testStr;
                sbfixed.Append(testStr);
                testStrCompiled += testStr;
            }

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());
        }

        [TestMethod]
        public void Test_FixedSizeInCreasingWithClearAndAppendAndRemove()
        {
            // Arrange
            string testStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string testStrCompiled = string.Empty;
            var sbfixed = new StringBuilder(256);

            // Act
            sbfixed.Append(testStr);
            testStrCompiled += testStr;

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());

            // Act
            for (int i = 0; i < 20; i++)
            {
                sbfixed.Clear();
                sbfixed.Append(testStr);
                testStrCompiled = testStr;
                sbfixed.Append(testStr);
                testStrCompiled += testStr;
                sbfixed.Remove(0, 5);
                testStrCompiled = testStrCompiled.Substring(5);
            }

            // Assert
            Assert.AreEqual(testStrCompiled, sbfixed.ToString());
        }


        [TestMethod]
        public void Clear_Empty_CapacityNotZero()
        {
            var builder = new StringBuilder();
            builder.Clear();
            Assert.IsTrue(builder.Capacity != 0);
        }

        [TestMethod]
        public void Clear_Empty_CapacityStaysUnchanged()
        {
            var sb = new StringBuilder(14);
            sb.Clear();
            Assert.AreEqual(14, sb.Capacity);
        }

        [TestMethod]
        public void Clear_Full_CapacityStaysUnchanged()
        {
            var sb = new StringBuilder(14);
            sb.Append("Hello World!!!");
            sb.Clear();
            Assert.AreEqual(14, sb.Capacity);
        }

        [TestMethod]
        public void Clear_AtMaxCapacity_CapacityStaysUnchanged()
        {
            var builder = new StringBuilder(14, 14);
            builder.Append("Hello World!!!");
            builder.Clear();
            Assert.AreEqual(14, builder.Capacity);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(10000)]
        public void Clear_AppendAndInsertBeforeClearManyTimes_CapacityStaysWithinRange(int times)
        {
            var builder = new StringBuilder();
            var originalCapacity = builder.Capacity;
            var s = new string(' ', 10);
            int oldLength = 0;
            for (int i = 0; i < times; i++)
            {
                builder.Append(s);
                builder.Append(s);
                builder.Append(s);
                builder.Insert(0, s, 1);
                builder.Insert(0, s, 1);
                oldLength = builder.Length;

                builder.Clear();
            }
            Assert.IsTrue(builder.Capacity >= 1 && builder.Capacity <= oldLength * 1.2);
        }

        [TestMethod]
        public void Clear_InitialCapacityMuchLargerThanLength_CapacityReducedToInitialCapacity()
        {
            var builder = new StringBuilder(100);
            var initialCapacity = builder.Capacity;
            builder.Append(new string('a', 40));
            builder.Insert(0, new string('a', 10), 1);
            builder.Insert(0, new string('a', 10), 1);
            builder.Insert(0, new string('a', 10), 1);
            var oldCapacity = builder.Capacity;
            var oldLength = builder.Length;
            builder.Clear();
            Assert.AreNotEqual(oldCapacity, builder.Capacity);
            Assert.AreEqual(initialCapacity, builder.Capacity);
            Assert.IsFalse(builder.Capacity >= 1 && builder.Capacity <= oldLength * 1.2);

            // find max between initial capacity and 1.2 * old length
            int maxCapacity = initialCapacity > (int)(oldLength * 1.2) ? initialCapacity : (int)(oldLength * 1.2);

            Assert.IsTrue(builder.Capacity >= 1 && builder.Capacity <= maxCapacity);
        }

        [TestMethod]
        public void Clear_StringBuilderHasTwoChunks_OneChunkIsEmpty_ClearReducesCapacity()
        {
            var sb = new StringBuilder(string.Empty);
            int initialCapacity = sb.Capacity;
            for (int i = 0; i < initialCapacity; i++)
            {
                sb.Append('a');
            }

            sb.Insert(0, new char[] { 'a' }, 0, 1);

            while (sb.Length > 1)
            {
                sb.Remove(1, 1);
            }

            int oldCapacity = sb.Capacity;
            sb.Clear();
            Assert.AreEqual(oldCapacity - 1, sb.Capacity);
            Assert.AreEqual(initialCapacity, sb.Capacity);
        }
    }

    static class RandomExtension
    {
        static public int Next(this Random random, int iMin, int iMax)
        {
            return random.Next(iMax - iMin) + iMin;
        }
    }
}
