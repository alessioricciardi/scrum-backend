namespace scrum_backend.Authorization.Policies
{
    public static class AuthorizationPolicies
    {
        public static string ProjectAdminOnly => "ProjectAdminOnly";
        public static string ProductOwnerOnly => "ProductOwnerOnly";
        public static string ScrumMasterOnly => "ScrumMasterOnly";
        public static string DeveloperOnly => "DeveloperOnly";
        public static string ProjectMemberOnly = "ProjectMemberOnly";

    }
}
