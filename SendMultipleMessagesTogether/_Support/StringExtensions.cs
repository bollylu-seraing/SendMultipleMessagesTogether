using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMultipleMessagesTogether.Support {
  public static class StringExtensions {

    #region --- Left --------------------------------------------
    /// <summary>
    /// Gets the left portion of a string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="length">The number of characters to get</param>
    /// <returns>The selected portion of the string. If Length > Length of the string, returns the string.</returns>
    public static string Left(this string sourceString, int length) {
      #region Validate parameters
      if (length < 0) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString.Length >= length) {
        return sourceString.Substring(0, length);
      }

      return sourceString;
    }
    #endregion --- Left --------------------------------------------

    #region --- Right --------------------------------------------
    /// <summary>
    /// Gets the right portion of the string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="length">The number of characters to get</param>
    /// <returns>The selected portion of the string. If Length > Length of the string, returns the string.</returns>
    public static string Right(this string sourceString, int length) {
      #region Validate parameters
      if (length < 0) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString.Length >= length) {
        return sourceString.Substring(sourceString.Length - length);
      }

      return sourceString;
    }
    #endregion --- Right --------------------------------------------

    #region --- After --------------------------------------------
    /// <summary>
    /// Gets the portion of the string after a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string after the delimiter</returns>
    public static string After(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return string.Empty;
      }
      int Index = sourceString.IndexOf(delimiter, 0, stringComparison);
      if (Index == -1) {
        return string.Empty;
      }

      return sourceString.Substring(Index + delimiter.Length);
    }
    #endregion --- After --------------------------------------------

    #region --- AfterLast --------------------------------------------
    /// <summary>
    /// Gets the portion of the string after the last occurence of a given string
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string after the last occurence of a delimiter</returns>
    public static string AfterLast(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      //if (sourceString == null) {
      //  return null;
      //}
      if (string.IsNullOrEmpty(delimiter)) {
        return sourceString;
      }
      #endregion Validate parameters

      if (sourceString == delimiter) {
        return string.Empty;
      }
      int Index = sourceString.LastIndexOf(delimiter, stringComparison);
      if (Index == -1) {
        return string.Empty;
      }

      return sourceString.Substring(Index + delimiter.Length);
    }
    #endregion --- AfterLast --------------------------------------------

    #region --- Before --------------------------------------------
    /// <summary>
    /// Gets the portion of the string before a given string
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string before the delimiter</returns>
    public static string Before(this string source, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      //if (sourceString == null) {
      //  return null;
      //}
      if (string.IsNullOrEmpty(delimiter)) {
        return source;
      }
      #endregion Validate parameters

      if (source == delimiter) {
        return "";
      }
      int Index = source.IndexOf(delimiter, stringComparison);
      //if ( Index == -1 ) {
      //  return sourceString;
      //}
      if (Index < 1) {
        return "";
      }

      return source.Left(Index);
    }
    #endregion --- Before --------------------------------------------

    #region --- BeforeLast --------------------------------------------
    /// <summary>
    /// Gets the portion of the string before the last occurence of a given string
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="delimiter">The string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string before the last occurence of the delimiter</returns>
    public static string BeforeLast(this string source, string delimiter, StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      //if (sourceString == null) {
      //  return null;
      //}
      if (string.IsNullOrEmpty(delimiter)) {
        return source;
      }
      #endregion Validate parameters

      if (source == delimiter) {
        return "";
      }
      int Index = source.LastIndexOf(delimiter, stringComparison);

      if (Index < 1) {
        return "";
      }

      return source.Left(Index);
    }
    #endregion --- BeforeLast --------------------------------------------

    #region --- Except --------------------------------------------
    /// <summary>
    /// Get the whole string but the part to remove
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="dataToRemove">The string to remove</param>
    /// <returns>The cleaned string</returns>
    public static string Except(this string source, string dataToRemove) {
      #region Validate parameters
      if (string.IsNullOrEmpty(dataToRemove)) {
        return source;
      }
      #endregion Validate parameters

      int Index = source.IndexOf(dataToRemove);
      if (Index == 0) {
        return source;
      }

      return source.Replace(dataToRemove, "");
    }
    #endregion --- Except --------------------------------------------

    #region --- Between --------------------------------------------
    /// <summary>
    /// Gets the portion of the string after a given string
    /// </summary>
    /// <param name="source">The source string</param>
    /// <param name="firstDelimiter">The first string to search for</param>
    /// <param name="secondDelimiter">The second string to search for</param>
    /// <param name="stringComparison">The culture to find delimiter (useful for ignoring case)</param>
    /// <returns>The selected portion of the string between the delimiters</returns>
    public static string Between(this string source, string firstDelimiter = "[", string secondDelimiter = "]", StringComparison stringComparison = StringComparison.CurrentCulture) {
      return source.After(firstDelimiter, stringComparison).Before(secondDelimiter, stringComparison);
    }
    #endregion --- Between --------------------------------------------

    /// <summary>
    /// Gets the strings between two given strings
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="firstDelimiter">The first string to search for</param>
    /// <param name="secondDelimiter">The second string to search for</param>
    /// <param name="stringComparison">How to compare</param>
    /// <returns>A list of items found between both delimiters</returns>
    public static IEnumerable<string> ItemsBetween(this string sourceString, string firstDelimiter = "[", string secondDelimiter = "]", StringComparison stringComparison = StringComparison.CurrentCulture) {
      #region Validate parameters
      if (string.IsNullOrEmpty(sourceString)) {
        yield break;
      }
      if (firstDelimiter == "") {
        yield break;
      }
      if (secondDelimiter == "") {
        yield break;
      }
      #endregion Validate parameters

      string ProcessedString = sourceString;

      while (ProcessedString != "" && ProcessedString.IndexOf(firstDelimiter, stringComparison) != -1 && ProcessedString.IndexOf(secondDelimiter, stringComparison) != -1) {
        yield return ProcessedString.After(firstDelimiter, stringComparison).Before(secondDelimiter, stringComparison);
        ProcessedString = ProcessedString.After(secondDelimiter, stringComparison);
      }

      yield break;

    }

    /// <summary>
    /// Get a list of strings from one big string splitted
    /// </summary>
    /// <param name="sourceString">The source string</param>
    /// <param name="delimiter">The delimmiter</param>
    /// <param name="stringSplitOptions">The options to split</param>
    /// <returns>A list of strings</returns>
    public static IEnumerable<string> GetItems(this string sourceString, string delimiter = ";", StringSplitOptions stringSplitOptions = StringSplitOptions.None) {
      #region === Validate parameters ===
      if (string.IsNullOrEmpty(sourceString)) {
        yield break;
      }
      if (delimiter == "") {
        yield return sourceString;
      }
      #endregion === Validate parameters ===
      foreach (string SplitItem in sourceString.Split(new string[] { delimiter }, stringSplitOptions)) {
        yield return SplitItem;
      }
    }


    /// <summary>
    /// Get a new string surrounded by double quotes
    /// </summary>
    /// <param name="source">The source string</param>
    /// <returns>a new string surrounded by double quotes, or null if the source is null</returns>
    public static string WithQuotes(this string source) {
      return $"\"{source}\"";
    }

    /// <summary>
    /// Removes external quotes from a string (ex. "\"MyString\"" => "MyString")
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>The string without inner quotes</returns>
    public static string RemoveExternalQuotes(this string sourceValue) {
      if (string.IsNullOrWhiteSpace(sourceValue)) {
        return "";
      }

      if (!sourceValue.Contains('"')) {
        return sourceValue;
      }

      StringBuilder RetVal = new StringBuilder(sourceValue);

      if (sourceValue.StartsWith("\"")) {
        RetVal.Remove(0, 1);
      }

      if (sourceValue.EndsWith("\"")) {
        RetVal.Truncate(1);
      }

      return RetVal.ToString();
    }

    /// <summary>
    /// Removes n characters from the end of the StringBuilder
    /// </summary>
    /// <param name="source">The string builder</param>
    /// <param name="length">The amount of character(s) to remove</param>
    /// <returns></returns>
    public static StringBuilder Truncate(this StringBuilder source, int length) {
      if (length <= 0) {
        return source;
      }
      if (length >= source.Length) {
        return source.Clear();
      }

      return source.Remove(source.Length - length, length);
    }

    public static bool ToBool(this string booleanString) {
      #region Validate parameters
      if (booleanString == null) {
        return false;
      }
      #endregion Validate parameters
      switch (booleanString.Trim().ToLower()) {
        case "0":
        case "false":
        case "no":
        case "n":
          return false;
        case "1":
        case "true":
        case "yes":
        case "y":
          return true;
        default:
          return false;
      }
    }
  }

}
