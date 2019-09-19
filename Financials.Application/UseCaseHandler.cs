using Financials.Application.Users.UseCases;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application
{
    public static class UseCaseHandler
    {
        public static TOutput Handle<TUseCase, TInput, TOutput>(TUseCase useCase, TInput input) where TUseCase: IUseCase<TInput, TOutput>
        {
            return useCase.Handle(input);
        }

        public static void testc()
        {
            UseCaseHandler.Handle<AddUser, AddUserInput, User>(new AddUser(null,null,null), new AddUserInput());
        }
    }
}
