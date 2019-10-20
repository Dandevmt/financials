using Financials.Application.CQRS;
using Financials.Application.UserManagement.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Tests.Security
{
    [TestClass]
    public class RequirePermissionDecoratorTests
    {
        Mock<IAccess> access;

        [TestInitialize]
        public void Initialize()
        {
            access = new Mock<IAccess>();            
        }

        class TestCommand : ICommand { }

        class TestHandlerWithoutPermission : ICommandHandler<TestCommand>
        {
            public Task<CommandResult> Handle(TestCommand command)
            {
                return CommandResult.Success().AsTask();
            }
        }
        [RequirePermission(Permission.AddUser)]
        class TestHandlerWithPermission : ICommandHandler<TestCommand>
        {
            public Task<CommandResult> Handle(TestCommand command)
            {
                return CommandResult.Success().AsTask();
            }
        }

        [TestMethod]
        public async Task CanDoNotCalled()
        {          

            var decorator = new RequirePermissionDecorator<TestCommand>(new TestHandlerWithoutPermission(), access.Object);

            await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await decorator.Handle(new TestCommand());
            });

            access.Verify(x => x.CanDo(It.IsAny<Permission>()), Times.Never);
        }

        [TestMethod]
        public async Task CanDoCalled()
        {
            access.Setup(x => x.CanDo(Permission.AddUser)).Returns(true);

            var decorator = new RequirePermissionDecorator<TestCommand>(new TestHandlerWithPermission(), access.Object);
            var result = await decorator.Handle(new TestCommand());

            access.Verify(x => x.CanDo(Permission.AddUser), Times.Once);
        }


        [TestMethod]
        public async Task PermissionDenied()
        {
            access.Setup(x => x.CanDo(Permission.AddUser)).Returns(false);

            var decorator = new RequirePermissionDecorator<TestCommand>(new TestHandlerWithPermission(), access.Object);
            var result = await decorator.Handle(new TestCommand());

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CommandError.Forbidden().Code, result.Error.Code);
        }
    }
}
