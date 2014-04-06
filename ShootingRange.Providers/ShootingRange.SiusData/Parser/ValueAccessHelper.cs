using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ShootingRange.SiusData.Parser
{
  static class ValueAccessHelper
  {
    public static string GetString(IEnumerable<string> valueList, int index, SuccessHelper success)
    {
      return GetValue<string>(valueList, index, success);
    }

    public static int GetInt(IEnumerable<string> valueList, int index, SuccessHelper success)
    {
      return GetValue<int>(valueList, index, success);
    }

    public static float GetFloat(IEnumerable<string> valueList, int index, SuccessHelper success)
    {
      return GetValue<float>(valueList, index, success);
    }

    public static double GetDouble(IEnumerable<string> valueList, int index, SuccessHelper success)
    {
      return GetValue<double>(valueList, index, success);
    }

    public static DateTime GetDateTime(IEnumerable<string> valueList, int index, SuccessHelper success)
    {
      return GetValue<DateTime>(valueList, index, success);
    }

    /// <summary>Get the value of a column identifier from a valuelist. </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="valueList">The valuelist.</param>
    /// <param name="index">The index</param>
    /// <param name="successHelper"><c>true</c> if the returned value is valid.</param>
    /// <returns>The value.</returns>
    private static T GetValue<T>(IEnumerable<string> valueList, int index, SuccessHelper successHelper)
    {
      string[] myValueList = valueList.ToArray();
      T value = default(T);
      bool success = false;

      if (myValueList.Length > index)
      {
        string stringValue = myValueList[index];
        TypeConverter tc = TypeDescriptor.GetConverter(value);

        value = (T) tc.ConvertFromString(stringValue);
        success = true;
      }

      successHelper.Success &= success;
      return value;
    }
  }
}