using System;
using System.Linq;
using System.Text.RegularExpressions;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Domain.Interfaces;
using MembershipSystem.Repository;

namespace MembershipSystem.Domain
{
    public class EmployeeService : IEmployeeService
    {
        private readonly SharedContext _context;

        public EmployeeService(SharedContext context)
        {
            _context = context;
        }

        public EmployeeService()
        {
            _context = new SharedContext();
        }

        public void CreatingEmployee(DomainModels.Employee employee)
        {
            using (var shared = _context)
            {
                if (employee.EmailAddress != null && employee.MembershipCardId != null
                                                  && employee.FirstName != null 
                                                  && employee.LastName != null
                                                  && employee.Pin != null)
                {
                    var card = shared.Employees.SingleOrDefault(s =>
                        s.MembershipCardDbModelId == employee.MembershipCardId);

                    if (card == null)
                    {
                        var emailPattern =
                            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

                        if (Regex.IsMatch(employee.EmailAddress, emailPattern))
                        {
                            var phonePattern = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";

                            string pinRegEx = "\\b\\d{4}\\b";
                            if (Regex.IsMatch(employee.Pin, pinRegEx))
                            {

                                if (MembershipCardId(employee.MembershipCardId))
                                {
                                    if (Regex.IsMatch(employee.MobileNumber, phonePattern))
                                    {
                                        var membershipCard = new Repository.DbModels.MembershipCardDbModel()
                                        {
                                            MembershipCardId = employee.MembershipCardId,
                                            Balance = 0
                                        };


                                        var employeeOb = new Repository.DbModels.EmployeeDbModel()
                                        {
                                            EmployeeId = employee.EmployeeId.ToString(),
                                            FirstName = employee.FirstName,
                                            EmailAddress = employee.EmailAddress,
                                            LastName = employee.LastName,
                                            MobileNumber = employee.MobileNumber,
                                            Pin = employee.Pin,
                                            MembershipCardDbModelId = employee.MembershipCardId
                                        };

                                        membershipCard.EmployeeDbModel = employeeOb;

                                        shared.MembershipCards.Add(membershipCard);
                                        shared.Employees.Add(employeeOb);

                                        shared.SaveChanges();
                                    }
                                    else
                                    {
                                        throw new InvalidPhoneNumberException();
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
                        else
                        {
                            throw new EmailInvalidException();
                        }
                    }
                    else
                    {
                        throw new CardAlreadyRegisteredException();
                    }
                }
                else
                {
                    throw new NullInputException();
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

            Array.Clear(count,0,count.Length-1);

            foreach (char s in card)
            {
                if (count[s] == 1)
                    return false;
                else
                {
                    count[(int) s] += 1;
                }
            }
             
            return true;
        }

    }
}
