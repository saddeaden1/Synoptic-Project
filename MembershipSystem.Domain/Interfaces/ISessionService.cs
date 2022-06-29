namespace MembershipSystem.Domain.Interfaces
{
    public interface ISessionService
    {
        string LoggingIn(string membershipCard, string pin);
        void LoggingOff(string sessionToken,string pin);
        bool HasTimedOut(string sessionToken);
        void UpdateLastRequestTime(string sessionToken);
        bool ValidateUser(string sessionToken, string pin);
        void TimeOutAllOldSessions();
    }
}