﻿using Financials.Application.UserManagement.Repositories;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.UseCases
{
    public class VerifyEmail : IUseCase<VerifyEmailInput, bool>
    {
        private IValidationCodeRepository codeRepo;
        private ICredentialRepository credRepo;

        public VerifyEmail(IValidationCodeRepository codeRepo, ICredentialRepository credRepo)
        {
            this.codeRepo = codeRepo;
            this.credRepo = credRepo;
        }

        public async Task Handle(VerifyEmailInput input, Action<bool> presenter)
        {
            var code = codeRepo.Get(input.UserId, ValidationCodeType.Email);
            if (code == null || !code.Code.Equals(input.Code) || (DateTime.Today - code.CreatedDate).TotalMinutes > 15)
                UserManagementError.InvalidEmailVerificationCode().Throw();

            var creds = credRepo.Get(input.UserId);
            creds.EmailVerified = DateTime.Now;

            var res = credRepo.UpdateOne(creds);
            if (res == null)
            {
                UserManagementError.EmailCouldNotUpdateDatabase().Throw();
            } else
            {
                presenter(true);
            }
        }
    }
}