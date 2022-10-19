
using nanoFramework.Benchmark;
using nanoFramework.Benchmark.Attributes;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace nanoFramework.System.Text.Benchmark
{
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark_10 : StringBenchmark
    {
        public StringBenchmark_10() : base(10) { }
    }
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark_100 : StringBenchmark
    {
        public StringBenchmark_100() : base(100) { }
    }
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark_1000 : StringBenchmark
    {
        public StringBenchmark_1000() : base(1000) { }
    }
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark_10000 : StringBenchmark
    {
        public StringBenchmark_10000() : base(10000) { }
    }

    [DebugLogger]
    [ConsoleParser]
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark 
    {
        public const int Iterations = 100;

        int _stringLength = 1;
        char[] _tokenCharArray;
        char[] _tokenCharArray2;
        private char[] _tokenCharArray3;
        private object _tokenCharArrayObject;
        string _tokenString;
        private string _tokenString2;
        private string _tokenString3;
        private string _tokenString4;
        object[] _tokenObjectArray;
        string[] _tokenStringArray;

        public StringBenchmark()
        {
            Setup();
        }
        public StringBenchmark(int stringLength)
        {
            _stringLength = stringLength;
            Setup();
        }
        void Setup()
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
            var test = new String(_tokenCharArray);
        }

        [Benchmark]
        public void Ctor_Test_1()
        {
            var test = new String(_tokenCharArray[0], _stringLength);
        }

        [Benchmark]
        public void Ctor_Test_2()
        {
            var test = new String(_tokenCharArray, 0, _stringLength);
        }

        [Benchmark]
        public void Compare_Test()
        {
            String token1 = new String(_tokenCharArray);
            String token2 = new String(_tokenCharArray2);
            var test = String.Compare(token1, token2);
        }

        [Benchmark]
        public void Concat_Test_0()
        {
            var test = String.Concat((object)_tokenCharArray);
        }

        [Benchmark]
        public void Concat_Test_1()
        {
            var test = String.Concat(_tokenObjectArray);
        }

        [Benchmark]
        public void Concat_Test_2()
        {
            var test = String.Concat(_tokenStringArray);
        }

        [Benchmark]
        public void Concat_Test_3()
        {
            var test = String.Concat(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Concat_Test_4()
        {
            var test = String.Concat(_tokenString, _tokenString2);
        }

        [Benchmark]
        public void Concat_Test_5()
        {
            var test = String.Concat(_tokenCharArray, _tokenCharArray2, _tokenCharArray3);
        }

        [Benchmark]
        public void Concat_Test_6()
        {
            var test = String.Concat(_tokenString, _tokenString2, _tokenString3);
        }

        [Benchmark]
        public void Concat_Test_7()
        {
            var test = String.Concat(_tokenString, _tokenString2, _tokenString3, _tokenString4);
        }

        [Benchmark]
        public void Empty_Test()
        {
            var test = String.Empty;
        }

        [Benchmark]
        public void Equals_Test_0()
        {
            var test = String.Equals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Equals_Test_1()
        {
            var test = String.Equals(_tokenString, _tokenString2);
        }
        [Benchmark]
        public void Equals_Test_2()
        {
            var test = _tokenString.Equals(_tokenCharArrayObject);
        }

        [Benchmark]
        public void Intern_Test()
        {
            var test = String.Intern(_tokenString);
        }

        [Benchmark]
        public void IsInterned_Test()
        {
            var test = String.IsInterned(_tokenString2);
        }

        [Benchmark]
        public void ReferenceEquals_Test()
        {
            var test = String.ReferenceEquals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void CompareTo_Test0()
        {
            var test = _tokenString.CompareTo(_tokenCharArrayObject);
        }

        [Benchmark]
        public void CompareTo_Test1()
        {
            var test = _tokenString.CompareTo(_tokenString2);
        }

        [Benchmark]
        public void GetHashCode_Test()
        {
            var test = _tokenString.GetHashCode();
        }

        [Benchmark]
        public void GetType_Test()
        {
            var test = _tokenString.GetType();
        }

        [Benchmark]
        public void IndexOf_Test_0()
        {
            var test = _tokenString.IndexOf(_tokenCharArray[0]);
        }

        [Benchmark]
        public void IndexOf_Test_1()
        {
            var test = _tokenString.IndexOf(_tokenString2);
        }

        [Benchmark]
        public void IndexOf_Test_2()
        {
            var test = _tokenString.IndexOf(_tokenCharArray[0], 0);
        }

        [Benchmark]
        public void IndexOf_Test_3()
        {
            var test = _tokenString.IndexOf(_tokenString2, 0);
        }

        [Benchmark]
        public void IndexOf_Test_4()
        {
            var test = _tokenString.IndexOf(_tokenCharArray[0], 0, 1);
        }

        [Benchmark]
        public void IndexOf_Test_5()
        {
            var test = _tokenString.IndexOf(_tokenString2, 0, 1);
        }

        [Benchmark]
        public void IndexOfAny_Test_0()
        {
            var test = _tokenString.IndexOfAny(_tokenCharArray);
        }

        [Benchmark]
        public void IndexOfAny_Test_1()
        {
            var test = _tokenString.IndexOfAny(_tokenCharArray, 0);
        }

        [Benchmark]
        public void IndexOfAny_Test_2()
        {
            var test = _tokenString.IndexOfAny(_tokenCharArray, 0, 1);
        }

        [Benchmark]
        public void LastIndexOf_Test_0()
        {
            var test = _tokenString.LastIndexOf(_tokenCharArray[0]);
        }

        [Benchmark]
        public void LastIndexOf_Test_1()
        {
            var test = _tokenString.LastIndexOf(_tokenString2);
        }

        [Benchmark]
        public void LastIndexOf_Test_2()
        {
            var test = _tokenString.LastIndexOf(_tokenCharArray[0], 0);
        }

        [Benchmark]
        public void LastIndexOf_Test_3()
        {
            var test = _tokenString.LastIndexOf(_tokenString2, 0);
        }

        [Benchmark]
        public void LastIndexOf_Test_4()
        {
            var test = _tokenString.LastIndexOf(_tokenCharArray[0], 0, 1);
        }

        [Benchmark]
        public void LastIndexOf_Test_5()
        {
            var test = _tokenString.LastIndexOf(_tokenString2, 0, 1);
        }

        [Benchmark]
        public void Length_Test()
        {
            var test = _tokenString.Length;
        }

        [Benchmark]
        public void Split_Test_0()
        {
            var test = _tokenString.Split(_tokenCharArray[0]);
        }

        [Benchmark]
        public void SubString_Test_0()
        {
            var test = _tokenString.Substring(0);
        }

        [Benchmark]
        public void SubString_Test_1()
        {
            var test = _tokenString.Substring(0, 1);
        }

        [Benchmark]
        public void ToCharArray_Test_0()
        {
            var test = _tokenString.ToCharArray();
        }

        [Benchmark]
        public void ToCharArray_Test_1()
        {
            var test = _tokenString.ToCharArray(0, 1);
        }

        [Benchmark]
        public void ToLower_Test()
        {
            var test = _tokenString.ToLower();
        }

        [Benchmark]
        public void ToString_Test()
        {
            var test = _tokenString.ToString();
        }

        [Benchmark]
        public void ToUpper_Test()
        {
            var test = _tokenString.ToUpper();
        }

        [Benchmark]
        public void Trim_Test_0()
        {
            var test = _tokenString.Trim();
        }

        [Benchmark]
        public void Trim_Test_1()
        {
            var test = _tokenString.Trim(_tokenCharArray);
        }

        [Benchmark]
        public void TrimEnd_Test()
        {
            var test = _tokenString.TrimEnd();
        }

        [Benchmark]
        public void TrimStart_Test()
        {
            var test = _tokenString.TrimStart();
        }


        private char[] GetTokenCharArray(int length)
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
        private object[] GetTokenObjectArray(int length)
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

        private string[] GetTokenStringArray(int length)
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
