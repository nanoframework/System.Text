//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Benchmark;
using nanoFramework.Benchmark.Attributes;
using System;

namespace nanoFramework.System.Text.Benchmark
{

#pragma warning disable S101 // Types should be named in PascalCase

    [IterationCount(Iterations)]
    public class StringBenchmark_10 : StringBenchmark
    {
        public StringBenchmark_10() : base(10) { }
    }

    [IterationCount(Iterations)]
    public class StringBenchmark_100 : StringBenchmark
    {
        public StringBenchmark_100() : base(100) { }
    }

    [IterationCount(Iterations)]
    public class StringBenchmark_1000 : StringBenchmark
    {
        public StringBenchmark_1000() : base(1000) { }
    }

    [IterationCount(Iterations)]
    public class StringBenchmark_10000 : StringBenchmark
    {
        public StringBenchmark_10000() : base(10000) { }
    }

    [DebugLogger]
    [ConsoleParser]
    [IterationCount(Iterations)]
    public class StringBenchmark
    {
        public const int Iterations = 100;
        private readonly int _stringLength = 1;
        private char[] _tokenCharArray;
        private char[] _tokenCharArray2;
        private char[] _tokenCharArray3;
        private object _tokenCharArrayObject;
        private string _tokenString;
        private string _tokenString2;
        private string _tokenString3;
        private string _tokenString4;
        private object[] _tokenObjectArray;
        private string[] _tokenStringArray;

        public StringBenchmark()
        {
            Setup();
        }

        public StringBenchmark(int stringLength)
        {
            _stringLength = stringLength;
            Setup();
        }

        private void Setup()
        {
            _tokenCharArray = GetTokenCharArray(_stringLength);
            _tokenCharArray2 = GetTokenCharArray(_stringLength);
            _tokenCharArray3 = GetTokenCharArray(_stringLength);
            _tokenCharArrayObject = (object)_tokenCharArray;
            _tokenString = new string(_tokenCharArray);
            _tokenString2 = new string(_tokenCharArray);
            _tokenString3 = new string(_tokenCharArray);
            _tokenString4 = new string(_tokenCharArray);
            _tokenObjectArray = GetTokenObjectArray(_stringLength);
            _tokenStringArray = GetTokenStringArray(_stringLength);
        }

        [Benchmark]
        public void Ctor_Test_0()
        {
            _ = new string(_tokenCharArray);
        }

        [Benchmark]
        public void Ctor_Test_1()
        {
            _ = new string(_tokenCharArray[0], _stringLength);
        }

        [Benchmark]
        public void Ctor_Test_2()
        {
            _ = new string(_tokenCharArray, 0, _stringLength);
        }

        [Benchmark]
        public void Compare_Test()
        {
            string token1 = new string(_tokenCharArray);
            string token2 = new string(_tokenCharArray2);
            _ = string.Compare(token1, token2);
        }

        [Benchmark]
        public void Concat_Test_0()
        {
            _ = string.Concat((object)_tokenCharArray);
        }

        [Benchmark]
        public void Concat_Test_1()
        {
            _ = string.Concat(_tokenObjectArray);
        }

        [Benchmark]
        public void Concat_Test_2()
        {
            _ = string.Concat(_tokenStringArray);
        }

        [Benchmark]
        public void Concat_Test_3()
        {
            _ = string.Concat(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Concat_Test_4()
        {
            _ = string.Concat(_tokenString, _tokenString2);
        }

        [Benchmark]
        public void Concat_Test_5()
        {
            _ = string.Concat(_tokenCharArray, _tokenCharArray2, _tokenCharArray3);
        }

        [Benchmark]
        public void Concat_Test_6()
        {
            _ = string.Concat(_tokenString, _tokenString2, _tokenString3);
        }

        [Benchmark]
        public void Concat_Test_7()
        {
            _ = string.Concat(_tokenString, _tokenString2, _tokenString3, _tokenString4);
        }

        [Benchmark]
        public void Empty_Test()
        {
            var test = string.Empty;
        }

        [Benchmark]
        public void Equals_Test_0()
        {
            _ = Equals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Equals_Test_1()
        {
            _ = string.Equals(_tokenString, _tokenString2);
        }
        [Benchmark]
        public void Equals_Test_2()
        {
            _ = _tokenString.Equals(_tokenCharArrayObject);
        }

        [Benchmark]
        public void Intern_Test()
        {
            _ = string.Intern(_tokenString);
        }

        [Benchmark]
        public void IsInterned_Test()
        {
            _ = string.IsInterned(_tokenString2);
        }

        [Benchmark]
        public void ReferenceEquals_Test()
        {
            _ = ReferenceEquals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void CompareTo_Test0()
        {
            _ = _tokenString.CompareTo(_tokenCharArrayObject);
        }

        [Benchmark]
        public void CompareTo_Test1()
        {
            _ = _tokenString.CompareTo(_tokenString2);
        }

        [Benchmark]
        public void GetHashCode_Test()
        {
            _ = _tokenString.GetHashCode();
        }

        [Benchmark]
        public void GetType_Test()
        {
            _ = _tokenString.GetType();
        }

        [Benchmark]
        public void IndexOf_Test_0()
        {
            _ = _tokenString.IndexOf(_tokenCharArray[0]);
        }

        [Benchmark]
        public void IndexOf_Test_1()
        {
            _ = _tokenString.IndexOf(_tokenString2);
        }

        [Benchmark]
        public void IndexOf_Test_2()
        {
            _ = _tokenString.IndexOf(_tokenCharArray[0], 0);
        }

        [Benchmark]
        public void IndexOf_Test_3()
        {
            _ = _tokenString.IndexOf(_tokenString2, 0);
        }

        [Benchmark]
        public void IndexOf_Test_4()
        {
            _ = _tokenString.IndexOf(_tokenCharArray[0], 0, 1);
        }

        [Benchmark]
        public void IndexOf_Test_5()
        {
            _ = _tokenString.IndexOf(_tokenString2, 0, 1);
        }

        [Benchmark]
        public void IndexOfAny_Test_0()
        {
            _ = _tokenString.IndexOfAny(_tokenCharArray);
        }

        [Benchmark]
        public void IndexOfAny_Test_1()
        {
            _ = _tokenString.IndexOfAny(_tokenCharArray, 0);
        }

        [Benchmark]
        public void IndexOfAny_Test_2()
        {
            _ = _tokenString.IndexOfAny(_tokenCharArray, 0, 1);
        }

        [Benchmark]
        public void LastIndexOf_Test_0()
        {
            _ = _tokenString.LastIndexOf(_tokenCharArray[0]);
        }

        [Benchmark]
        public void LastIndexOf_Test_1()
        {
            _ = _tokenString.LastIndexOf(_tokenString2);
        }

        [Benchmark]
        public void LastIndexOf_Test_2()
        {
            _ = _tokenString.LastIndexOf(_tokenCharArray[0], 0);
        }

        [Benchmark]
        public void LastIndexOf_Test_3()
        {
            _ = _tokenString.LastIndexOf(_tokenString2, 0);
        }

        [Benchmark]
        public void LastIndexOf_Test_4()
        {
            _ = _tokenString.LastIndexOf(_tokenCharArray[0], 0, 1);
        }

        [Benchmark]
        public void LastIndexOf_Test_5()
        {
            _ = _tokenString.LastIndexOf(_tokenString2, 0, 1);
        }

        [Benchmark]
        public void Length_Test()
        {
            _ = _tokenString.Length;
        }

        [Benchmark]
        public void Split_Test_0()
        {
            _ = _tokenString.Split(_tokenCharArray[0]);
        }

        [Benchmark]
        public void SubString_Test_0()
        {
            _ = _tokenString.Substring(0);
        }

        [Benchmark]
        public void SubString_Test_1()
        {
            _ = _tokenString.Substring(0, 1);
        }

        [Benchmark]
        public void ToCharArray_Test_0()
        {
            _ = _tokenString.ToCharArray();
        }

        [Benchmark]
        public void ToCharArray_Test_1()
        {
            _ = _tokenString.ToCharArray(0, 1);
        }

        [Benchmark]
        public void ToLower_Test()
        {
            _ = _tokenString.ToLower();
        }

        [Benchmark]
        public void ToString_Test()
        {
            _ = _tokenString.ToString();
        }

        [Benchmark]
        public void ToUpper_Test()
        {
            _ = _tokenString.ToUpper();
        }

        [Benchmark]
        public void Trim_Test_0()
        {
            _ = _tokenString.Trim();
        }

        [Benchmark]
        public void Trim_Test_1()
        {
            _ = _tokenString.Trim(_tokenCharArray);
        }

        [Benchmark]
        public void TrimEnd_Test()
        {
            _ = _tokenString.TrimEnd();
        }

        [Benchmark]
        public void TrimStart_Test()
        {
            _ = _tokenString.TrimStart();
        }

#pragma warning restore S101 // Types should be named in PascalCase

        private static char[] GetTokenCharArray(int length)
        {
            char[] token = new char[length];

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    token[i] = 'a';
                }
                else
                {
                    token[i] = '\u0066';
                }
            }

            return token;
        }

        private static object[] GetTokenObjectArray(int length)
        {
            object[] token = new object[length];

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    token[i] = 'a';
                }
                else
                {
                    token[i] = "\uD840DC99";
                }
            }

            return token;
        }

        private static string[] GetTokenStringArray(int length)
        {
            string[] token = new string[length];

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    token[i] = "a";
                }
                else
                {
                    token[i] = "\uD840DC99";
                }
            }

            return token;
        }
    }
}
