using System;
namespace NuSight.Core.Attributes
{
    public class AutofacRegistrationOrderAttribute : Attribute
    {
        public const string AttributeName = "Order";

        public AutofacRegistrationOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
