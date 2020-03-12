using System;
using System.Globalization;

namespace ContosoUniversity.Client.Components
{
    public static class InputConverter
    {
        public static bool ParseValue<TValue>(Type caller, string value, out TValue result,
            out string validationErrorMessage)
        {
            if (typeof(TValue) == typeof(string))
            {
                result = (TValue)(object)value;
                validationErrorMessage = null;

                return true;
            }

            if (typeof(TValue) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result = (TValue)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }

            if (typeof(TValue) == typeof(byte[]))
            {
                result = (TValue)(object)Convert.FromBase64String(value);
                validationErrorMessage = null;

                return true;
            }

            throw new InvalidOperationException($"{caller} doe not support the type '{typeof(TValue)}'.");
        }
    }
}