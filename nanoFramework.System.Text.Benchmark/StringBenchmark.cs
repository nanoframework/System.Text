
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
    //[IterationCount(StringBenchmark.Iterations)]
    //public class StringBenchmark_10000 : StringBenchmark
    //{
    //    public StringBenchmark_10000() : base(10000) { }
    //}

    [DebugLogger]
    [ConsoleParser]
    [IterationCount(StringBenchmark.Iterations)]
    public class StringBenchmark
    {
        // TODO - set iterations back to 100
        public const int Iterations = 1;

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
            var str0 = new String(_tokenCharArray);
        }

        [Benchmark]
        public void Ctor_Test_1()
        {
            var str1 = new String(_tokenCharArray[0], _stringLength);
        }

        [Benchmark]
        public void Ctor_Test_2()
        {
            var str2 = new String(_tokenCharArray, 0, _stringLength);
        }

        [Benchmark]
        public void Compare_Test()
        {
            String token1 = new String(_tokenCharArray);
            String token2 = new String(_tokenCharArray2);
            var compResult = String.Compare(token1, token2);
        }

        [Benchmark]
        public void Concat_Test_0()
        {
            String concatString0;
            concatString0 = String.Concat((object)_tokenCharArray);
        }

        [Benchmark]
        public void Concat_Test_1()
        {
            String concatString1;
            concatString1 = String.Concat(_tokenObjectArray);
        }

        [Benchmark]
        public void Concat_Test_2()
        {
            String concatString2;
            concatString2 = String.Concat(_tokenStringArray);
        }

        [Benchmark]
        public void Concat_Test_3()
        {
            String concatString3;
            concatString3 = String.Concat(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Concat_Test_4()
        {
            String concatString4;
            concatString4 = String.Concat(_tokenString, _tokenString2);
        }

        [Benchmark]
        public void Concat_Test_5()
        {
            String concatString5;
            concatString5 = String.Concat(_tokenCharArray, _tokenCharArray2, _tokenCharArray3);
        }

        [Benchmark]
        public void Concat_Test_6()
        {
            String concatString6;
            concatString6 = String.Concat(_tokenString, _tokenString2, _tokenString3);
        }

        [Benchmark]
        public void Concat_Test_7()
        {
            String concatString7;
            concatString7 = String.Concat(_tokenString, _tokenString2, _tokenString3, _tokenString4);
        }

        [Benchmark]
        public void Empty_Test()
        {
            var test = String.Empty;
        }

        [Benchmark]
        public void Equals_Test_0()
        {
            String.Equals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void Equals_Test_1()
        {
            String.Equals(_tokenString, _tokenString2);
        }
        [Benchmark]
        public void Equals_Test_2()
        {
            _tokenString.Equals(_tokenCharArrayObject);
        }

        [Benchmark]
        public void Intern_Test()
        {
            String.Intern(_tokenString);
        }

        [Benchmark]
        public void IsInterned_Test()
        {
            String.IsInterned(_tokenString2);
        }

        [Benchmark]
        public void ReferenceEquals_Test()
        {
            String.ReferenceEquals(_tokenCharArray, _tokenCharArray2);
        }

        [Benchmark]
        public void CompareTo_Test0()
        {
            _tokenString.CompareTo(_tokenCharArrayObject);
        }

        [Benchmark]
        public void CompareTo_Test1()
        {
            _tokenString.CompareTo(_tokenString2);
        }

        [Benchmark]
        public void GetHashCode_Test()
        {
            _tokenString.GetHashCode();
        }

        [Benchmark]
        public void GetType_Test()
        {
            _tokenString.GetType();
        }

        [Benchmark]
        public void IndexOf_Test_0()
        {
            _tokenString.IndexOf(_tokenCharArray[0]);
        }

        [Benchmark]
        public void IndexOf_Test_1()
        {
            _tokenString.IndexOf(_tokenString2);
        }

        [Benchmark]
        public void IndexOf_Test_2()
        {
            _tokenString.IndexOf(_tokenCharArray[0], 0);
        }

        [Benchmark]
        public void IndexOf_Test_3()
        {
            _tokenString.IndexOf(_tokenString2, 0);
        }

        [Benchmark]
        public void IndexOf_Test_4()
        {
            _tokenString.IndexOf(_tokenCharArray[0], 0, 1);
        }

        [Benchmark]
        public void IndexOf_Test_5()
        {
            _tokenString.IndexOf(_tokenString2, 0, 1);
        }

        //[TestMethod]
        //public void IndexOfAny_Test_0()
        //{
        //    RunTest(StringTests.IndexOfAny0);
        //}

        //[TestMethod]
        //public void IndexOfAny_Test_1()
        //{
        //    RunTest(StringTests.IndexOfAny1);
        //}

        //[TestMethod]
        //public void IndexOfAny_Test_2()
        //{
        //    RunTest(StringTests.IndexOfAny2);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_0()
        //{
        //    RunTest(StringTests.LastIndexOf0);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_1()
        //{
        //    RunTest(StringTests.LastIndexOf1);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_2()
        //{
        //    RunTest(StringTests.LastIndexOf2);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_3()
        //{
        //    RunTest(StringTests.LastIndexOf3);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_4()
        //{
        //    RunTest(StringTests.LastIndexOf4);
        //}

        //[TestMethod]
        //public void LastIndexOf_Test_5()
        //{
        //    RunTest(StringTests.LastIndexOf5);
        //}

        //[TestMethod]
        //public void Length_Test()
        //{
        //    RunTest(StringTests.Length);
        //}

        //[TestMethod]
        //public void Split_Test_0()
        //{
        //    RunTest(StringTests.Split0);
        //}

        //[TestMethod]
        //public void Split_Test_1()
        //{
        //    RunTest(StringTests.Split1);
        //}

        //[TestMethod]
        //public void SubString_Test_0()
        //{
        //    RunTest(StringTests.SubString0);
        //}

        //[TestMethod]
        //public void SubString_Test_1()
        //{
        //    RunTest(StringTests.SubString1);
        //}

        //[TestMethod]
        //public void ToCharArray_Test_0()
        //{
        //    RunTest(StringTests.ToCharArray0);
        //}

        //[TestMethod]
        //public void ToCharArray_Test_1()
        //{
        //    RunTest(StringTests.ToCharArray1);
        //}

        //[TestMethod]
        //public void ToLower_Test()
        //{
        //    RunTest(StringTests.ToLower);
        //}

        //[TestMethod]
        //public void ToString_Test()
        //{
        //    RunTest(StringTests.ToString);
        //}

        //[TestMethod]
        //public void ToUpper_Test()
        //{
        //    RunTest(StringTests.ToUpper);
        //}

        //[TestMethod]
        //public void Trim_Test_0()
        //{
        //    RunTest(StringTests.Trim0);
        //}

        //[TestMethod]
        //public void Trim_Test_1()
        //{
        //    RunTest(StringTests.Trim1);
        //}

        //[TestMethod]
        //public void TrimEnd_Test()
        //{
        //    RunTest(StringTests.TrimEnd);
        //}

        //[TestMethod]
        //public void TrimStart_Test()
        //{
        //    RunTest(StringTests.TrimStart);
        //}
        //[TestMethod]
        //public void Test1()
        //{
        //    Assert.True(true);
        //}


        //private void RunTest(StringTests test)
        //{
        //    long start = 0, stop = 0;
        //    int maxLength = 3000;
        //    int iterations = 10;
        //    int[] time = new int[iterations];
        //    int currLength = 1;
        //    double duration = 0;

        //    while (currLength < maxLength + 1)
        //    {
        //        for (int i = 0; i < iterations; i++)
        //        {
        //            switch (test)
        //            {
        //                case StringTests.Ctor0:
        //                    String str0;
        //                    char[] token00 = GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    str0 = new String(token00);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Ctor1:
        //                    String str1;
        //                    char token01 = GetTokenCharArray(1)[0];
        //                    start = DateTime.UtcNow.Ticks;
        //                    str1 = new String(token01, currLength);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Ctor2:
        //                    String str2;
        //                    char[] token02 = GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    str2 = new String(token02, 0, currLength);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Compare:
        //                    int compResult;
        //                    String token11 = new String(GetTokenCharArray(currLength));
        //                    String token12 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    compResult = String.Compare(token11, token12);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat0:
        //                    String concatString0;
        //                    object token200 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString0 = String.Concat(token200);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat1:
        //                    String concatString1;
        //                    object[] token201 = GetTokenObjectArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString1 = String.Concat(token201);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat2:
        //                    String concatString2;
        //                    string[] token202 = GetTokenStringArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString2 = String.Concat(token202);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat3:
        //                    String concatString3;
        //                    object token203 = (object)GetTokenCharArray(currLength);
        //                    object token204 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString3 = String.Concat(token203, token204);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat4:
        //                    String concatString4;
        //                    String token205 = new String(GetTokenCharArray(currLength));
        //                    String token206 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString4 = String.Concat(token205, token206);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat5:
        //                    String concatString5;
        //                    object token207 = (object)GetTokenCharArray(currLength);
        //                    object token208 = (object)GetTokenCharArray(currLength);
        //                    object token209 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString5 = String.Concat(token207, token208, token209);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat6:
        //                    String concatString6;
        //                    String token210 = new String(GetTokenCharArray(currLength));
        //                    String token211 = new String(GetTokenCharArray(currLength));
        //                    String token212 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString6 = String.Concat(token210, token211, token212);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Concat7:
        //                    String concatString7;
        //                    String token213 = new String(GetTokenCharArray(currLength));
        //                    String token214 = new String(GetTokenCharArray(currLength));
        //                    String token215 = new String(GetTokenCharArray(currLength));
        //                    String token216 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    concatString7 = String.Concat(token213, token214, token215, token216);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Empty:
        //                    String token3;
        //                    start = DateTime.UtcNow.Ticks;
        //                    token3 = String.Empty;
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Equals0:
        //                    bool equalsResult0;
        //                    object token41 = (object)GetTokenCharArray(currLength);
        //                    object token42 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    equalsResult0 = String.Equals(token41, token42);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Equals1:
        //                    bool equalsResult1;
        //                    String token43 = new String(GetTokenCharArray(currLength));
        //                    String token44 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    equalsResult1 = String.Equals(token43, token44);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Equals2:
        //                    bool equalsResult2;
        //                    String token45 = new String(GetTokenCharArray(currLength));
        //                    object token46 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    equalsResult2 = token45.Equals(token46);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Intern:
        //                    string internString;
        //                    String token5 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    internString = String.Intern(token5);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IsInterned:
        //                    string isInternString;
        //                    String token6 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    isInternString = String.IsInterned(token6);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ReferenceEquals:
        //                    bool referenceEquals;
        //                    object token71 = (object)GetTokenCharArray(currLength);
        //                    object token72 = (object)GetTokenCharArray(currLength);
        //                    start = DateTime.UtcNow.Ticks;
        //                    referenceEquals = String.ReferenceEquals(token71, token72);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.CompareTo0:
        //                    int compareResult0;
        //                    object token80 = (object)GetTokenCharArray(currLength);
        //                    String token81 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    compareResult0 = token81.CompareTo(token80);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.CompareTo1:
        //                    int compareResult1;
        //                    String token82 = new String(GetTokenCharArray(currLength));
        //                    String token83 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    compareResult1 = token82.CompareTo(token83);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.GetHashCode:
        //                    int getHashCodeResult;
        //                    String token9 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    getHashCodeResult = token9.GetHashCode();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.GetType:
        //                    Type getTypeResult;
        //                    String tokenA = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    getTypeResult = tokenA.GetType();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf0:
        //                    int indexOfResult0;
        //                    String tokenB00 = new String(GetTokenCharArray(currLength));
        //                    char tokenB01 = GetTokenCharArray(currLength)[0];
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfResult0 = tokenB00.IndexOf(tokenB01);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf1:
        //                    int indexOfResult1;
        //                    String tokenB10 = new String(GetTokenCharArray(currLength));
        //                    String tokenB11 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    Console.WriteLine("Iteration: " + i);
        //                    Console.WriteLine("CurrentLength: " + currLength);
        //                    indexOfResult1 = tokenB10.IndexOf(tokenB11);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf2:
        //                    int indexOfResult2;
        //                    String tokenB20 = new String(GetTokenCharArray(currLength));
        //                    char tokenB21 = GetTokenCharArray(currLength)[0];
        //                    int tokenB22 = 0;// Math.Random(1);
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfResult2 = tokenB20.IndexOf(tokenB21, tokenB22);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf3:
        //                    int indexOfResult3;
        //                    String tokenB30 = new String(GetTokenCharArray(currLength));
        //                    String tokenB31 = new String(GetTokenCharArray(currLength));
        //                    int tokenB32 = 0;// Math.Random(1);
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfResult3 = tokenB30.IndexOf(tokenB31, tokenB32);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf4:
        //                    String tokenB40 = new String(GetTokenCharArray(currLength));
        //                    char tokenB41 = GetTokenCharArray(currLength)[0];
        //                    int tokenB42 = 0;// Math.Random(1);
        //                    int tokenB43;
        //                    if (currLength == 1)
        //                    {
        //                        tokenB43 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenB43 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    if (currLength == 1)
        //                    {
        //                        tokenB43 = 1;
        //                    }
        //                    start = DateTime.UtcNow.Ticks;
        //                    int indexOfResult4 = tokenB40.IndexOf(tokenB41, tokenB42, tokenB43);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOf5:
        //                    int indexOfResult5;
        //                    String tokenB50 = new String(GetTokenCharArray(currLength));
        //                    String tokenB51 = new String(GetTokenCharArray(currLength));
        //                    int tokenB52 = 0;// Math.Random(1);
        //                    int tokenB53;
        //                    if (currLength == 1)
        //                    {
        //                        tokenB53 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenB53 = 0;// Math.Random(currLength - 1);
        //                    }
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfResult5 = tokenB50.IndexOf(tokenB51, tokenB52, tokenB53);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOfAny0:
        //                    int indexOfAnyResult0;
        //                    char[] tokenC00 = GetTokenCharArray(currLength);
        //                    String tokenC01 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfAnyResult0 = tokenC01.IndexOfAny(tokenC00);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOfAny1:
        //                    int indexOfAnyResult1;
        //                    char[] tokenC10 = GetTokenCharArray(currLength);
        //                    String tokenC11 = new String(GetTokenCharArray(currLength));
        //                    int tokenC12 = 0;// Math.Random(1);                            
        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfAnyResult1 = tokenC11.IndexOfAny(tokenC10, tokenC12);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.IndexOfAny2:
        //                    int indexOfAnyResult2;
        //                    char[] tokenC20 = GetTokenCharArray(currLength);
        //                    String tokenC21 = new String(GetTokenCharArray(currLength));
        //                    int tokenC22 = 0;//Math.Random(1);
        //                    int tokenC23;
        //                    if (currLength == 1)
        //                    {
        //                        tokenC23 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenC23 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    if (currLength == 1)
        //                    {
        //                        tokenC23 = 1;
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    indexOfAnyResult2 = tokenC21.IndexOfAny(tokenC20, tokenC22, tokenC23);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf0:
        //                    String tokenD00 = new String(GetTokenCharArray(currLength));
        //                    char tokenD01 = GetTokenCharArray(currLength)[0];
        //                    start = DateTime.UtcNow.Ticks;
        //                    int lastindexOfResult0 = tokenD00.LastIndexOf(tokenD01);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf1:
        //                    int lastindexOfResult1;
        //                    String tokenD10 = new String(GetTokenCharArray(currLength));
        //                    String tokenD11 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfResult1 = tokenD10.LastIndexOf(tokenD11);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf2:
        //                    int lastindexOfResult2;
        //                    String tokenD20 = new String(GetTokenCharArray(currLength));
        //                    char tokenD21 = GetTokenCharArray(currLength)[0];
        //                    int tokenD22 = 0;//Math.Random(1);
        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfResult2 = tokenD20.LastIndexOf(tokenD21, tokenD22);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf3:
        //                    int lastindexOfResult3;
        //                    String tokenD30 = new String(GetTokenCharArray(currLength));
        //                    String tokenD31 = new String(GetTokenCharArray(currLength));
        //                    int tokenD32 = 0;//Math.Random(1);
        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfResult3 = tokenD30.LastIndexOf(tokenD31, tokenD32);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf4:
        //                    int lastindexOfResult4;
        //                    String tokenD40 = new String(GetTokenCharArray(currLength));
        //                    char tokenD41 = GetTokenCharArray(currLength)[0];
        //                    int tokenD42 = 0;//Math.Random(1);
        //                    int tokenD43;
        //                    if (currLength == 1)
        //                    {
        //                        tokenD43 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenD43 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfResult4 = tokenD40.LastIndexOf(tokenD41, tokenD42, tokenD43);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOf5:
        //                    String tokenD50 = new String(GetTokenCharArray(currLength));
        //                    String tokenD51 = new String(GetTokenCharArray(currLength));
        //                    int tokenD52 = 0;//Math.Random(1);
        //                    int tokenD53;
        //                    if (currLength == 1)
        //                    {
        //                        tokenD53 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenD53 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    int lastindexOfResult5 = tokenD50.LastIndexOf(tokenD51, tokenD52, tokenD53);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOfAny0:
        //                    int lastindexOfAnyResult0;
        //                    char[] tokenE00 = GetTokenCharArray(currLength);
        //                    String tokenE01 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfAnyResult0 = tokenE01.IndexOfAny(tokenE00);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOfAny1:
        //                    int lastindexOfAnyResult1;
        //                    char[] tokenE10 = GetTokenCharArray(currLength);
        //                    String tokenE11 = new String(GetTokenCharArray(currLength));
        //                    int tokenE12 = 0;//Math.Random(1);
        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfAnyResult1 = tokenE11.IndexOfAny(tokenE10, tokenE12);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.LastIndexOfAny2:
        //                    int lastindexOfAnyResult2;
        //                    char[] tokenE20 = GetTokenCharArray(currLength);
        //                    String tokenE21 = new String(GetTokenCharArray(currLength));
        //                    int tokenE22 = 0;//Math.Random(1);
        //                    int tokenE23;
        //                    if (currLength == 1)
        //                    {
        //                        tokenE23 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenE23 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    lastindexOfAnyResult2 = tokenE21.IndexOfAny(tokenE20, tokenE22, tokenE23);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Length:
        //                    int lengthResult;
        //                    String tokenF21 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    lengthResult = tokenF21.Length;
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Split0:
        //                    String[] splitResult0 = new String[currLength];
        //                    char[] tokenG00 = GetTokenCharArray(currLength);
        //                    String tokenG01 = new String(tokenG00);
        //                    start = DateTime.UtcNow.Ticks;
        //                    splitResult0 = tokenG01.Split(tokenG00);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Split1:
        //                    String[] splitResult1 = new String[currLength];
        //                    char[] tokenG10 = GetTokenCharArray(currLength);
        //                    String tokenG11 = new String(tokenG10);
        //                    start = DateTime.UtcNow.Ticks;
        //                    splitResult1 = tokenG11.Split(tokenG10);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.SubString0:
        //                    String subStringResult0;
        //                    int tokenH0 = 0;//Math.Random(1);
        //                    String tokenH1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    subStringResult0 = tokenH1.Substring(tokenH0);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.SubString1:
        //                    String subStringResult1;
        //                    int tokenH2 = 0;//Math.Random(1);
        //                    String tokenH3 = new String(GetTokenCharArray(currLength));
        //                    int tokenH4;
        //                    if (currLength == 1)
        //                    {
        //                        tokenH4 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenH4 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    subStringResult1 = tokenH3.Substring(tokenH2, tokenH4);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ToCharArray0:
        //                    char[] toCharArrayResult0 = new char[currLength];
        //                    String tokenI0 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    toCharArrayResult0 = tokenI0.ToCharArray();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ToCharArray1:
        //                    char[] toCharArrayResult1 = new char[currLength];
        //                    String tokenI1 = new String(GetTokenCharArray(currLength));
        //                    int tokenI2 = 0;//Math.Random(1);
        //                    int tokenI3;
        //                    if (currLength == 1)
        //                    {
        //                        tokenI3 = 1;
        //                    }
        //                    else
        //                    {
        //                        tokenI3 = 0;//Math.Random(currLength - 1);
        //                    }

        //                    start = DateTime.UtcNow.Ticks;
        //                    toCharArrayResult1 = tokenI1.ToCharArray(tokenI2, tokenI3);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ToLower:
        //                    string toLowerResult;
        //                    String tokenJ1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    toLowerResult = tokenJ1.ToLower();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ToString:
        //                    string toStringResult;
        //                    String tokenK1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    toStringResult = tokenK1.ToString();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.ToUpper:
        //                    string toUpperResult;
        //                    String tokenL1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    toUpperResult = tokenL1.ToUpper();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Trim0:
        //                    string trimResult0;
        //                    String tokenM0 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    trimResult0 = tokenM0.Trim();
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.Trim1:
        //                    string trimResult1;
        //                    char[] tokenM1 = new char[currLength];
        //                    String tokenM2 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    trimResult1 = tokenM2.Trim(tokenM1);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.TrimEnd:
        //                    string trimEndResult;
        //                    char[] tokenN0 = new char[currLength];
        //                    String tokenN1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    trimEndResult = tokenN1.TrimEnd(tokenN0);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;

        //                case StringTests.TrimStart:
        //                    string trimStartResult;
        //                    char[] tokenO0 = new char[currLength];
        //                    String tokenO1 = new String(GetTokenCharArray(currLength));
        //                    start = DateTime.UtcNow.Ticks;
        //                    trimStartResult = tokenO1.TrimStart(tokenO0);
        //                    stop = DateTime.UtcNow.Ticks;
        //                    break;
        //            }

        //            duration += stop - start;
        //        }

        //        double callTime = duration / (iterations * 10000);

        //        if (currLength < maxLength)
        //        {
        //            Debug.WriteLine(currLength + ":" + callTime + "ms,");
        //        }
        //        else
        //        {
        //            Debug.WriteLine(currLength + ":" + callTime + "ms");
        //        }

        //        if (currLength == maxLength)
        //        {
        //            break;
        //        }

        //        currLength *= 10;

        //        if (currLength > maxLength)
        //        {
        //            currLength = maxLength;
        //        }
        //    }
        //}

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
