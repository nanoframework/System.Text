//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.Diagnostics;

namespace System.Text
{
    /// <summary>
    /// Represents a mutable string of characters. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a port of the full .NET Framework <see cref="StringBuilder"/> class.
    /// </para>
    /// <para>
    /// Contributed to the .NETMF code base by Julius Friedman.
    /// </para>
    /// </remarks>
    public sealed class StringBuilder
    {
        private const int DefaultCapacity = 0x10;
        private const int DefaultMaxCapacity = short.MaxValue;
        private const int MaxChunkSize = 8000;

        #region Fields

        private readonly int _maxCapacity;
        private char[] _chunkChars;
        private int _chunkLength;
        private StringBuilder _chunkPrevious;
        private int _chunkOffset;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the maximum capacity of this instance.
        /// </summary>
        /// <value>The maximum number of characters this instance can hold.</value>
        public int MaxCapacity => _maxCapacity;

        /// <summary>
        /// Gets or sets the character at the specified character position in this instance.
        /// </summary>
        /// <param name="index">The position of the character.</param>
        /// <returns>The Unicode character at position index.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is outside the bounds of this instance while getting a character.</exception>"
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the bounds of this instance while setting a character.</exception>"
        public char this[int index]
        {
            get
            {
                StringBuilder chunk = this;

                while (true)
                {
                    int indexInBlock = index - chunk._chunkOffset;

                    if (indexInBlock >= 0)
                    {
                        if (indexInBlock >= chunk._chunkLength)
                        {
#pragma warning disable S112 // General exceptions should never be thrown
                            throw new IndexOutOfRangeException();
#pragma warning restore S112 // General exceptions should never be thrown
                        }

                        return chunk._chunkChars[indexInBlock];
                    }

                    chunk = chunk._chunkPrevious;

                    if (chunk == null)
                    {
#pragma warning disable S112 // General exceptions should never be thrown
                        throw new IndexOutOfRangeException();
#pragma warning restore S112 // General exceptions should never be thrown
                    }
                }
            }

            set
            {
                StringBuilder chunk = this;

                while (true)
                {
                    int indexInBlock = index - chunk._chunkOffset;

                    if (indexInBlock >= 0)
                    {
                        if (indexInBlock >= chunk._chunkLength)
                        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                            throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
                        }

                        chunk._chunkChars[indexInBlock] = value;
                        return;
                    }

                    chunk = chunk._chunkPrevious;

                    if (chunk == null)
                    {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                        throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be contained in the memory allocated by the current instance.
        /// </summary>
        /// <value>
        /// The maximum number of characters that can be contained in the memory allocated by the current instance. Its value can range from Length to MaxCapacity.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <para>The value specified for a set operation is less than the current length of this instance.</para>
        /// <para>-or-</para>
        /// <para>The value specified for a set operation is greater than the maximum capacity.</para>
        /// </exception>
        public int Capacity
        {
            get => _chunkChars.Length + _chunkOffset;

            set
            {
                if (value < 0 || value > MaxCapacity || value < Length)
                {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
                }

                if (Capacity != value)
                {
                    int num = value - _chunkOffset;
                    char[] destinationArray = new char[num];

                    Array.Copy(_chunkChars, destinationArray, _chunkLength);

                    _chunkChars = destinationArray;
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <value>The length of this instance.</value>
        /// <exception cref="ArgumentOutOfRangeException">If <see cref="Length"/> is set to a value that is less than zero or greater than <see cref="MaxCapacity"/>.</exception>
        public int Length
        {
            get => _chunkOffset + _chunkLength;

            set
            {
                if (value < 0 || value > MaxCapacity)
                {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                    throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
                }

                if (value == 0 && _chunkPrevious == null)
                {
                    _chunkLength = 0;
                    _chunkOffset = 0;

                    return;
                }

                int delta = value - Length;

                if (delta > 0)
                {
                    // Pad ourselves with null characters.
                    Append('\0', delta);
                }
                else
                {
                    StringBuilder chunk = FindChunkForIndex(value);

                    if (chunk != this)
                    {
                        // Avoid possible infinite capacity growth.  See https://github.com/dotnet/coreclr/pull/16926
                        int capacityToPreserve = MathInternal.Min(Capacity, MathInternal.Max(Length * 6 / 5, _chunkChars.Length));
                        int newLen = capacityToPreserve - chunk._chunkOffset;

                        if (newLen > chunk._chunkChars.Length)
                        {
                            // We crossed a chunk boundary when reducing the Length. We must replace this middle-chunk with a new larger chunk,
                            // to ensure the capacity we want is preserved.
                            char[] newArray = new char[newLen];
                            Array.Copy(chunk._chunkChars, newArray, chunk._chunkLength);
                            _chunkChars = newArray;
                        }
                        else
                        {
                            // Special case where the capacity we want to keep corresponds exactly to the size of the content.
                            // Just take ownership of the array.
                            _chunkChars = chunk._chunkChars;
                        }

                        _chunkPrevious = chunk._chunkPrevious;
                        _chunkOffset = chunk._chunkOffset;

                    }

                    _chunkLength = value - chunk._chunkOffset;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class from the specified substring and capacity. 
        /// </summary>
        /// <param name="value">The string that contains the substring used to initialize the value of this instance. If value is <see langword="null"/>, the new <see cref="StringBuilder"/> will contain the empty string (that is, it contains <see cref="string.Empty"/>).</param>
        /// <param name="startIndex">The position within value where the substring begins.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="capacity">The suggested starting size of the <see cref="StringBuilder"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="capacity"/> is less than zero, <paramref name="length"/> is less than zero, <paramref name="startIndex"/> is less than zero, or <paramref name="startIndex"/> is greater than the length of <paramref name="value"/> minus <paramref name="length"/>.</exception>
        public unsafe StringBuilder(
            string value,
            int startIndex,
            int length,
            int capacity)
        {
            if (capacity < 0 || length < 0 || startIndex < 0)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            value ??= string.Empty;

            if (startIndex > value.Length - length)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            _maxCapacity = DefaultMaxCapacity;

            if (capacity == 0)
            {
                capacity = DefaultCapacity;
            }

            if (capacity < length)
            {
                capacity = length;
            }

            // Allocate the chunk of capactity
            _chunkChars = new char[capacity];

            // Set the length of the chunk
            _chunkLength = length;

            // Copy the value to the chunkChars
            value.ToCharArray(startIndex, length).CopyTo(_chunkChars, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class using the specified string and capacity.
        /// </summary>
        /// <param name="value">The string used to initialize the value of the instance. If value is <see langword="null"/>, the new <see cref="StringBuilder"/> will contain the empty string (that is, it contains <see cref="string.Empty"/>).</param>
        /// <param name="capacity">The suggested starting size of the StringBuilder.</param>
        public StringBuilder(
            string value,
            int capacity)
            : this(value, 0, value
                  != null ? value.Length : 0, capacity)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class that starts with a specified capacity and can grow to a specified maximum. 
        /// </summary>
        /// <param name="capacity">The suggested starting size of the <see cref="StringBuilder"/>.</param>
        /// <param name="maxCapacity">The maximum number of characters the current string can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="capacity"/> is less than zero, <paramref name="maxCapacity"/> is less than one, or <paramref name="capacity"/> is greater than <paramref name="maxCapacity"/>.</exception>
        public StringBuilder(
            int capacity,
            int maxCapacity)
        {
            if (capacity > maxCapacity || maxCapacity < 1 || capacity < 0)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (capacity == 0)
            {
                capacity = MathInternal.Min(DefaultCapacity, maxCapacity);
            }

            _maxCapacity = maxCapacity;
            _chunkChars = new char[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class using the specified capacity. 
        /// </summary>
        /// <param name="capacity">The suggested starting size of this instance.</param>
        public StringBuilder(int capacity)
            : this(string.Empty, capacity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class using the specified string. 
        /// </summary>
        /// <param name="value">The string used to initialize the value of the instance. If value is <see langword="null"/>, the new <see cref="StringBuilder"/> will contain the empty string (that is, it contains <see cref="string.Empty"/>).</param>
        public StringBuilder(string value)
            : this(value, DefaultCapacity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilder"/> class. 
        /// </summary>
        public StringBuilder()
            : this(DefaultCapacity) { }

        private StringBuilder(
            int size,
            int maxCapacity,
            StringBuilder previousBlock)
        {
            _chunkChars = new char[size];
            _maxCapacity = maxCapacity;
            _chunkPrevious = previousBlock;

            if (previousBlock != null)
            {
                _chunkOffset = previousBlock._chunkOffset + previousBlock._chunkLength;
            }
        }

        private StringBuilder(StringBuilder from)
        {
            _chunkLength = from._chunkLength;
            _chunkOffset = from._chunkOffset;
            _chunkChars = from._chunkChars;
            _chunkPrevious = from._chunkPrevious;
            _maxCapacity = from._maxCapacity;
        }

        #endregion

        #region Append methods

        /// <summary>
        /// Appends the string representation of a specified <see cref="bool"/> value to this instance. 
        /// </summary>
        /// <param name="value">The Boolean value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(bool value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of a specified 8-bit unsigned integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(byte value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of a specified Unicode character to this instance. 
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(char value)
        {
            if (_chunkLength < _chunkChars.Length)
            {
                _chunkChars[_chunkLength++] = value;
            }
            else
            {
                Append(value, 1);
            }

            return this;
        }

        /// <summary>
        /// Appends the string representation of a specified double-precision floating-point number to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(double value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of a specified 16-bit signed integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(short value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of the Unicode characters in a specified array to this instance. 
        /// </summary>
        /// <param name="value">The array of characters to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        /// <remarks>
        /// This method appends all the characters in the specified array to the current instance in the same order as they appear in value. If value is <see langword="null"/>, no changes are made.
        /// </remarks>
        public StringBuilder Append(char[] value)
        {
            if (value != null && value.Length > 0)
            {
                Append(value, value.Length);
            }

            return this;
        }

        /// <summary>
        /// Appends the string representation of a specified 32-bit signed integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(int value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of a specified 64-bit unsigned integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(long value) => Append(value.ToString());

        /// <summary>
        /// Appends the string representation of a specified object to this instance. 
        /// </summary>
        /// <param name="value">The object to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(object value) => value == null ? this : Append(value.ToString());

        /// <summary>
        /// Appends a copy of the specified string to this instance.
        /// </summary>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                char[] chunkChars = _chunkChars;
                int chunkLength = _chunkLength;
                int length = value.Length;
                int num3 = chunkLength + length;

                if (num3 < chunkChars.Length)
                {
                    char[] tmp = value.ToCharArray();
                    Array.Copy(
                        tmp,
                        0,
                        chunkChars,
                        chunkLength,
                        length);

                    _chunkLength = num3;
                }
                else
                {
                    AppendHelper(ref value);
                }
            }

            return this;
        }

#pragma warning disable CS3001 // Argument type 'sbyte' is not CLS-compliant
        /// <summary>
        /// Appends the string representation of a specified 8-bit signed integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(sbyte value) => Append(value.ToString());
#pragma warning restore CS3001

        /// <summary>
        /// Appends the string representation of a specified double-precision floating-point number to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(float value) => Append(value.ToString());

#pragma warning disable CS3001 // Argument type 'ushort' is not CLS-compliant
        /// <summary>
        /// Appends the string representation of a specified 16-bit unsigned integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(ushort value) => Append(value.ToString());
#pragma warning restore CS3001

#pragma warning disable CS3001 // Argument type 'uint' is not CLS-compliant
        /// <summary>
        /// Appends the string representation of a specified 32-bit unsigned integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(uint value) => Append(value.ToString());
#pragma warning restore CS3001

#pragma warning disable CS3001 // Argument type 'ulong' is not CLS-compliant
        /// <summary>
        /// Appends the string representation of a specified 64-bit unsigned integer to this instance. 
        /// </summary>
        /// <param name="value">The value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(ulong value) => Append(value.ToString());
#pragma warning restore CS3001

        /// <summary>
        /// Appends a copy of a specified substring to this instance. 
        /// </summary>
        /// <param name="value">The string that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within value.</param>
        /// <param name="count">The number of characters in value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(
            string value,
            int startIndex,
            int count)
        {
            if (startIndex < 0
                || count < 0
                || value == null
                || startIndex > value.Length - count)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (count != 0)
            {
                Append(value.Substring(startIndex, count));
            }

            return this;
        }

        /// <summary>
        /// Appends the string representation of a specified subarray of Unicode characters to this instance
        /// </summary>
        /// <param name="value">A character array. </param>
        /// <param name="startIndex">The starting position in value. </param>
        /// <param name="charCount">The number of characters to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(
            char[] value,
            int startIndex,
            int charCount)
        {
            if (startIndex < 0
                || charCount < 0
                || value == null
                || startIndex > value.Length - charCount)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (charCount != 0)
            {
                for (int i = startIndex; i < startIndex + charCount; ++i)
                {
                    Append(value[i], 1);
                }
            }

            return this;
        }

        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character to this instance. 
        /// </summary>
        /// <param name="value">The character to append.</param>
        /// <param name="repeatCount">The number of times to append value.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder Append(
            char value,
            int repeatCount)
        {
            if (repeatCount < 0)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (repeatCount != 0)
            {
                int chunkLength = _chunkLength;

                while (repeatCount > 0)
                {
                    if (chunkLength < _chunkChars.Length)
                    {
                        _chunkChars[chunkLength++] = value;
                        repeatCount--;
                    }
                    else
                    {
                        _chunkLength = chunkLength;
                        ExpandByABlock(repeatCount);
                        chunkLength = 0;
                    }
                }
                _chunkLength = chunkLength;
            }

            return this;
        }

        #endregion

        #region Remove methods

        /// <summary>
        /// Removes the specified range of characters from this instance. 
        /// </summary>
        /// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
        /// <param name="length">The number of characters to remove.</param>
        /// <returns>A reference to this instance after the excise operation has completed.</returns>
        public StringBuilder Remove(
            int startIndex,
            int length)
        {
            if (length < 0
                || startIndex < 0
                || length > Length - startIndex)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (Length == length && startIndex == 0)
            {
                Length = 0;

                return this;
            }

            if (length > 0)
            {
                Remove(startIndex, length, out _, out _);
            }

            return this;
        }

        #endregion

        #region Insert methods

        /// <summary>
        /// Inserts one or more copies of a specified string into this instance at the specified character position. 
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">The string to insert.</param>
        /// <param name="count">The number of times to insert value.</param>
        /// <returns>A reference to this instance after insertion has completed.</returns>
        public StringBuilder Insert(
            int index,
            string value,
            int count)
        {
            if (count < 0
                || index > Length)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (!string.IsNullOrEmpty(value) && count != 0)
            {
                long num2 = value.Length * count;

                if (num2 > MaxCapacity - Length)
                {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                    throw new OutOfMemoryException();
#pragma warning restore S112 // OK to use in .NET nanoFramework context
                }

                MakeRoom(
                    index,
                    (int)num2,
                    out StringBuilder builder,
                    out int num3,
                    false);

                char[] chars = value.ToCharArray();
                int charLength = chars.Length;

                while (count > 0)
                {
                    int cindex = 0;

                    ReplaceInPlaceAtChunk(
                        ref builder,
                        ref num3,
                        chars,
                        ref cindex,
                        charLength);

                    --count;
                }
            }

            return this;
        }

        /// <summary>
        /// Inserts the string representation of a specified subarray of Unicode characters into this instance at the specified character position. 
        /// </summary>
        /// <param name="index">The position in this instance where insertion begins.</param>
        /// <param name="value">A character array.</param>
        /// <param name="startIndex">The starting index within value.</param>
        /// <param name="charCount">The number of characters to insert.</param>
        /// <returns>A reference to this instance after the insert operation has completed.</returns>
        public StringBuilder Insert(
            int index,
            char[] value,
            int startIndex,
            int charCount)
        {
            if (index > Length
                || value == null
                || startIndex < 0
                || charCount < 0
                || startIndex > value.Length - charCount)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (charCount > 0)
            {
                Insert(index, new string(value, startIndex, charCount), 1);
            }

            return this;
        }

        #endregion

        #region Replace methods

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified character with another specified character. 
        /// </summary>
        /// <param name="oldChar">The character to replace. </param>
        /// <param name="newChar">The character that replaces <paramref name="oldChar"/>. </param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring. </param>
        /// <returns>A reference to this instance with <paramref name="oldChar"/> replaced by <paramref name="newChar"/> in the range from <paramref name="startIndex"/> to <paramref name="startIndex"/> + <paramref name="count"/> - 1.</returns>
        public StringBuilder Replace(
            char oldChar,
            char newChar,
            int startIndex,
            int count)
        {
            if (startIndex > Length
                || count < 0
                || startIndex > Length - count)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            int num2 = startIndex + count;
            StringBuilder chunkPrevious = this;

            while (true)
            {
                int num3 = num2 - chunkPrevious._chunkOffset;
                int num4 = startIndex - chunkPrevious._chunkOffset;

                if (num3 >= 0)
                {
                    int index = MathInternal.Max(num4, 0);
                    int num6 = MathInternal.Min(chunkPrevious._chunkLength, num3);

                    while (index < num6)
                    {
                        if (chunkPrevious._chunkChars[index] == oldChar)
                        {
                            chunkPrevious._chunkChars[index] = newChar;
                        }

                        index++;
                    }
                }

                if (num4 < 0)
                {
                    chunkPrevious = chunkPrevious._chunkPrevious;
                }
                else
                {
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Replaces all occurrences of a specified character in this instance with another specified character. 
        /// </summary>
        /// <param name="oldChar">The character to replace.</param>
        /// <param name="newChar">The character that replaces <paramref name="oldChar"/>.</param>
        /// <returns>A reference to this instance with <paramref name="oldChar"/> replaced by <paramref name="newChar"/>.</returns>
        public StringBuilder Replace(char oldChar, char newChar) => Replace(oldChar, newChar, 0, Length);

        /// <summary>
        /// Replaces, within a substring of this instance, all occurrences of a specified string with another specified string. 
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces <paramref name="oldValue"/>, or <see langword="null"/>.</param>
        /// <param name="startIndex">The position in this instance where the substring begins.</param>
        /// <param name="count">The length of the substring.</param>
        /// <returns>A reference to this instance with all instances of <paramref name="oldValue"/> replaced by <paramref name="newValue"/> in the range from <paramref name="startIndex"/> to <paramref name="startIndex"/> + <paramref name="count"/> - 1.</returns>
        public StringBuilder Replace(
            string oldValue,
            string newValue,
            int startIndex,
            int count)
        {
            if (startIndex > Length
                || count < 0
                || startIndex > Length - count
                || oldValue == null
                || oldValue.Length == 0)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            newValue ??= string.Empty;

            int newLength = newValue.Length;
            int oldLength = oldValue.Length;
            int[] sourceArray = null;
            int replacementsCount = 0;
            StringBuilder chunk = FindChunkForIndex(startIndex);
            int indexInChunk = startIndex - chunk._chunkOffset;

            while (count > 0)
            {
                if (StartsWith(chunk, indexInChunk, count, oldValue))
                {
                    if (sourceArray == null)
                    {
                        sourceArray = new int[5];
                    }
                    else if (replacementsCount >= sourceArray.Length)
                    {
                        int[] destinationArray = new int[sourceArray.Length * 3 / 2 + 4];

                        Array.Copy(
                            sourceArray,
                            destinationArray,
                            sourceArray.Length);

                        sourceArray = destinationArray;
                    }

                    sourceArray[replacementsCount] = indexInChunk;
                    ++replacementsCount;
                    indexInChunk += oldLength;
                    count -= oldLength;
                }
                else
                {
                    ++indexInChunk;
                    --count;
                }

                if (indexInChunk >= chunk._chunkLength || count == 0)
                {
                    int index = indexInChunk + chunk._chunkOffset;

                    ReplaceAllInChunk(sourceArray, replacementsCount, chunk, oldLength, newValue);

                    index += (newLength - oldLength) * replacementsCount;
                    replacementsCount = 0;

                    chunk = FindChunkForIndex(index);

                    indexInChunk = index - chunk._chunkOffset;
                }
            }

            return this;
        }

        /// <summary>
        /// Replaces all occurrences of a specified string in this instance with another specified string. 
        /// </summary>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The string that replaces <paramref name="oldValue"/>, or <see langword="null"/>.</param>
        /// <returns>A reference to this instance with all instances of <paramref name="oldValue"/> replaced by <paramref name="newValue"/>.</returns>
        public StringBuilder Replace(
            string oldValue,
            string newValue) => Replace(
                oldValue,
                newValue,
                0,
                Length);

        #endregion

        #region Other methods

        /// <summary>
        /// Removes all characters from the current <see cref="StringBuilder"/> instance. 
        /// </summary>
        /// <returns>An object whose <see cref="Length"/> is 0 (zero).</returns>
        public StringBuilder Clear()
        {
            Length = 0;
            return this;
        }

        /// <summary>
        /// Converts the value of this instance to a String. (Overrides <see cref="Object.ToString"/>().)
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            char[] result = new char[Length];
            StringBuilder chunkPrevious = this;

            do
            {
                if (chunkPrevious._chunkLength > 0)
                {
                    char[] chunkChars = chunkPrevious._chunkChars;
                    int chunkOffset = chunkPrevious._chunkOffset;
                    int chunkLength = chunkPrevious._chunkLength;

                    Array.Copy(
                        chunkChars,
                        0,
                        result,
                        chunkOffset,
                        chunkLength);
                }

                chunkPrevious = chunkPrevious._chunkPrevious;
            }
            while (chunkPrevious != null);

            return new string(result);
        }

        /// <summary>
        /// Converts the value of a substring of this instance to a <see cref="string"/>. 
        /// </summary>
        /// <param name="startIndex">The starting position of the substring in this instance.</param>
        /// <param name="length">The length of the substring.</param>
        /// <returns>A string whose value is the same as the specified substring of this instance.</returns>
        public string ToString(
            int startIndex,
            int length)
        {
            int currentLength = Length;

            if (startIndex < 0
                || startIndex > currentLength
                || length < 0
                || startIndex > currentLength - length)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            StringBuilder chunk = this;
            int sourceEndIndex = startIndex + length;
            char[] result = new char[length];
            int destinationIndex = length;

            while (destinationIndex > 0)
            {
                int chunkLength = sourceEndIndex - chunk._chunkOffset;

                if (chunkLength >= 0)
                {
                    if (chunkLength > chunk._chunkLength)
                    {
                        chunkLength = chunk._chunkLength;
                    }

                    int leftChars = destinationIndex;
                    int charCount = leftChars;
                    int index = chunkLength - leftChars;

                    if (index < 0)
                    {
                        charCount += index;
                        index = 0;
                    }

                    destinationIndex -= charCount;

                    if (charCount > 0)
                    {
                        char[] chunkChars = chunk._chunkChars;

                        if (charCount + destinationIndex > length
                            || charCount + index > chunkChars.Length)
                        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                            throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
                        }

                        Array.Copy(
                            chunkChars,
                            index,
                            result,
                            destinationIndex,
                            charCount);
                    }
                }

                chunk = chunk._chunkPrevious;
            }

            return new string(result);
        }

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <param name="str">A reference to this instance after the append operation has completed.</param>
        public StringBuilder AppendLine(string str)
        {
            Append(str);
            return AppendLine();
        }

        /// <summary>
        /// Appends the default line terminator to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder AppendLine() => Append("\r\n");

        #endregion

        #region Internals

        internal int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentOutOfRangeException();
#pragma warning restore S3928 // OK to use in .NET nanoFramework context
            }

            if (Capacity < capacity)
            {
                Capacity = capacity;
            }

            return Capacity;
        }

        internal bool StartsWith(StringBuilder chunk, int indexInChunk, int count, string value)
        {
            for (int i = 0, e = value.Length; i < e; ++i)
            {
                if (count == 0)
                {
                    return false;
                }

                if (indexInChunk >= chunk._chunkLength)
                {
                    chunk = Next(chunk);

                    if (chunk == null)
                    {
                        return false;
                    }

                    indexInChunk = 0;
                }

                if (value.GetCharByIndex(i) != chunk._chunkChars[indexInChunk])
                {
                    return false;
                }

                ++indexInChunk;
                --count;
            }

            return true;
        }

        internal void ReplaceAllInChunk(
            int[] replacements,
            int replacementsCount,
            StringBuilder sourceChunk,
            int removeCount,
            string value)
        {
            //If there is a replacement to occur
            if (replacementsCount > 0)
            {
                // Determine the amount of characters to remove
                int count = (value.Length - removeCount) * replacementsCount;

                // Scope the working chunk
                StringBuilder chunk = sourceChunk;

                // Determine the index of the first replacement
                int indexInChunk = replacements[0];

                // If there is a character being added make room
                if (count > 0)
                {
                    MakeRoom(
                        chunk._chunkOffset + indexInChunk,
                        count,
                        out chunk,
                        out indexInChunk,
                        true);
                }

                // Start at the first replacement
                int index = 0;
                int replacementIndex = 0;
                char[] chars = value.ToCharArray();

            ReplaceValue:
                // Replace the value                 
                ReplaceInPlaceAtChunk(
                    ref chunk,
                    ref indexInChunk,
                    chars,
                    ref replacementIndex,
                    value.Length);

                if (replacementIndex == value.Length)
                {
                    replacementIndex = 0;
                }

                // Determine the next replacement 
                int valueIndex = replacements[index] + removeCount;

                // Move the pointer of the working replacement
                ++index;

                // If we are not past the replacement boundary
                if (index < replacementsCount)
                {
                    // Determine the next replacement
                    int nextIndex = replacements[index];

                    // If there is a character remaining to be replaced
                    if (count != 0)
                    {
                        // Replace it
                        ReplaceInPlaceAtChunk(
                            ref chunk,
                            ref indexInChunk,
                            sourceChunk._chunkChars,
                            ref valueIndex,
                            nextIndex - valueIndex);
                    }
                    //Move the pointer
                    else
                    {
                        indexInChunk += nextIndex - valueIndex;
                    }

                    // Finish replacing
                    goto ReplaceValue;
                }

                // We are are done and there is charcters to be removed they are at the end
                if (count < 0)
                {
                    // Remove them by negating the count to make it positive the chars removed are from (chunk.m_ChunkOffset + indexInChunk) to -count
                    Remove(
                        chunk._chunkOffset + indexInChunk,
                        -count,
                        out chunk,
                        out indexInChunk);
                }
            }
        }

        internal StringBuilder Next(StringBuilder chunk)
        {
            return chunk == this ? null : FindChunkForIndex(chunk._chunkOffset
                + chunk._chunkLength);
        }

        private void ReplaceInPlaceAtChunk(
            ref StringBuilder chunk,
            ref int indexInChunk,
            char[] value,
            ref int valueIndex,
            int count)
        {
            if (count == 0)
            {
                return;
            }

            while (true)
            {
                int length = MathInternal.Min(chunk._chunkLength - indexInChunk, count);

                Array.Copy(
                    value,
                    valueIndex,
                    chunk._chunkChars,
                    indexInChunk,
                    length);

                indexInChunk += length;

                if (indexInChunk >= chunk._chunkLength)
                {
                    chunk = Next(chunk);
                    indexInChunk = 0;
                }

                count -= length;
                valueIndex += length;

                if (count == 0)
                {
                    return;
                }
            }
        }

        internal void MakeRoom(
            int index,
            int count,
            out StringBuilder chunk,
            out int indexInChunk,
            bool doneMoveFollowingChars)
        {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            if (count + Length > _maxCapacity)
            {
                throw new ArgumentOutOfRangeException();
            }
#pragma warning restore S3928 // OK to use in .NET nanoFramework context

            chunk = this;

            while (chunk._chunkOffset > index)
            {
                chunk._chunkOffset += count;
                chunk = chunk._chunkPrevious;
            }

            indexInChunk = index - chunk._chunkOffset;

            if (!doneMoveFollowingChars
                && chunk._chunkLength <= 0x20
                && chunk._chunkChars.Length - chunk._chunkLength >= count)
            {
                int chunkLength = chunk._chunkLength;

                while (chunkLength > indexInChunk)
                {
                    chunkLength--;
                    chunk._chunkChars[chunkLength + count] = chunk._chunkChars[chunkLength];
                }

                chunk._chunkLength += count;
            }
            else
            {
                StringBuilder builder = new(
                    MathInternal.Max(count, DefaultCapacity),
                    chunk._maxCapacity,
                    chunk._chunkPrevious);

                builder._chunkLength = count;

                int length = MathInternal.Min(count, indexInChunk);

                if (length > 0)
                {
                    Array.Copy(
                        chunk._chunkChars,
                        0,
                        builder._chunkChars,
                        0,
                        length);

                    int nextLength = indexInChunk - length;

                    if (nextLength >= 0)
                    {
                        Array.Copy(
                            chunk._chunkChars,
                            length,
                            chunk._chunkChars,
                            0,
                            nextLength);

                        indexInChunk = nextLength;
                    }
                }

                chunk._chunkPrevious = builder;
                chunk._chunkOffset += count;

                if (length < count)
                {
                    chunk = builder;
                    indexInChunk = length;
                }
            }
        }

        internal StringBuilder FindChunkForIndex(int index)
        {
            StringBuilder chunkPrevious = this;

            while (chunkPrevious._chunkOffset > index)
            {
                chunkPrevious = chunkPrevious._chunkPrevious;
            }

            return chunkPrevious;
        }

        internal void AppendHelper(ref string value)
        {
            if (value == null || value == string.Empty)
            {
                return;
            }

            Append(value.ToCharArray(), value.Length);
        }

        internal void ExpandByABlock(int minBlockCharCount)
        {
            Debug.Assert(Capacity == Length, nameof(ExpandByABlock) + " should only be called when there is no space left.");
            Debug.Assert(minBlockCharCount > 0);

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            if (minBlockCharCount + Length > _maxCapacity || minBlockCharCount + Length < minBlockCharCount)
            {
                throw new ArgumentOutOfRangeException();
            }
#pragma warning restore S3928 // OK to use in .NET nanoFramework context

            // - We always need to make the new chunk at least as big as was requested (`minBlockCharCount`).
            // - We'd also prefer to make it at least at big as the current length (thus doubling capacity).
            //   - But this is only up to a maximum, so we stay in the small object heap, and never allocate
            //     really big chunks even if the string gets really big.
            int newBlockLength = MathInternal.Max(
                minBlockCharCount,
                MathInternal.Min(Length, MaxChunkSize));


            // Allocate the array before updating any state to avoid leaving inconsistent state behind in case of out of memory exception
            char[] chunkChars = new char[newBlockLength];

            // Move all of the data from this chunk to a new one, via a few O(1) reference adjustments.
            // Then, have this chunk point to the new one as its predecessor.
            _chunkPrevious = new StringBuilder(this);
            _chunkOffset += _chunkLength;
            _chunkLength = 0;

            _chunkChars = chunkChars;
        }

        internal void Remove(int startIndex, int count, out StringBuilder chunk, out int indexInChunk)
        {
            int num = startIndex + count;
            chunk = this;

            StringBuilder builder = null;
            int sourceIndex = 0;

            while (true)
            {
                if (num - chunk._chunkOffset >= 0)
                {
                    if (builder == null)
                    {
                        builder = chunk;
                        sourceIndex = num - builder._chunkOffset;
                    }

                    if (startIndex - chunk._chunkOffset >= 0)
                    {
                        indexInChunk = startIndex - chunk._chunkOffset;
                        int destinationIndex = indexInChunk;
                        int num4 = builder._chunkLength - sourceIndex;

                        if (builder != chunk)
                        {
                            destinationIndex = 0;
                            chunk._chunkLength = indexInChunk;
                            builder._chunkPrevious = chunk;
                            builder._chunkOffset = chunk._chunkOffset + chunk._chunkLength;

                            if (indexInChunk == 0)
                            {
                                builder._chunkPrevious = chunk._chunkPrevious;
                                chunk = builder;
                            }
                        }

                        builder._chunkLength -= sourceIndex - destinationIndex;

                        if (destinationIndex != sourceIndex)
                        {
                            Array.Copy(
                                builder._chunkChars,
                                sourceIndex,
                                builder._chunkChars,
                                destinationIndex,
                                num4);
                        }
                        return;
                    }
                }
                else
                {
                    chunk._chunkOffset -= count;
                }

                chunk = chunk._chunkPrevious;
            }
        }

        internal void Append(
            char[] value,
            int valueCount)
        {
            if (value == null)
            {
                return;
            }

            int num = valueCount + _chunkLength;

            if (num <= _chunkChars.Length)
            {
                Array.Copy(
                    value,
                    0,
                    _chunkChars,
                    _chunkLength,
                    valueCount);

                _chunkLength = num;
            }
            else
            {
                int count = _chunkChars.Length - _chunkLength;

                if (count > 0)
                {
                    Array.Copy(
                        value,
                        0,
                        _chunkChars,
                        _chunkLength,
                        count);

                    _chunkLength = _chunkChars.Length;
                }

                int minBlockCharCount = valueCount - count;

                ExpandByABlock(minBlockCharCount);

                Array.Copy(
                    value,
                    count,
                    _chunkChars,
                    0,
                    minBlockCharCount);

                _chunkLength = minBlockCharCount;
            }
        }

        #endregion        
    }
}
