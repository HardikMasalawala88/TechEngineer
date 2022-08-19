namespace TechEngineer.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            //Admin side role
            public const string SuperAdmin = "Superadmin";
            public const string Admin = "Admin";
            public const string Engineer = "Engineer";

            //User side role
            // OrganizationITHead -> BranchITHead
            public const string OrganizationITHead = "OrganizationITHead";
            public const string BranchITHead = "BranchITHead";
            public const string StoreAdmin = "StoreAdmin";
            public const string StoreManager = "StoreManager";
            public const string StoreUser = "StoreUser";
        }

        public static class Tenants
        {
            //Admin side role
            public const string SuperAdmin = "Superadmin";
            public const string Admin = "Admin";
            public const string Engineer = "Engineer";

            //User side role
            // OrganizationITHead -> BranchITHead
            public const string OrganizationITHead = "OrganizationITHead";
            public const string BranchITHead = "BranchITHead";
            public const string StoreAdmin = "StoreAdmin";
            public const string StoreManager = "StoreManager";
            public const string StoreUser = "StoreUser";
        }
    }
}
