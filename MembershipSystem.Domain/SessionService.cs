using System;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Domain.Interfaces;
using MembershipSystem.Repository;
using MembershipSystem.Repository.DbModels;

namespace MembershipSystem.Domain
{
    public class SessionService : ISessionService
    {
        private readonly SharedContext _context;

        public SessionService(SharedContext context)
        {
            _context = context;
        }

        public SessionService()
        {
            _context = new SharedContext();
        }

        public string LoggingIn(string membershipCard, string pin)
        {

            var pinRegEx = "\\b\\d{4}\\b";

            if(Regex.IsMatch(pin, pinRegEx))
            {
                if (MembershipCardId(membershipCard))
                {
                    using (_context)
                    {
                        var card = _context.MembershipCards.SingleOrDefault(s => s.MembershipCardId.ToString() == membershipCard);

                        if (card == null)
                        {
                            throw new CardNotExistException();
                        }

                        var employee = _context.Employees.SingleOrDefault(s => s.MembershipCardDbModelId == membershipCard);

                        if (employee == null)
                        {
                            //told to register
                            throw new CardNotRegisteredException();
                        }

                        if (employee.Pin != pin)
                        {
                            throw new InvalidCredentialException();
                        }

                        var activeSession =
                            _context.ActiveSessions.SingleOrDefault(s => s.EmployeeId == employee.EmployeeId);

                        if (activeSession != null)
                        {
                            throw new AlreadyLoggedInException();
                        }

                        var session = new ActiveSessionDbModel()
                        {
                            ActiveSessionId = Guid.NewGuid(),
                            EmployeeId = employee.EmployeeId,
                            LogOn = DateTime.Now.ToString(),
                            SessionToken = Guid.NewGuid().ToString(),
                            LastRequest = DateTime.Now.ToString()
                        };
                        _context.ActiveSessions.Add(session);
                        _context.SaveChanges();

                        return session.SessionToken;
                    }
                }
                else
                {
                    throw new InvalidCardTypeException();
                }
            }
            else
            {
                throw new InvalidPinTypeException();
            }
        }

        public void LoggingOff(string sessionToken,string pin)
        {
            if (!Guid.TryParse(sessionToken, out Guid result))
            {
                throw new Exception("Invalid Session Token Format");
            }

            var session = _context.ActiveSessions.SingleOrDefault(s => s.SessionToken == sessionToken);

            if (session == null)
            {
                throw new Exception("You have no active session please log in again");
            }

            var employee = _context.Employees.Single(s => s.EmployeeId.ToString() == session.EmployeeId);

            var pinRegEx = "\\b\\d{4}\\b";

            if (Regex.IsMatch(pin, pinRegEx))
            {
                if (employee.Pin != pin)
                {
                    throw new InvalidCredentialException();
                }
            }
            else
            {
                throw new InvalidPinTypeException();
            }

            _context.SessionHistorys.Add(new SessionHistoryDbModel()
            {
                SessionHistoryId = Guid.NewGuid(),
                EmployeeId = session.EmployeeId,
                LogOn = session.LogOn,
                LogOff = DateTime.Now.ToString()
            });

            _context.ActiveSessions.Remove(session);
            _context.SaveChanges();

        }

        public bool HasTimedOut(string sessionToken)
        {
            var session = _context.ActiveSessions.Single(s => s.SessionToken == sessionToken);

            if ((DateTime.Now - DateTime.Parse(session.LastRequest)) > TimeSpan.FromMinutes(10))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateLastRequestTime(string sessionToken)
        {

            try
            {
                var session = _context.ActiveSessions.Single(s => s.SessionToken.ToString() == sessionToken);
                var newSession = new ActiveSessionDbModel()
                {
                    EmployeeId = session.EmployeeId,
                    ActiveSessionId = session.ActiveSessionId,
                    LastRequest = DateTime.Now.ToString(),
                    LogOn = session.LogOn,
                    SessionToken = session.SessionToken
                };
                _context.ActiveSessions.Remove(session);
                _context.ActiveSessions.Add(newSession);
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Updating last request time failed");
            }

        }

        public bool ValidateUser(string sessionToken, string pin)
        {

            if (!Guid.TryParse(sessionToken, out var result))
            {
                throw new Exception("Invalid Session Token Format");
            }

            var activeSession = _context.ActiveSessions.SingleOrDefault(s => s.SessionToken.ToString() == sessionToken);

            if (activeSession == null)
            {
                var oldSession = _context.SessionHistorys.SingleOrDefault(s => s.SessionToken == sessionToken);

                if(oldSession == null)
                {
                    throw new Exception("Invalid Session token");
                }
                else
                {
                    throw new TimeoutException();
                }
            }

            var user = _context.Employees.Single(s => s.EmployeeId == activeSession.EmployeeId);

            if (user.Pin != pin)
            {
                throw new InvalidCredentialException();
            }

            if (HasTimedOut(sessionToken))
            {
                throw new TimeoutException();
            }

            return true;
        }

        public void TimeOutAllOldSessions()
        {

            if (_context.ActiveSessions.Any())
            {
                foreach (var activeSession in _context.ActiveSessions)
                {
                    if (HasTimedOut(activeSession.SessionToken))
                    {
                        _context.SessionHistorys.Add(new SessionHistoryDbModel()
                        {
                            EmployeeId = activeSession.EmployeeId,
                            LogOn = activeSession.LogOn,
                            LogOff = DateTime.Now.ToString()
                        });

                        _context.ActiveSessions.Remove(activeSession);
                        _context.SaveChanges();
                    }
                }
            }
        }

        private bool MembershipCardId(string card)
        {
            string membershipCardRegEx = "\\b[0-9|A-z]{16}\\b";


            if (!Regex.IsMatch(card, membershipCardRegEx))
            {
                return false;
            }

            int[] count = new int[256];

            Array.Clear(count, 0, count.Length - 1);

            foreach (char s in card)
            {
                if (count[s] == 1)
                    return false;
                else
                {
                    count[(int)s] += 1;
                }
            }

            return true;
        }
    }
}