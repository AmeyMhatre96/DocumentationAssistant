using System.ComponentModel;

namespace DocumentationExpert.Vsix
{
	/// <summary>
	/// The general options.
	/// </summary>
	public class General : BaseOptionModel<General>
	{
		/// <summary>
		/// Gets or sets the header text.
		/// </summary>
		[Category("General")]
		[DisplayName("File Header content")]
		[Description("Enter the header content. \n\rAvailable variables: {fileName}, {date}")]
		[DefaultValue("")]
		[Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string FileHeaderText { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to disable for unit tests.
		/// </summary>
		[Category("General")]
		[DisplayName("Disable for test projects")]
		[Description("Whether the documentation suggestions should be shown on members of test project.")]
		[DefaultValue(true)]
		public bool DisableForTestProjects { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether should replace file header.
		/// </summary>
		[Category("General")]
		[DisplayName("Replace existing file header")]
		[Description("If a file header already exists, should it be replaced.")]
		[DefaultValue(false)]
		public bool ShouldReplaceFileHeader { get; set; } = false;

		[Category("My category")]
		[DisplayName("My Enum")]
		[Description("Select the members for which you want to see documentation suggestions.")]
		[DefaultValue(EnableForMembers.All)]
		[TypeConverter(typeof(EnumDisplayNameConverter<EnableForMembers>))]
		public EnableForMembers EnableForMembers { get; set; } = EnableForMembers.All;
	}

	public enum EnableForMembers
	{
		[Description("Value Three")]
		Public,
		[Description("Value Three")]
		PublicAndInternal,
		[Description("Value Three")] 
		PublicInternalAndProtected,
		[Description("Value Three")] 
		All
	}

	public class EnumDisplayNameConverter<T> : EnumConverter
	{
		public EnumDisplayNameConverter() : base(typeof(T))
		{
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (value != null)
				{
					var enumValue = (Enum)value;
					return GetEnumDisplayName(enumValue);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private string GetEnumDisplayName(Enum enumValue)
		{
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
			if (fieldInfo != null)
			{
				var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
				if (descriptionAttribute != null)
				{
					return descriptionAttribute.Description;
				}
			}
			return enumValue.ToString();
		}
	}
}
