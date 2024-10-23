namespace SerBeast_API.Utilities
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public static class BookingStatusExtensions
    {
        public static string ToFriendlyString(this BookingStatus status)
        {
            return status.ToString();
        }
    }
}
