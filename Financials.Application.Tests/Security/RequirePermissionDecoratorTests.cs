using Financials.CQRS;
using Financials.Application.UserManagement.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Financials.Application.Configuration;

namespace Financials.Application.Tests.Security
{
    [TestClass]
    public class RequirePermissionDecoratorTests
    {
        AppSettings appSettings = new AppSettings() { ApplicationName = "App" };
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

        //class TestHandlerWithPermission : ICommandHandler<TestCommandWithPermission>
        //{
        //    public Task<Result> Handle(TestCommandWithPermission command)
        //    {
        //        return Result.Success().AsTask();
        //    }
        //}


        //[TestMethod]
        //public async Task CanDoNotCalled()
        //{          

        //    var handler = new TestHandlerWithPermission();

        //    await Assert.ThrowsExceptionAsync<Exception>(async () =>
        //    {
        //        await handler.Handle(new TestCommandWithPermission());
        //    });

        //    access.Verify(x => x.CanDo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        //}

        //[TestMethod]
        //public async Task CanDoCalled()
        //{
        //    access.Setup(x => x.CanDo("1", appSettings.ApplicationName, Permission.AddUsers.ToString())).Returns(true);

        //    var decorator = new RequirePermissionDecorator<TestCommandWithPermission>(new TestHandlerWithPermission(), access.Object, appSettings);
        //    var result = await decorator.Handle(new TestCommandWithPermission());

        //    access.Verify(x => x.CanDo("1", "app", Permission.AddUsers.ToString()), Times.Once);
        //}


        //[TestMethod]
        //public async Task PermissionDenied()
        //{
        //    access.Setup(x => x.CanDo("1", appSettings.ApplicationName, Permission.AddUsers.ToString())).Returns(false);

        //    var decorator = new RequirePermissionDecorator<TestCommandWithPermission>(new TestHandlerWithPermission(), access.Object, appSettings);
        //    var result = await decorator.Handle(new TestCommandWithPermission());

        //    Assert.IsFalse(result.IsSuccess);
        //    Assert.AreEqual(CommandError.Forbidden().Code, result.Error.Code);
        //}
    }
}
