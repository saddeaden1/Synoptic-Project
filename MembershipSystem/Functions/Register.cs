using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using MembershipSystem.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MembershipSystem.Domain.DomainModels;
using MembershipSystem.Domain.Exceptions;
using MembershipSystem.Domain.Interfaces;
using Newtonsoft.Json;

namespace MembershipSystem.Functions
{
    public class Register
    {

        private readonly IEmployeeService _employeeService;

        public Register(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [FunctionName("Register")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", "post", Route = "Register")] HttpRequest req,
            ILogger log)
        {

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<Employee>(content);

            var service = new EmployeeService();

            try
            {
                service.CreatingEmployee(model);
            }
            catch (EmailInvalidException)
            {
                return new BadRequestObjectResult("Email is invalid");
            }
            catch (CardAlreadyRegisteredException)
            {
                return new BadRequestObjectResult("User already exists");
            }
            catch (InvalidPhoneNumberException)
            {
                return new BadRequestObjectResult("Invalid Phone Number");
            }
            catch (InvalidPinTypeException)
            {
                return new BadRequestObjectResult("Invalid Pin");
            }
            catch (InvalidCardTypeException)
            {
                return new BadRequestObjectResult("Invalid Card ");
            }
            catch (NullInputException)
            {
                return new BadRequestObjectResult("You have inputted a empty value");
            }
            catch (Exception e)
            {
                log.LogInformation(e.ToString());
                return new InternalServerErrorResult();
            }


            return new OkResult();
        }
    }
}