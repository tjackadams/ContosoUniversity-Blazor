using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ContosoUniversity.Client.Components
{
    public class Label<TValue> : ComponentBase
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
            builder.OpenElement(0, "label");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddContent(2, GetDisplayName());
            builder.CloseElement();
        }

        private string GetDisplayName()
        {
            var expression = (MemberExpression)For.Body;
            var value = expression.Member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

            return value?.Name ?? expression.Member.Name ?? "";
        }
    }
}