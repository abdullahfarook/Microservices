using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Identity.Api.ViewModel;

namespace Identity.Api.Core
{
    public static class Permissions
    {

        public static class Users
        {
            public const string View = "permissions.users.view";
            public const string Manage = "permissions.users.manage";
        }
        public static class Roles
        {
            public const string View = "permissions.roles.view";
            public const string Manage = "permissions.roles.manage";
        }
    }
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        //public const string UsersPermissionGroupName = "User Permissions";
        //public static ApplicationPermission ViewUsers = new ApplicationPermission("View Users", Permissions.Users.View, UsersPermissionGroupName, "Permission to view other users account details");
        //public static ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users", Permissions.Users.Manage, UsersPermissionGroupName, "Permission to create, delete and modify other users account details");

        //public const string RolesPermissionGroupName = "Role Permissions";
        //public static ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", Permissions.Roles.View, RolesPermissionGroupName, "Permission to view available roles");
        //public static ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles", Permissions.Roles.Manage, RolesPermissionGroupName, "Permission to create, delete and modify roles");
        ////public static ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles", Permissions.Roles.Assign, RolesPermissionGroupName, "Permission to assign roles to users");


        static ApplicationPermissions()
        {
            var allPermissions = new List<ApplicationPermission>();

            var nestedClasses = typeof(Permissions).GetNestedTypes();
            foreach (var nestedClass in nestedClasses)
            {
                foreach (FieldInfo field in nestedClass.GetFields())
                {
                    allPermissions.Add(new ApplicationPermission(name:
                        $"{field.Name} {nestedClass.Name}",
                        field.GetValue(nestedClass).ToString(),
                        $"{nestedClass.Name}",
                        $"Permission to {field.Name} {nestedClass.Name}")
                    );
                }
            }


            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.SingleOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.SingleOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }
        public static PermissionGroup[] GetAllPermissions()
        {

            var groups = AllPermissions.GroupBy(p => p.GroupName);
            return groups.Select(g => new PermissionGroup
            {
                Name = g.Key,
                Permissions = g
                   .Select(p => new ClaimViewModel
                   {
                       Name = p.Name,
                       Value = p.Value,
                       Description = p.Description
                   })
                   .ToList()
            }).ToArray();
        }
        //public static string[] GetAdministrativePermissionValues()
        //{
        //    return new string[] { ManageUsers, ManageRoles };
        //}
    }
    public class ApplicationPermission
    {

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }



        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }


        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}
