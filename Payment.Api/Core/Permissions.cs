using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Api.Core
{
    public static class Permissions
    {
        public static class Methods
        {
            public const string View = "permissions.methods.view";
            public const string Manage = "permissions.methods.manage";
        }

        public static class Invoices
        {
            public const string View = "permissions.invoices.view";
        }

        public static class Coupons
        {
            public const string View = "permissions.coupons.view";
            public const string Manage = "permissions.coupons.manage";
        }

        public static class Subscriptions
        {
            public const string View = "permissions.subscriptions.view";
            public const string Manage = "permissions.subscriptions.manage";
        }

        public static class Packages
        {
            public const string View = "permissions.packages.view";
            public const string Manage = "permissions.packages.manage";
        }

        public static class Products
        {
            public const string View = "permissions.products.view";
            public const string Manage = "permissions.products.manage";
        }
    }
}
