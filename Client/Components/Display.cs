using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ContosoUniversity.Client.Components
{
    public class Display<TValue> : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter] public Expression<Func<TValue>> For { get; set; }

        protected override void OnParametersSet()
        {
            if (For == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a value for the " +
                                                    $"{nameof(For)} parameter.");
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "span");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddContent(2, GetStringValue());
            builder.CloseElement();
        }

        private string GetStringValue()
        {
            var expression = (MemberExpression)For.Body;
            var stringFormatAttribute =
                expression.Member.GetCustomAttribute(typeof(DisplayFormatAttribute)) as DisplayFormatAttribute;
            if(stringFormatAttribute == null)
            {
                var func = For.Compile();
                var result = func();
                if(result == null)
                {
                    return string.Empty;
                }

                return func().ToString();
            }

            var stringFormat = stringFormatAttribute.DataFormatString;
            var compiledExpression = For.Compile();
            return string.Format(stringFormat, compiledExpression());
        }
    }
}
