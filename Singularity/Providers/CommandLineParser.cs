using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;
using System.Text.RegularExpressions;
using Singularity.Resources;

namespace Singularity.Parsers
{
	/// <summary>
	/// <para>Class CLAParser provides everything for easy and fast handling of command line arguments.</para>
	/// <para>Required and optional parameters can be defined, as well as the type of each parameter (e.g. bool, int, string).</para>
	/// <para>If command line is correct, all arguments and their values can be accessed via a Dictionary interface.</para>
	/// <para>In case of an error, an exception is raised which explains the error.</para>
	/// <para>All output text can be nationalized by means of a ResourceManager.</para>
	/// </summary>
	public class CommandLineParser : IEnumerable, IEnumerator
	{
		/// <summary>
		/// <para>Class CLAParser provides everything for easy and fast handling of command line arguments.</para>
		/// <para>Usage:</para>
		/// <para>1) Create an instance of CLAParser by calling this constructor.</para>
		/// <para>2) Define parameters by calling Parameter() as often as needed.</para>
		/// <para>3) Optionally: Set variables such as AllowAdditionalParameters, AllowValuesWithoutParameter, and ParameterPrefix.</para>
		/// <para>4) Call Parse(), catch all CmdLineArgumentExceptions, and show those to user.</para>
		/// <para>5) Call GetUsage() and GetParameterInfo() to create information about using command line arguments.</para>
		/// </summary>aamespace of main code file Program.cs)<para>[This is necessary so that CLAParser can find its resource files (CmdLineArgumentParserRes.resx, CmdLineArgumentParserRes.de-DE.resx, ...)]</para></param>
		public CommandLineParser()
		{
			_foundParameters = new StringDictionary();
			_wantedParameters = new SortedDictionary<String, ParameterDefinition>();
			_enumerator = _foundParameters.GetEnumerator();

			ParameterPrefix = "/";
			AllowAdditionalParameters = false;
			AllowValuesWithoutParameter = false;
		}


		/// <summary>
		/// On parsing all parameters of command line are stored here
		/// if they match the requirements defined in WanntedParameters.
		/// </summary>
		private StringDictionary _foundParameters;
		private IEnumerator _enumerator;

		/// <summary>
		/// Stores all available information about a parameter defined by the programmer.
		/// </summary>
		private class ParameterDefinition
		{
			public ParameterDefinition(String parameterName, ParamAllowType allowType, ECommandArgumentValueTypes valueType, String parameterHelp)
			{
				this.Parameter = parameterName;
				this.AllowType = allowType;
				this.ValueType = valueType;
				this.Help = parameterHelp;
			}
			public String Parameter { get; set; }
			public ParamAllowType AllowType { get; set; }
			public ECommandArgumentValueTypes ValueType { get; set; }
			public String Help { get; set; }
		}
		/// <summary>
		/// Collection of all optional and required parameters. This is the base for validation.
		/// </summary>
		private SortedDictionary<String, ParameterDefinition> _wantedParameters;

		/// <summary>
		/// Specifies wether the parameter has to be supplied or wether it is optional.
		/// </summary>
		public enum ParamAllowType { Optional, Required };
		/// <summary>
		/// <para>Values without parameters are those words directly following the executable.</para>
		/// <para>Example: program.exe values without params /first_parameter first_value.</para>
		/// <para>By default this variable is false, thus letting the previous example raise an exception.</para>
		/// </summary>
		public Boolean AllowValuesWithoutParameter { get; set; }
		/// <summary>
		/// <para>Additional paramters are those not defined by the programmers using the Parameter() function.</para>
		/// <para>By default this variable is false, thus causing an exception if the user specifies an undefined paramter,</para>
		/// <para>e.g. program.exe /undefined_parameter value_5</para>
		/// </summary>
		public Boolean AllowAdditionalParameters { get; set; }
		/// <summary>
		/// <para>ParameterPrefix specifies the prefix for all parameters. It is just used by</para>
		/// <para>GetUsage() and GetParameterInfo(), not the Parse(). [Parse always accepts / and -]</para>
		/// <para>By default this variable is set to /</para>
		/// </summary>
		public String ParameterPrefix { get; set; }
		/// <summary>
		/// <para>Retrieve a parameter value if it exists.</para>
		/// <para>Remember: program.exe /parameter value.</para>
		/// <para>Note: Function Parse() must be called before!</para>
		/// </summary>
		/// <param name="param">Specifies the parameter</param>
		/// <returns>The corresponding value, or null if the is no such parameter,
		/// or if Parse() was not called yet.</returns> 
		public String this[String param]
		{
			get
			{
				if (_foundParameters == null) return null;
				else return (_foundParameters[param]);
			}
		}

		/// <summary>
		/// <para>Defines parameters which program understands.</para>
		/// <para>Parameter() can be called as often as required.</para>
		/// <para>Information passed to CLAParser by Parameter() is later used by Parse(), GetUsage(), GetParamaterInfo()</para>
		/// </summary>
		/// <param name="allowType">Choose parameter to be either as optional or required.</param>
		/// <param name="parameterName">Name of the parameter (everything behind / )</param>
		/// <param name="valueType">Defines valid values for the parameter.</param>
		/// <param name="parameterHelp">Information about the parameter. This string will later be used by GetParameterInfo().</param>
		public void Parameter(ParamAllowType allowType, String parameterName, ECommandArgumentValueTypes valueType, String parameterHelp)
		{
			ParameterDefinition param = new ParameterDefinition(parameterName, allowType, valueType, parameterHelp);
			_wantedParameters.Add(param.Parameter, param);
		}

		/// <summary>
		/// <para>Starts the parsing process. Throws CmdLineArgumentExceptions in case of errors.</para>
		/// <para>Afterwards use the enumerator or the dictionary interface to access the found paramters and their values.</para>
		/// </summary>
		/// <param name="argumentLine">Argument line passed via command line to the program.</param>        
		public void Parse(String argumentLine)
		{
			//NOTE: IF PARSING DOES NOT WORK AS EXPECTED, TRY TO ESCAPE QUOTES (ie. \" )
			//(from cmd.exe this seems to be necessary; instead single quotes could be used.)

			//pure: ^(((?<unknownvalues>[^/-]*)[\s]*)?([/-](?<name>[^\s-/:=]+)([:=]?)([\s]*)(?<value>(".*")|('.*')|([\s]*[^/-][^\s]+[\s]*)|([^/-]+)|)?([\s]*))*)$                                               
			String correctCmdLineRegEx = "^(((?<unknownvalues>[^/-]*)[\\s]*)?([/-](?<name>[^\\s-/:=]+)([:=]?)([\\s]*)(?<value>(\".*\")|('.*')|([\\s]*[^/-][^\\s]+[\\s]*)|([^/-]+)|)?([\\s]*))*)$";
			//start from beginning (^) and go to very end ($)
			//find anything except \ or - ([^\/-])
			//find each parameter-value pair which seems to be okay. however, there might be unwanted some / or - signs in between.
			//each pair must start with / or - ([/-])
			//next is the parameter name which can be anything but spaces, -, /, or : ([^\\s-/:=])
			//next is the value which can either be one of following: (note: order matters!)
			//  -anything enclosed by " or ' ((\".*\")|('.*'))
			//  -anything but spaces not starting with / nor -  optionally enclosed by spaces (([\\s]*[^/-][^\\s]+[\\s]*))
			//  -anything but / or - ([^/-]+).
			//the argument may end with spaces (([\\s]*))

			RegexOptions ro = new RegexOptions();
			ro = ro | RegexOptions.IgnoreCase;
			ro = ro | RegexOptions.Multiline;
			Regex parseCmdLine = new Regex(correctCmdLineRegEx, ro);

			///For test and debug purposes function Matches() is used which returns
			///a MatchCollection. However, there should never be more than one entry.
			/*MatchCollection mc = ParseCmdLine.Matches(ArgumentLine.ToString());            
			if (mc.Count > 1)
				 throw new Exception("Internal Exception: MatchCollection contains more than 1 entry!");
			foreach (Match m in mc)*/

			///By default use Match() because in case of no match raising ExceptionSyntaxError would be skipped by Matches() and foreach.
			Match m = parseCmdLine.Match(argumentLine.ToString());
			{

				if (m.Success == false)
				{
					///Regular expression did not match ArgumentLine. There might be two / or -.
					///Find out up to where ArgumentLine seems to be okay and raise an exception reporting the rest.
					Int32 lastCorrectPosition = FindMismatchReasonInRegex(correctCmdLineRegEx, argumentLine);
					String probableErrorCause = argumentLine.Substring(lastCorrectPosition);
					throw new InvalidOperationException(ResManager.Translate(
						new Words("Exception,ExceptionSyntaxError,[{0}],ExceptionSyntaxError2,[{1}],ExceptionSyntaxError3".FormatX(argumentLine, probableErrorCause), 
						ValueLib.Comma.StringValue)));
				}
				else
				{
					//RegEx match ArgumentLine, thus syntax is ok.

					///try to add values without parameters to FoundParameter using function
					///AddNewFoundParameter(). Before adding move quotes if any.
					///If those arguments are not allowed AddNewFoundParameter() raises an exception.
					Group uGrp = m.Groups["unknownvalues"];
					if (uGrp != null && uGrp.Value != String.Empty)
					{
						String unknown = uGrp.Value.Trim();
						Regex enclosed = new Regex("^(\".*\")|('.*')$");
						Match e = enclosed.Match(unknown);
						if (e.Length != 0)
							unknown = unknown.Substring(1, unknown.Length - 2);

						AddNewFoundParameter(String.Empty, unknown);
					}

					Group paramGrp = m.Groups["name"];
					Group valueGrp = m.Groups["value"];
					if (paramGrp == null || valueGrp == null)
					{
						//this should never happen.
						throw new Exception("Internal Exception: Commandline parameter(s) incorrect.");
					}

					///RegEx find always pairs of name- and value-group. their count should thus always match.
					if (paramGrp.Captures.Count != valueGrp.Captures.Count)
						throw new Exception("Internal Exception: Number of parameters and number of values is not equal. This should never happen.");

					///try to add each name-value-match to FoundParameters using AddNewFoundParameter() function.
					///if value is quoted, remove quotes before calling AddNewFoundParameter().
					///if value is of wrong type AddNewFoundParameter() throws an exception.
					for (Int32 i = 0; i < paramGrp.Captures.Count; i++)
					{
						//if there are spaces at either side of value or param, trim those.
						String value = valueGrp.Captures[i].ToString().Trim();
						String param = paramGrp.Captures[i].ToString().Trim();
						Regex enclosed = new Regex("^(\".*\")|('.*')$");
						Match e = enclosed.Match(value);
						if (e.Length != 0)
							value = value.Substring(1, value.Length - 2);
						AddNewFoundParameter(param, value);
					}
				}
			}
			CheckRequiredParameters();
		}

		/// <summary>
		/// <para>Starts the parsing process. Throws CmdLineArgumentExceptions in case of errors.</para>
		/// <para>Afterwards use the enumerator or the dictionary interface to access the found paramters and their values.</para>
		/// </summary>
		/// <param name="arguments">Arguments as string array passed via command line to the program.</param>
		public void Parse(String[] arguments)
		{
			String mArgs = String.Empty;
			foreach (String s in arguments)
			{
				mArgs += s + " ";
			}
			Parse(mArgs);
		}

		private void CheckRequiredParameters()
		{
			foreach (KeyValuePair<String, ParameterDefinition> param in _wantedParameters)
			{
				if (param.Value.AllowType == ParamAllowType.Required)
				{
					if (_foundParameters.ContainsKey(param.Key) == false)
					{
						throw new ExceptionRequiredParameterMissing(ResManager.Translate(
							new Words("Exception,ExceptionRequiredParameterMissing,[{0}],ExceptionRequiredParameterMissing2".FormatX(param.Key), ",")));
					}
				}
			}
		}

		/// <summary>
		/// <para>Creates information for command line usage for user.</para>        
		/// <para>To create this usage string information passed to CLAParser by function Parameter() is used.</para>
		/// <para>Format of returned string:</para>
		/// <para>&#182;</para>
		/// <para>Usage:</para>
		/// <para>name_of_program.exe /output_file &lt;string&gt; /character &lt;string&gt; /number &lt;int&gt; [/v [/v [...]]]</para>
		/// </summary>
		/// <returns></returns>
		public String GetUsage()
		{
			String value = String.Empty;
			String usage = ResManager.Translate("Usage") + "\r\n" + System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
			String optionalBracketLeft = String.Empty;
			String optionalBracketRight = String.Empty;
			String paramString;

			for (Int32 i = 0; i < 2; i++)
			{
				foreach (KeyValuePair<String, ParameterDefinition> param in _wantedParameters)
				{
					//first take only required parameters then only optional
					if (i == 0 && param.Value.AllowType == ParamAllowType.Optional) continue;
					else if (i == 1 && param.Value.AllowType == ParamAllowType.Required) continue;

					paramString = param.Key;
					if (param.Value.AllowType == ParamAllowType.Optional)
					{
						optionalBracketLeft = "[";
						optionalBracketRight = "]";
					}
					switch (param.Value.ValueType)
					{
						default:
						case ECommandArgumentValueTypes.Bool: value = String.Empty; break;
						case ECommandArgumentValueTypes.String: value = " <" + ResManager.Translate("String") + ">"; break;
						case ECommandArgumentValueTypes.Int: value = " <" + ResManager.Translate("Int") + ">"; break;
						case ECommandArgumentValueTypes.OptionalString: value = " [<" + ResManager.Translate("String") + ">]"; break;
						case ECommandArgumentValueTypes.OptionalInt: value = " [<" + ResManager.Translate("Int") + ">]"; break;
						case ECommandArgumentValueTypes.MultipleBool: value = String.Empty; paramString += " [" + ParameterPrefix + paramString + " [...]]"; break;
						case ECommandArgumentValueTypes.MultipleInts: value = " [<" + ResManager.Translate("Int") + "1> [<" + ResManager.Translate("Int") + "2> [...]]]"; break;
					}
					usage += " " + optionalBracketLeft + ParameterPrefix + paramString + value + optionalBracketRight + " ";
				}
			}
			return usage;
		}

		/// <summary>
		/// <para>Creates information about each parameter which can be displayed to user as a help.</para>
		/// <para>To create this help string information passed to CLAParser by function Parameter() is used.</para>
		/// <para>Format of returned string:</para>
		/// <para>&#182;</para>
		/// <para>Parameters:</para>
		/// <para>Required:</para>
		/// <para>/output_file : Specify output file.</para>
		/// <para>/character   : Character to be written to output file.</para>
		/// <para>/number      : Number of times to write character to output file.</para>
		/// <para>&#182;</para>
		/// <para>Optional</para>
		/// <para>/v : Define (multiple) /v flag(s) for verbose output. Each /v increases verbosity more.</para>
		/// </summary>
		/// <returns>string with information about each parameter</returns>
		public String GetParameterInfo()
		{
			String parameterInfo = ResManager.Translate("Parameters") + "\r\n";

			for (Int32 i = 0; i < 2; i++)
			{
				//find the longest parameter for this section (i==0->required, i==1->optional)
				Int32 longestParamter = 0;
				foreach (KeyValuePair<String, ParameterDefinition> param in _wantedParameters)
					if ((i == 0 && param.Value.AllowType == ParamAllowType.Required) ||
						 (i == 1 && param.Value.AllowType == ParamAllowType.Optional))
						if (longestParamter < param.Key.Length)
							longestParamter = param.Key.Length;

				//Print section header only of there is at least one parameter.
				if (longestParamter > 0 && i == 0) parameterInfo += ResManager.Translate("Required") + "\r\n";
				else if (longestParamter > 0 && i == 1) parameterInfo += "\r\n" + ResManager.Translate("Optional") + "\r\n";

				foreach (KeyValuePair<String, ParameterDefinition> param in _wantedParameters)
				{
					//first take only required parameters then only optional
					if (i == 0 && param.Value.AllowType == ParamAllowType.Optional) continue;
					else if (i == 1 && param.Value.AllowType == ParamAllowType.Required) continue;

					parameterInfo += ParameterPrefix + param.Key + new String(' ', longestParamter - param.Key.Length) + " : " + param.Value.Help + "\r\n";
				}
			}
			return parameterInfo;
		}

		/// <summary>
		/// Returns a enumerator which walks through the dictionary of found parameters.
		/// </summary>
		/// <returns>enumerator of dictionary of found parameters</returns>
		/// <remarks>Needed since Implementing IEnumerable</remarks>
		public IEnumerator GetEnumerator()
		{
			_enumerator = _foundParameters.GetEnumerator();
			return _enumerator;
		}

		/// <summary>
		/// Sets the enumerator to the next found parameter.
		/// </summary>
		/// <returns>true if there is a next found parameter, else false</returns>
		/// <remarks>Needed since Implementing IEnumerable</remarks>
		public Boolean MoveNext()
		{
			return _enumerator.MoveNext();
		}

		/// <summary>
		/// Resets the enumerator to the initial position in front of the first found parameter.
		/// </summary>
		public void Reset()
		{
			_enumerator.Reset();
		}

		/// <summary>
		/// Returns the current found parameter from enumerator.
		/// </summary>
		public Object Current
		{
			get
			{
				return ((DictionaryEntry)_enumerator.Current);
			}
		}
		/// <summary>
		/// Returns the number of found parameters.
		/// </summary>
		public Int32 Count
		{
			get
			{
				return _foundParameters.Count;
			}
		}

		/// <summary>
		/// <para>Call FindMismatchReasonInRegex() if SearchStr does not match RegEx in order to find out
		/// up to where SearchStr matches and where the mismatch starts.</para>
		/// <para>&#182;</para>
		/// <para>Decomposes regular expression RegEx into sub expressions according to parenthesis groupings.
		/// Each sub expression which can be matched, indicates that SearchStr is valid up to that position.
		/// Thus this function can find out up to which position SearchStr is valid and where probably
		/// an error is located.</para>
		/// </summary>
		/// <param name="regEx">Regular expression which is decomposed.</param>
		/// <param name="searchStr">String which does not match RegEx.</param>
		/// <returns>Returns the character position where the reason for the regex mismatch probably is located.</returns>
		private Int32 FindMismatchReasonInRegex(String regEx, String searchStr)
		{
			//disassemble RegEx string by finding all opening parentheses and their matching closing parts.
			SortedDictionary<Int32, Int32> parentesis = new SortedDictionary<Int32, Int32>();
			Stack<Int32> openP = new Stack<Int32>();
			try
			{
				for (Int32 i = 0; i < regEx.Length; i++)
				{
					if (regEx[i] == '(')
					{
						//make sure that this ( is not escaped!
						if (!((i == 1 && regEx[i - 1] == '\\') ||
								 (i > 1 && regEx[i - 1] == '\\' && regEx[i - 2] != '\\')))
							openP.Push(i);

					}
					else if (regEx[i] == ')')
					{
						//make sure that this ) is not escaped!
						if (!((i == 1 && regEx[i - 1] == '\\') ||
								 (i > 1 && regEx[i - 1] == '\\' && regEx[i - 2] != '\\')))
						{
							Int32 pop = openP.Pop();
							parentesis.Add(pop, i);
						}
					}
				}
				// since RegEx should be valid, this can never happen.
				if (openP.Count != 0) throw new Exception("Internal Exception: Parenthesis not balanced!");
			}
			catch (Exception)
			{
				// since RegEx should be valid, this can never happen.
				throw new Exception("Internal Exception: Parenthesis not balanced!");
			}

			// Parenthesis contains all parenthesis matches ordered by the position of the opening parenthesis
			IEnumerator e = parentesis.GetEnumerator();
			Int32 lastCorrectPosition = 0;
			while (e.MoveNext())
			{
				KeyValuePair<Int32, Int32> c = (KeyValuePair<Int32, Int32>)e.Current;

				//get sub-regular-expression of parenthesis grouping.
				String subRegEx = regEx.Substring(c.Key, c.Value - c.Key + 1);
				Regex sub = null;
				try
				{
					sub = new Regex(subRegEx);
				}
				catch
				{
					//this should never happen since subexpression of a valid regex should still be valid.
					throw new Exception("Internal Exception: SubRegEx invalid: " + subRegEx.ToString());
				}
				Match m = sub.Match(searchStr);
				if (m.Success == true)
				{
					// if there is a match this subexpression matches the SearchStr and the mismatch must
					// follow afterwards.
					// find the end position of the match and increase LastCorrectPosition count to that position.
					// (warning: here the wrong match might be detected,
					// but since its is unlikely that commandline argument contains several identical parts,
					// this potential problem is ignored.)
					Int32 newLastCorrectPosition = searchStr.IndexOf(m.Value) + m.Value.Length;
					if (newLastCorrectPosition > lastCorrectPosition)
						lastCorrectPosition = newLastCorrectPosition;
				}
			}
			return lastCorrectPosition;
		}

		/// <summary>
		/// Adds and parameter-value-pair to FoundParameters if that pair matches the specification defined in WantedParameters.
		/// In case of a mismatch an exception is raised.
		/// </summary>
		/// <param name="newParam">The new parameter which is to be added to FoundParameters.</param>
		/// <param name="newValue">Value which corresponds to NewParam.</param>
		private void AddNewFoundParameter(String newParam, String newValue)
		{
			//just to make sure, however this should never happen.
			if (newParam == null || newValue == null)
				throw new Exception("Internal Exception: NewParam or NewValue in AddNewFoundParameter() == null!");

			// values without parameter is only allowed if AllowValuesWithoutParameter==true.
			if (newParam == String.Empty && AllowValuesWithoutParameter == false)
				throw new ExceptionValueWithoutParameterFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionValueWithoutParameterFound") + newValue + ResManager.Translate("ExceptionValueWithoutParameterFound2"));

			if (newParam == String.Empty && AllowValuesWithoutParameter == true)
			{
				// values without parameter are allowed -> add them to FoundParameters
				_foundParameters.Add(newParam, newValue);
			}
			else
			{
				//NewParam is not empty. Test if it is a WantedParamter, raise exception if not, else add it to FoundParameters.
				if (_wantedParameters.ContainsKey(newParam) == false && AllowAdditionalParameters == false)
					throw new ExceptionUnknownParameterFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionUnknownParameterFound") + newParam + ResManager.Translate("ExceptionUnknownParameterFound2"));
				else if (_wantedParameters.ContainsKey(newParam) == false && AllowAdditionalParameters == false)
					_foundParameters.Add(newParam, newValue);
				else if (_wantedParameters.ContainsKey(newParam) == true)
				{
					//found parameter is wanted. check if value has right format for each ValueType.
					switch (_wantedParameters[newParam].ValueType)
					{
						// bool parameters do not accept any value.
						case ECommandArgumentValueTypes.MultipleBool:
						case ECommandArgumentValueTypes.Bool: if (newValue != String.Empty)
								throw new ExceptionInvalidValueFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionInvalidValueFound") + newParam + ResManager.Translate("ExceptionInvalidValueFoundBool"));
							break;

						// optionalInt might be empty, then make it 0 and treat like a normal int.
						case ECommandArgumentValueTypes.OptionalInt: if (newValue == String.Empty) newValue = "0"; goto case ECommandArgumentValueTypes.Int; //"" is okay for OptionalInt
						// int must be able to be converted to int32 without causing exception!
						case ECommandArgumentValueTypes.Int:                                                                      //else check if integer
							try
							{
								Convert.ToInt32(newValue);
							}
							catch (Exception)
							{
								throw new ExceptionInvalidValueFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionInvalidValueFound") + newParam + ResManager.Translate("ExceptionInvalidValueFoundInt"));
							}
							break;

						// multipleInt must be splitted and then be converted to int32.
						case ECommandArgumentValueTypes.MultipleInts:
							try
							{
								Regex split = new Regex("[\\s]+");
								String[] values = split.Split(newValue);
								foreach (String value in values)
									Convert.ToInt32(value);
							}
							catch (Exception)
							{
								throw new ExceptionInvalidValueFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionInvalidValueFound") + newParam + ResManager.Translate("ExceptionInvalidValueFoundInts"));
							}
							break;

						// String can be anything but not empty.
						case ECommandArgumentValueTypes.String:
							if (newValue == String.Empty)
								throw new ExceptionInvalidValueFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionInvalidValueFound") + newParam + ResManager.Translate("ExceptionInvalidValueFoundString"));
							break;

						// OptionalString can be anything. No check necessary.
						case ECommandArgumentValueTypes.OptionalString: break;

						// this should never happen because all cases are matched!
						default: throw new Exception("Internal Exception: Unmatch case in AddNewFoundParameter()!");
					}

					// now parameter is wanted and format is okay. insert param and value into FoundParameters
					// if param does not already exists!
					// only exception: multipleBool
					if (_foundParameters.ContainsKey(newParam))
					{
						if (_wantedParameters[newParam].ValueType != ECommandArgumentValueTypes.MultipleBool)
							throw new ExceptionRepeatedParameterFound(ResManager.Translate("Exception") + ResManager.Translate("ExceptionRepeatedParameterFound") + newParam + ResManager.Translate("ExceptionRepeatedParameterFoundOnce"));
						else
						{
							_foundParameters[newParam] = (Convert.ToInt32(_foundParameters[newParam]) + 1).ToString();
						}

					}
					else
					{
						if (_wantedParameters[newParam].ValueType == ECommandArgumentValueTypes.MultipleBool)
							_foundParameters[newParam] = "1";
						else
							_foundParameters.Add(newParam, newValue);
					}

				}
			}

		}


		/// <summary>
		/// Raised if a parameter which was previously defined by Parameter() is not part of argument line.
		/// </summary>
		public class ExceptionRequiredParameterMissing : ArgumentException
		{
			public ExceptionRequiredParameterMissing() { }
			public ExceptionRequiredParameterMissing(String message) : base(message) { }
		}
		/// <summary>
		/// Raised if a parameter which was not previously defined by Parameter() is part of argument line and
		/// AllowAdditionalParameters is not set to true.
		/// </summary>
		public class ExceptionUnknownParameterFound : ArgumentException
		{
			public ExceptionUnknownParameterFound() { }
			public ExceptionUnknownParameterFound(String message) : base(message) { }
		}
		/// <summary>
		/// Raised if a parameter holds a value which does not comply with its specification defined by
		/// previous call of Parameter().
		/// </summary>
		public class ExceptionInvalidValueFound : ArgumentException
		{
			public ExceptionInvalidValueFound() { }
			public ExceptionInvalidValueFound(String message) : base(message) { }
		}
		/// <summary>
		/// Raised if a parameter was used more than once, e.g. program.exe /p value1 /p value2
		/// Only exception for parameters of type MultipleBool; no exception raised for program.exe /mp /mp
		/// </summary>
		public class ExceptionRepeatedParameterFound : ArgumentException
		{
			public ExceptionRepeatedParameterFound() { }
			public ExceptionRepeatedParameterFound(String message) : base(message) { }
		}

		/// <summary>
		/// Raised if a parameter of type String, Int, or MultipleInts does not hold a value.
		/// </summary>
		public class ExceptionValueWithoutParameterFound : ArgumentException
		{
			public ExceptionValueWithoutParameterFound() { }
			public ExceptionValueWithoutParameterFound(String message) : base(message) { }
		}

		private ResourcesManager ResManager => _resManager ?? (_resManager = new ResourcesManager("Singularity.Resources.CommandLineParser", this.GetType().Assembly));
		private ResourcesManager _resManager;

	}
}

