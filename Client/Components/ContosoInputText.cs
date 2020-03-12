using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace ContosoUniversity.Client.Components
{
    public class ContosoInputText<TValue> : InputBase<TValue>
    {

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "class", CssClass);
            builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
            builder.AddAttribute(4, "onchange",
                EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value,
                    CurrentValueAsString));
            builder.CloseElement();
        }

        protected override bool TryParseValueFromString(string value, out TValue result,
            out string validationErrorMessage)
        {
            return InputConverter.ParseValue(GetType(), value, out result, out validationErrorMessage);
        }

    }
}