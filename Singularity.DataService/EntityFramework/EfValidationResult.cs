using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace Singularity.DataService
{
	public class EfValidationResult : ValidationResult
	{
		public EfValidationResult(String errorMessage, IEnumerable<String> memberNames = null) : base(errorMessage, memberNames)
		{
		}

		public EfValidationResult(String errorMessage, String memberName, String propertyName = null) : base(errorMessage, null)
		{
			MemberName = memberName;
			PropertyName = propertyName;
		}


		public override String ToString()
		{
			if (MemberName.IsEmpty() && PropertyName.IsEmpty() && MemberNames.IsEmpty())
			{
				return $"Error: {ErrorMessage}";
			}
			if (!MemberNames.IsEmpty())
			{
				return $"Error: {ErrorMessage} Member Names: " + String.Join(ValueLib.CommaSpace.StringValue, MemberNames);
			}
			if (PropertyName.IsEmpty())
			{
				return $"Error: {ErrorMessage}, Member Name: {MemberName}, ";
			}
			return $"Error: {ErrorMessage}, Member Name: {MemberName}, Property: {PropertyName}, ";
		}

		public String MemberName { get; set; }
		public String PropertyName { get; set; }
	}
}
