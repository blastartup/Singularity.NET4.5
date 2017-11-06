using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Singularity;

namespace Singularity.Web
{
	public static class HtmlRender
	{
		/// <summary>
		/// Creates a drop down list which is bound to an enum from the model.
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression">The enum to be bound to the drop down list.</param>
		/// <returns></returns>
		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			return EnumDropDownListFor(htmlHelper, expression, null);
		}

		/// <summary>
		/// Creates a drop down list which is bound to an enum from the model.
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression">The enum to be bound to the drop down list.</param>
		/// <param name="htmlAttributes">Any HTML attributes to add on to the drop down list.</param>
		/// <returns></returns>
		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			Type enumType = GetNonNullableModelType(metadata);
			IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

			IEnumerable<SelectListItem> items = from value in values
												select new SelectListItem
												{
													Text = GetEnumName(value),
													Value = value.ToString(),
													Selected = value.Equals(metadata.Model)
												};

			// If the enum is nullable, add an 'empty' item to the collection
			if (metadata.IsNullableValueType)
				items = SingleEmptyItem.Concat(items);

			return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
		}

		private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
		{
			Type realModelType = modelMetadata.ModelType;

			Type underlyingType = Nullable.GetUnderlyingType(realModelType);
			if (underlyingType != null)
			{
				realModelType = underlyingType;
			}
			return realModelType;
		}

		/// <summary>
		/// Method used within this class to get the name of the enum from the enum additional attributes.
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value">The enum to be bound to the drop down list.</param>
		/// <returns></returns>
		private static string GetEnumName<TEnum>(TEnum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			EnumAdditionalAttribute[] attributes = (EnumAdditionalAttribute[])fi.GetCustomAttributes(typeof(EnumAdditionalAttribute), false);

			if ((attributes != null) && (attributes.Length > 0))
			{
				return attributes[0].Name;
			}
			return value.ToString();
		}

		private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };
	}
}