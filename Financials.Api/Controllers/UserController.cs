using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application.Codes;
using Financials.Application.Users;
using Financials.Application.Users.UseCases;
using Financials.Database;
using Financials.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoDatabase mongo;
        private readonly ICodeGenerator codeGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly UserRepository userRepository;
        private readonly ValidationCodeRepository codeRepository;
        private readonly CredentialRepository credRepo;
        public UserController(IMongoDatabase mongo, ICodeGenerator codeGenerator, IPasswordHasher passwordHasher)
        {
            this.mongo = mongo;
            this.codeGenerator = codeGenerator;
            this.passwordHasher = passwordHasher;
            userRepository = new UserRepository(this.mongo);
            codeRepository = new ValidationCodeRepository(this.mongo);
            credRepo = new CredentialRepository(this.mongo);
        }

        [HttpGet]
        public User Get(string id)
        {
            return userRepository.Get(Guid.Parse(id));
        }

        [HttpPost]
        public User Post([FromBody] AddUserInput input)
        {
            User user = null;
            new AddUser(userRepository, codeRepository, codeGenerator, passwordHasher, credRepo).Handle(input, u => user = u);
            return user;
        }
    }
}