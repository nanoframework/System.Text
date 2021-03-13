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
    class EncodingTests
    {
        [TestMethod]
        public void Utf8EncodingTests_Test1()
        {
            string str = "this is a normal string that will be used to convert to bytes then back to a string";
            byte[] data = new byte[128];
            int len = str.Length;
            int idx = 0;
            Random rand = new System.Random();
            int cBytes = 0;

            while (len > 0)
            {
                int size = (len <= 2) ? len : rand.Next(len / 2) + 1;
                len -= size;

                int cnt = UTF8Encoding.UTF8.GetBytes(str, idx, size, data, cBytes);

                Assert.Equal(str.Substring(idx, size), new string(UTF8Encoding.UTF8.GetChars(data, cBytes, cnt)));

                cBytes += cnt;
                idx += size;
            }
            Assert.Equal(cBytes, str.Length);
            string strAfter = new string(UTF8Encoding.UTF8.GetChars(data, 0, cBytes));
            Assert.Equal(str, strAfter);
        }

        [TestMethod]
        public void Utf8EncodingTests_Test2()
        {
            string str = "this is a normal string that will be used to convert to bytes then back to a string";
            byte[] data = UTF8Encoding.UTF8.GetBytes(str);
            Assert.Equal(data.Length, str.Length);
            string strAfter = new string(UTF8Encoding.UTF8.GetChars(data));
            Assert.Equal(str, strAfter);
        }

        [TestMethod]
        public void Utf8EncodingTests_Test3()
        {
            // This tests involves a string with a special character
            string str = "AB\u010DAB";
            byte[] data = new byte[4];
            int count = UTF8Encoding.UTF8.GetBytes(str, 1, 3, data, 0);
            Assert.Equal(count, 4);
            Assert.Equal(new string(UTF8Encoding.UTF8.GetChars(data)), "B\u010DA");
        }
    }
}