using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Common.Net;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CredhubSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredhubController : ControllerBase
    {
        private ILogger<CredhubController> _logger;
        private IConfiguration _configuration;
        private CloudFoundryApplicationOptions _appOptions;
        private CloudFoundryServicesOptions _serviceOptions;

        private string _userName;
        private string _password;

        public CredhubController(
            ILogger<CredhubController> logger,
            IConfiguration configuration,
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> serviceOptions)
        {
            _logger = logger;
            _configuration = configuration;
            _appOptions = appOptions.Value;
            _serviceOptions = serviceOptions.Value;
           
        }

       // GET api/credhub/creds
        [HttpGet]
        [Route("creds")]
        public ActionResult<IEnumerable<string>> GetCreds()
        {
            _userName = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-credhub-service"))
                    .Credentials["share-username"].Value;

            _password = _serviceOptions.Services["credhub"]
                    .First(q => q.Name.Equals("test-credhub-service"))
                    .Credentials["share-password"].Value;

            return new string[] { $"userName: {_userName}", $"password: {_password}" };
        }


        [HttpGet]
        [Route("config")]
        public ActionResult<IEnumerable<string>> GetCredsFromConfig()
        {
            _userName = _configuration["myAppSecrets:userName"];
            _password = _configuration["myAppSecrets:password"];;

            _logger.LogInformation($"username: {_configuration["vcap:services:test-credhub-service:credentials:share-username"]}");

            return new string[] { $"userName: {_userName}", $"password: {_password}" };
        }
        // GET api/credhub/services
        [HttpGet]
        [Route("services")]
        public ActionResult<IEnumerable<string>> GetServices()
        {
            _logger.LogInformation($"service count: {_serviceOptions.Services.Count}");

            return _serviceOptions.Services.Select(s => s.Key).ToList();
        }

    }
}