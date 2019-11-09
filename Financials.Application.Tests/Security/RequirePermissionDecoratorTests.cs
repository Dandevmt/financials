using Financials.CQRS;
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

        class TestCommandWithPermission : ICommand, IRequirePermission
        {
            public string TenantId => "1";

            public Permission Permission => Permission.AddUsers;
        }

        class TestHandlerWithPermission : ICommandHandler<TestCommandWithPermission>
        {
            public Task<CommandResult> Handle(TestCommandWithPermission command)
            {
                return CommandResult.Success().AsTask();
            }
        }


        [TestMethod]
        public async Task CanDoNotCalled()
        {          

            var handler = new TestHandlerWithPermission();

            await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await handler.Handle(new TestCommandWithPermission());
            });

            access.Verify(x => x.CanDo(It.IsAny<string>(), It.IsAny<Permission>()), Times.Never);
        }

        [TestMethod]
        public async Task CanDoCalled()
        {
            access.Setup(x => x.CanDo("1", Permission.AddUsers)).Returns(true);

            var decorator = new RequirePermissionDecorator<TestCommandWithPermission>(new TestHandlerWithPermission(), access.Object);
            var result = await decorator.Handle(new TestCommandWithPermission());

            access.Verify(x => x.CanDo("1", Permission.AddUsers), Times.Once);
        }


        [TestMethod]
        public async Task PermissionDenied()
        {
            access.Setup(x => x.CanDo("1", Permission.AddUsers)).Returns(false);

            var decorator = new RequirePermissionDecorator<TestCommandWithPermission>(new TestHandlerWithPermission(), access.Object);
            var result = await decorator.Handle(new TestCommandWithPermission());

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CommandError.Forbidden().Code, result.Error.Code);
        }
    }
}
