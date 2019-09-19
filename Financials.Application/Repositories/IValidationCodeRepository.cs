using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Repositories
{
    public interface IValidationCodeRepository
    {
        ValidationCode Get(Guid userId, ValidationCodeType type);
        ValidationCode Add(ValidationCode code);
        bool Delete(Guid userId, ValidationCodeType type);
    }
}
