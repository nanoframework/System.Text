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
    public class EncodingTests
    {
        [TestMethod]
        public void Utf8EncodingTests_Test1()
        {
            string str = "this is a normal string that will be used to convert to bytes then back to a string";

            byte[] data = new byte[128];
            int len = str.Length;
            int idx = 0;

            Random rand = new Random();

            int cBytes = 0;

            while (len > 0)
            {
                int size = (len <= 2) ? len : rand.Next(len / 2) + 1;
                len -= size;

                int cnt = Encoding.UTF8.GetBytes(str, idx, size, data, cBytes);

                Assert.AreEqual(str.Substring(idx, size), new string(Encoding.UTF8.GetChars(data, cBytes, cnt)));

                cBytes += cnt;
                idx += size;
            }
            Assert.AreEqual(cBytes, str.Length);
            string strAfter = new string(Encoding.UTF8.GetChars(data, 0, cBytes));
            Assert.AreEqual(str, strAfter);
        }

        [TestMethod]
        public void Utf8EncodingTests_Test2()
        {
            string str = "this is a normal string that will be used to convert to bytes then back to a string";
            byte[] data = Encoding.UTF8.GetBytes(str);
            Assert.AreEqual(data.Length, str.Length);
            string strAfter = new string(Encoding.UTF8.GetChars(data));
            Assert.AreEqual(str, strAfter);
        }

        [TestMethod]
        public void Utf8EncodingTests_Test3()
        {
            // This tests involves a string with a special character
            string str = "AB\u010DAB";
            byte[] data = new byte[4];
            int count = Encoding.UTF8.GetBytes(str, 1, 3, data, 0);
            Assert.AreEqual(4, count);
            Assert.AreEqual("B\u010DA", new string(Encoding.UTF8.GetChars(data)));
        }
    }
}
