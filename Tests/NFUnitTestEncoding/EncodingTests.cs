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

        private void RoundtripUtf8(byte[] input, byte[] expected, int expectedStringLength)
        {
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            Assert.AreEqual(expectedStringLength, decoded.Length);
            CollectionAssert.AreEqual(expected, reencoded, $"Failed on input: {BitConverter.ToString(input)}");
        }

        [TestMethod]
        public void Utf8EncodingTests_TestValid2ByteSequence()
        {
            byte[] input = new byte[] { 0xC2, 0xA9 }; // U+00A9 ©
            byte[] expected = new byte[] { 0xC2, 0xA9 };
            RoundtripUtf8(input, expected, 1);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestValid3ByteSequence()
        {
            byte[] input = new byte[] { 0xE2, 0x82, 0xAC }; // U+20AC €
            byte[] expected = new byte[] { 0xE2, 0x82, 0xAC };
            RoundtripUtf8(input, expected, 1);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestValid4ByteSequence()
        {
            byte[] input = new byte[] { 0xF0, 0x9F, 0x98, 0x80 }; // U+1F600 😀
            byte[] expected = new byte[] { 0xF0, 0x9F, 0x98, 0x80 };
            RoundtripUtf8(input, expected, 2);
        }


        [TestMethod]
        public void Utf8EncodingTests_TestOverlongEncoding1()
        {
            // Overlong encoding of '/' - each byte is invalid and produces one replacement character
            byte[] input = new byte[] { 0xC0, 0xAF }; // Overlong '/'
                                                      // 0xC0 is invalid starter, 0xAF is standalone continuation byte (also invalid)
                                                      // Expected: one � for the invalid starter, one � for the standalone continuation
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 1);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestOverlongEncoding2()
        {
            // Overlong encoding - the lead byte consumes valid continuations, producing one �
            byte[] input = new byte[] { 0xE0, 0x80, 0xAF }; // Overlong '/'
                                                            // All bytes consumed together as one invalid overlong sequence
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 1);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalid3ByteLastByteInvalid()
        {
            byte[] input = new byte[] { 0xE2, 0x82, 0xFE }; // UTF-8 3 bytes, 0xFE is the invalid character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 2);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalid3ByteMiddleByteInvalid()
        {
            byte[] input = new byte[] { 0xE2, 0xFE, 0xAC }; // UTF-8 3 bytes, 0xFE is the invalid character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 3);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalid4ByteLastByteInvalid()
        {
            byte[] input = new byte[] { 0xF0, 0x9F, 0x98, 0xFE }; // UTF-8 4 bytes, 0xFE is the invalid character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 2);
        }

        //  0xF0, 0x9F, 0x98, 0x80
        [TestMethod]
        public void Utf8EncodingTests_TestInvalid4ByteThirdByteInvalid()
        {
            byte[] input = new byte[] { 0xF0, 0x9F, 0xFE, 0x80 };  // UTF-8 4 bytes, 0xFE is the invalid character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 3);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalid4ByteSecondByteInvalid()
        {
            byte[] input = new byte[] { 0xF0, 0xFE, 0x98, 0x80 }; // UTF-8 4 bytes, 0xFE is the invalid character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 4);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestIsolatedContinuationByte()
        {
            byte[] input = new byte[] { 0x80 }; // Invalid lone continuation byte
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            CollectionAssert.AreEqual(new byte[] { 0xEF, 0xBF, 0xBD }, reencoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestIllegalCodePositionSurrogate()
        {
            byte[] input = new byte[] { 0xED, 0xA0, 0x80 }; // U+D800 high surrogate
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            // All three bytes consumed together as one invalid sequence
            CollectionAssert.AreEqual(new byte[] { 0xEF, 0xBF, 0xBD }, reencoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestMaximumValidCodepoint()
        {
            byte[] input = new byte[] { 0xF4, 0x8F, 0xBF, 0xBF }; // U+10FFFF
            byte[] expected = new byte[] { 0xF4, 0x8F, 0xBF, 0xBF };
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            RoundtripUtf8(input, expected, 2);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestCodepointBeyondUPlus10FFFF()
        {
            byte[] input = new byte[] { 0xF4, 0x90, 0x80, 0x80 }; // > U+10FFFF
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            // All 4 bytes consumed together as one invalid sequence
            CollectionAssert.AreEqual(new byte[] { 0xEF, 0xBF, 0xBD }, reencoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestUnexpectedContinuationSequence()
        {
            byte[] input = new byte[] { 0xC2, 0x41 }; // Valid 0xC2 (start of 2-byte), but 0x41 (A) not valid continuation
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            // Expect: �A
            CollectionAssert.AreEqual(new byte[] { 0xEF, 0xBF, 0xBD, 0x41 }, reencoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestASCII()
        {
            byte[] input = new byte[] { 0x47, 0x6F, 0x6F, 0x64 }; // "Good"
            byte[] expected = new byte[] { 0x47, 0x6F, 0x6F, 0x64 };
            RoundtripUtf8(input, expected, 4);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestASCIIWithInvalidByte()
        {
            byte[] input = new byte[] { 0x42, 0x41, 0x44, 0xEF, 0xFF }; // BAD��
            byte[] expected = new byte[] { 0x42, 0x41, 0x44, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 5);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestASCIIWithInvalidByteInBetween()
        {
            byte[] input = new byte[] { 0x42, 0x41, 0x44, 0xEF, 0xFF, 0x42, 0x41, 0x44 }; // BAD��BAD
            byte[] expected = new byte[] { 0x42, 0x41, 0x44, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0x42, 0x41, 0x44 };
            RoundtripUtf8(input, expected, 8);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestASCIIWithNull()
        {
            // All characters will be dropped after the null character in nanoFramework
            byte[] input = new byte[] { 0x54, 0x65, 0x73, 0x74, 0x20, 0x6E, 0x75, 0x6C, 0x6C, 0x00,
                                     0x61, 0x66, 0x74, 0x65, 0x72, 0x20, 0x6E, 0x75, 0x6C, 0x6C }; // Test null\0after null
            byte[] expected = new byte[] { 0x54, 0x65, 0x73, 0x74, 0x20, 0x6E, 0x75, 0x6C, 0x6C };
            RoundtripUtf8(input, expected, 9);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestStringWithEmoji()
        {
            string strInput = "nanoFramework is fantastic 🚀";
            var input = Encoding.UTF8.GetBytes(strInput);
            byte[] expected = new byte[] { 0x6E, 0x61, 0x6E, 0x6F, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x20, 0x69,
                         0x73, 0x20, 0x66, 0x61, 0x6E, 0x74, 0x61, 0x73, 0x74, 0x69, 0x63, 0x20, 0xF0, 0x9F, 0x9A, 0x80 };
            RoundtripUtf8(input, expected, 29);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestFullASCIIRange()
        {
            // Full ASCII Range except the null character
            byte[] input = new byte[]
            {
                 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
           0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
           0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
           0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
           0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
           0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
           0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
           0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
           0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47,
           0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
           0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57,
           0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F,
           0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67,
           0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F,
           0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77,
           0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F
            };
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);
            RoundtripUtf8(reencoded, input, 127);
        }

        // NEW TESTS FOR FIXES

        [TestMethod]
        public void Utf8EncodingTests_TestIncrementalDecodingExactBuffer()
        {
            // Test the fix for exact buffer size (no space for null terminator)
            // This was the original issue where iMaxChars=1, outputUTF16_size=1
            string testString = "AB";
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(testString);

            // Decode one character at a time with exact buffer
            char[] outputChars = new char[1];
            int bytesUsed, charsUsed;
            bool completed;

            var decoder = Encoding.UTF8.GetDecoder();
            decoder.Convert(utf8Bytes, 0, 1, outputChars, 0, 1, false, out bytesUsed, out charsUsed, out completed);

            Assert.AreEqual(1, bytesUsed);
            Assert.AreEqual(1, charsUsed);
            Assert.AreEqual('A', outputChars[0]);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalidSurrogatePairHandling()
        {
            // Test UTF-16 to UTF-8 conversion with invalid surrogate pairs
            // High surrogate (0xD800) followed by a regular character 'A' (0x41)
            // The high surrogate should be replaced with U+FFFD and 'A' should be preserved

            // Create string with high surrogate followed by 'A'
            char[] chars = new char[] { (char)0xD800, 'A', 'B' };
            string testString = new string(chars);

            byte[] encoded = Encoding.UTF8.GetBytes(testString);

            // Expect: U+FFFD (0xEF 0xBF 0xBD) + 'A' (0x41) + 'B' (0x42)
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0x41, 0x42 };
            CollectionAssert.AreEqual(expected, encoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestInvalidSurrogatePairMiddle()
        {
            // High surrogate followed by another high surrogate
            char[] chars = new char[] { 'A', (char)0xD800, (char)0xD801, 'B' };
            string testString = new string(chars);

            byte[] encoded = Encoding.UTF8.GetBytes(testString);

            // Expect: 'A' (0x41) + U+FFFD (0xEF 0xBF 0xBD) + U+FFFD (0xEF 0xBF 0xBD) + 'B' (0x42)
            byte[] expected = new byte[] { 0x41, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0x42 };
            CollectionAssert.AreEqual(expected, encoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestUnpairedLowSurrogate()
        {
            // Low surrogate without preceding high surrogate
            char[] chars = new char[] { 'A', (char)0xDC00, 'B' };
            string testString = new string(chars);

            byte[] encoded = Encoding.UTF8.GetBytes(testString);

            // Expect: 'A' (0x41) + U+FFFD (0xEF 0xBF 0xBD) + 'B' (0x42)
            byte[] expected = new byte[] { 0x41, 0xEF, 0xBF, 0xBD, 0x42 };
            CollectionAssert.AreEqual(expected, encoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestUnpairedHighSurrogateAtEnd()
        {
            // High surrogate at the end of input
            char[] chars = new char[] { 'A', 'B', (char)0xD800 };
            string testString = new string(chars);

            byte[] encoded = Encoding.UTF8.GetBytes(testString);

            // Expect: 'A' (0x41) + 'B' (0x42) + U+FFFD (0xEF 0xBF 0xBD)
            byte[] expected = new byte[] { 0x41, 0x42, 0xEF, 0xBF, 0xBD };
            CollectionAssert.AreEqual(expected, encoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestPartial2ByteSequence()
        {
            // Start of 2-byte sequence without continuation byte
            byte[] input = new byte[] { 0x41, 0xC2 }; // 'A' followed by incomplete 2-byte sequence
            byte[] expected = new byte[] { 0x41, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 2);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestPartial3ByteSequence()
        {
            // Start of 3-byte sequence with only 1 continuation byte
            byte[] input = new byte[] { 0x41, 0xE2, 0x82 }; // 'A' followed by incomplete 3-byte sequence
                                                            // 'A' + one � for the incomplete sequence (lead byte + 1 valid continuation consumed together)
            byte[] expected = new byte[] { 0x41, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 2);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestPartial4ByteSequence()
        {
            // Start of 4-byte sequence with only 2 continuation bytes
            byte[] input = new byte[] { 0x41, 0xF0, 0x9F, 0x98 }; // 'A' followed by incomplete 4-byte sequence
                                                                  // 'A' + one � for the incomplete sequence (lead byte + 2 valid continuations consumed together)
            byte[] expected = new byte[] { 0x41, 0xEF, 0xBF, 0xBD };
            RoundtripUtf8(input, expected, 2);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestMixedValidAndInvalidSequences()
        {
            // Mix of valid and invalid sequences
            byte[] input = new byte[]
                   {
     0x41,   // 'A' - valid ASCII
 0xC2, 0xA9,       // © - valid 2-byte
             0xE2, 0x82,  // incomplete 3-byte
             0x42,             // 'B' - valid ASCII
       0xF0, 0x9F, 0x98, 0x80, // 😀 - valid 4-byte
        0xED, 0xA0, 0x80// invalid surrogate
                   };

            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            Assert.IsNotNull(decoded);
            Assert.IsTrue(decoded.Contains("A"));
            Assert.IsTrue(decoded.Contains("©"));
            Assert.IsTrue(decoded.Contains("B"));
        }

        [TestMethod]
        public void Utf8EncodingTests_TestValidSurrogatePair()
        {
            // Test proper handling of valid surrogate pairs
            // 😀 (U+1F600) should encode to F0 9F 98 80 and decode back correctly
            string emoji = "😀";
            byte[] encoded = Encoding.UTF8.GetBytes(emoji);

            byte[] expected = new byte[] { 0xF0, 0x9F, 0x98, 0x80 };
            CollectionAssert.AreEqual(expected, encoded);

            string decoded = Encoding.UTF8.GetString(encoded, 0, encoded.Length);
            Assert.AreEqual(emoji, decoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestMultipleSurrogatePairs()
        {
            // Multiple emoji/surrogate pairs in sequence
            string emojis = "😀😁😂";
            byte[] encoded = Encoding.UTF8.GetBytes(emojis);
            string decoded = Encoding.UTF8.GetString(encoded, 0, encoded.Length);
            Assert.AreEqual(emojis, decoded);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestIncrementalDecodingMultiByte()
        {
            // Test incremental decoding of multi-byte sequences
            byte[] utf8 = new byte[] { 0xE2, 0x82, 0xAC }; // €

            char[] output = new char[1];
            int bytesUsed, charsUsed;
            bool completed;

            var decoder = Encoding.UTF8.GetDecoder();
            decoder.Convert(utf8, 0, 3, output, 0, 1, false, out bytesUsed, out charsUsed, out completed);

            Assert.AreEqual(3, bytesUsed);
            Assert.AreEqual(1, charsUsed);
            Assert.AreEqual('€', output[0]);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestOverlongEncodingRejection()
        {
            // Ensure overlong encodings are rejected and replaced with U+FFFD
            // Overlong encoding of 'A' (should be 0x41, not C1 81)
            byte[] input = new byte[] { 0xC1, 0x81 };
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);

            // Should produce replacement characters
            Assert.AreNotEqual(input.Length, reencoded.Length);
            Assert.IsTrue(decoded.Contains("\uFFFD"));
        }

        [TestMethod]
        public void Utf8EncodingTests_TestSequentialInvalidBytes()
        {
            // Multiple sequential invalid bytes
            byte[] input = new byte[] { 0xFE, 0xFF, 0xFE };
            string decoded = Encoding.UTF8.GetString(input, 0, input.Length);
            byte[] reencoded = Encoding.UTF8.GetBytes(decoded);

            // Each invalid byte should become one replacement character
            byte[] expected = new byte[] { 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD, 0xEF, 0xBF, 0xBD };
            CollectionAssert.AreEqual(expected, reencoded);
            Assert.AreEqual(3, decoded.Length);
        }

        [TestMethod]
        public void Utf8EncodingTests_TestBoundaryCodepoints()
        {
            // Test boundary values for different UTF-8 sequence lengths

            // U+007F - last 1-byte character
            byte[] input1 = new byte[] { 0x7F };
            RoundtripUtf8(input1, input1, 1);

            // U+0080 - first 2-byte character  
            byte[] input2 = new byte[] { 0xC2, 0x80 };
            RoundtripUtf8(input2, input2, 1);

            // U+07FF - last 2-byte character
            byte[] input3 = new byte[] { 0xDF, 0xBF };
            RoundtripUtf8(input3, input3, 1);

            // U+0800 - first 3-byte character
            byte[] input4 = new byte[] { 0xE0, 0xA0, 0x80 };
            RoundtripUtf8(input4, input4, 1);

            // U+FFFF - last 3-byte character (excluding surrogates)
            byte[] input5 = new byte[] { 0xEF, 0xBF, 0xBF };
            RoundtripUtf8(input5, input5, 1);
        }
    }
}
