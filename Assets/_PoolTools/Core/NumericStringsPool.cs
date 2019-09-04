using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PoolTools
{
    public class NumericStringsPool
    {
        public int MinInt;
        public int MaxInt;

        private string[] _numericStrings;
        private bool _isInt;
        private bool _isFloat;
        private int _floatDecimalsCount;

        public void CreateIntStringsPool(int min, int max, string formatString)
        {
            _isInt = true;
            MinInt = min;
            MaxInt = max;

            int stringsAmount = max - min + 1;
			
			_numericStrings = new string[stringsAmount];
			
            for (int i = 0; i < stringsAmount; i++)
            {
                _numericStrings[i] = (min + i).ToString(formatString);
            }
        }

        public void CreateFloatStringsPool(int min, int max, int decimalsCount, string formatString)
        {
            _isFloat = true;
            MinInt = min;
            MaxInt = max;
            _floatDecimalsCount = decimalsCount;

            int stringsPerInt = (int)Math.Pow(10, _floatDecimalsCount);
            int stringsAmount = ((max - min) * stringsPerInt) + 1;
			
			_numericStrings = new string[stringsAmount];
			
            float increment = 1f / stringsPerInt;
            for (int i = 0; i < stringsAmount; i++)
            {
                _numericStrings[i] = (min + (i * increment)).ToString(formatString);
            }
        }

        public string GetFormatStringForDecimalsCount(int decimalsCount)
        {
            string formatString = "{0:0.";
            for (int i = 0; i < decimalsCount; i++)
            {
                formatString += "0";
            }
            formatString += "}";

            return formatString;
        }

        public void DestroyPool()
        {
            _numericStrings = null;
        }

        public bool GetIntString(int number, out string numString)
        {
            if (_isInt && number >= MinInt && number <= MaxInt)
            {
                int calcIndex = (number - MinInt);
                numString = _numericStrings[calcIndex];
                return true;
            }

            numString = null;
            return false;
        }

        public bool GetFloatString(float number, out string numString)
        {
            if (_isFloat && number >= MinInt && number <= MaxInt)
            {
                int stringsPerInt = (int)Math.Pow(10, _floatDecimalsCount);
                float roundedFloat = Mathf.RoundToInt(number * stringsPerInt) / stringsPerInt;
                int calcIndex = (int)((roundedFloat - MinInt) * stringsPerInt);
                numString = _numericStrings[calcIndex];
                return true;
            }

            numString = null;
            return false;
        }

        public int GetStringsCount()
        {
            return _numericStrings.Length;
        }
    }
}