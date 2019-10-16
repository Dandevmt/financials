using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.UserManagement.Repositories
{
    public interface IValidationCodeRepository
    {
        ValidationCode Get(Guid userId, ValidationCodeType type);
        ValidationCode GetFederationCode(string federationCode);
        ValidationCode Add(ValidationCode code);
        bool Delete(Guid userId, ValidationCodeType type);
    }
}
