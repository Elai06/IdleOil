namespace Constants
{
    public static class Exceptions
    {
        public const string NOT_ENOUGH_CURRENCY = "User doesn't have enough currency for this operation";
        public const string PURCHASE_UNSUCCESSFUL = "Purchase was not successful";
        public const string PURCHASE_NOT_IN_APP = "Purchase {title} is not in-app";
        public const string PURCHASING_NOT_INITIALIZED = "Purchasing is not initialized";
        public const string PURCHASING_INITIALIZATION_IN_PROGRESS = "Purchasing initialization in progress";
        public const string PURCHASE_IN_PROGRESS = "Purchase is in progress";
        public const string PURCHASE_UNSUCCESSFUL_WITH_REASON = "Purchase was not successful, reason: {reason}";
    }
}