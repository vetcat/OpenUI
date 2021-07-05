using System;

namespace Libs.OpenUI.Localization
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class LocalizationAttribute : Attribute
	{
		public string Value { get; }

		public LocalizationAttribute(string value)
		{
			Value = value;
		}
	}
}