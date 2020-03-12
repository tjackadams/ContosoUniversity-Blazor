using System;

namespace ContosoUniversity.Infrastructure.ModelBinding
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BindRequestProperty : Attribute
    {
    }
}
