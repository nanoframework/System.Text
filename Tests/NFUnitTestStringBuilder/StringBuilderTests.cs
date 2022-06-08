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
            Assert.Equal(stringBuilder.ToString(), Boolean.TrueString);
            stringBuilder.Append(false);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString));
            stringBuilder.Append(byte.MinValue);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue));
            stringBuilder.Append(new char[] { 'x', 'a' });
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa"));
            stringBuilder.Append(double.Epsilon);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString()));
            stringBuilder.Append(float.Epsilon);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString()));
            stringBuilder.Append(int.MaxValue);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue));
            stringBuilder.Append(long.MaxValue);
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue, long.MaxValue));
            stringBuilder.Append((object)"string");
            Assert.Equal(stringBuilder.ToString(), string.Concat(Boolean.TrueString, Boolean.FalseString, byte.MinValue, char.MinValue, "xa", double.Epsilon.ToString(), float.Epsilon.ToString(), int.MaxValue, long.MaxValue, "string"));
        }

        [TestMethod]
        public void Test_1_RemoveTest_0()
        {
            Assert.Equal(stringBuilder.Clear().ToString(), string.Empty);
            string testString = "0123456789";
            stringBuilder.Append(testString);
            stringBuilder.Remove(0, 1);
            Assert.Equal(stringBuilder.ToString(), "123456789");
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            Assert.Equal(stringBuilder.ToString(), "12345678");
        }

        [TestMethod]
        public void Test_2_InsertTest_0()
        {
            stringBuilder.Clear();
            string testString = "0000";
            stringBuilder.Append(testString);
            stringBuilder.Insert(0, "x", 2);
            Assert.Equal(stringBuilder.ToString(), "xx0000");
            stringBuilder.Insert(stringBuilder.Length, "x", 2);
            Assert.Equal(stringBuilder.ToString(), "xx0000xx");
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
            Assert.Equal(stringBuilder.ToString(), "xx_xx");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_1()
        {
            stringBuilder.Clear(); string testString = "BEGIN";
            //stringBuilder.Append("BEGIN_MID_END");
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
            Assert.Equal(stringBuilder.ToString(), "xx_MID_END");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_2()
        {
            string testString = "MID";
            stringBuilder.Replace(testString, "xx");
            Assert.Equal(stringBuilder.ToString(), "xx_xx_END");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_3()
        {
            string testString = "END";
            stringBuilder.Replace(testString, "xx");
            Assert.Equal(stringBuilder.ToString(), "xx_xx_xx");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_4()
        {
            string testString = "The quick br!wn d#g jumps #ver the lazy cat.";
            stringBuilder = new System.Text.StringBuilder(testString);
            stringBuilder.Replace('#', '!', 15, 29);        // Some '#' -> '!'
            Assert.Equal(stringBuilder.ToString(), "The quick br!wn d!g jumps !ver the lazy cat.");
            stringBuilder.Replace('!', 'o');                // All '!' -> 'o'
            Assert.Equal(stringBuilder.ToString(), "The quick brown dog jumps over the lazy cat.");
            stringBuilder.Replace("cat", "dog");            // All "cat" -> "dog"
            Assert.Equal(stringBuilder.ToString(), "The quick brown dog jumps over the lazy dog.");
            stringBuilder.Replace("dog", "fox", 15, 20);    // Some "dog" -> "fox"
            Assert.Equal(stringBuilder.ToString(), "The quick brown fox jumps over the lazy dog.");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_5()
        {
            stringBuilder.Clear();
            stringBuilder.Append("12345");
            stringBuilder.Replace("45", "def");
            Assert.Equal(stringBuilder.ToString(), "123def");
        }

        [TestMethod]
        public void Test_3_ReplaceTest_6()
        {
            stringBuilder.Clear();

            stringBuilder.Append("[{1234}]Test}]");
            stringBuilder.Replace("}]", "}]example");
            Assert.Equal(stringBuilder.ToString(), "[{1234}]exampleTest}]example");
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

                Assert.Equal(sMFOutput, sNativeOutput);
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
            Assert.Equal(stringBuilder.ToString(), string.Empty); string testString = "0000";
            stringBuilder.Append(string.Empty);
            stringBuilder.Append(testString);
            stringBuilder.Append(string.Empty);
            //should allocate here
            stringBuilder.Append("_");
            stringBuilder.Append("_");
            //result is true if capacity is > 5
            Assert.True(stringBuilder.Capacity > 5);
        }

        [TestMethod]
        public void Test_5_ToStringLengthTest_0()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(0, 5);
            Assert.Equal("Hello", outStr);
        }

        [TestMethod]
        public void Test_5_ToStringLengthTest_1()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(0, 25);
            Assert.Equal("Hello from nanoFramework!", outStr);
        }

        [TestMethod]
        public void Test_5_ToStringStartIndexLengthTest_0()
        {
            stringBuilder.Clear();
            stringBuilder.Capacity = 16;
            stringBuilder.Append("Hello from nanoFramework!");
            var outStr = stringBuilder.ToString(11, 13);
            Assert.Equal("nanoFramework", outStr);
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
            Assert.Equal($"{testSize}{testStr}", sbvariable.ToString());
            Assert.Equal($"{testSize}{testStr}", sbfixed.ToString());            
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
            Assert.Equal(testStrCompiled, sbfixed.ToString());

            // Act
            for(int i=0; i<20; i++)
            {
                sbfixed.Append(testStr);
                testStrCompiled += testStr;
            }

            // Assert
            Assert.Equal(testStrCompiled, sbfixed.ToString());
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

