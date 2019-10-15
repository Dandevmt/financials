using Financials.Application.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Tests.Security
{
    [TestClass]
    public class UseCasePermissionDecoratorTests
    {
        Mock<IUseCase<string, string>> useCase;
        Mock<IAccess> access;
        Mock<IPermissionRequired> useCasePerm;

        [TestInitialize]
        public void Initialize()
        {
            useCase = new Mock<IUseCase<string, string>>();
            useCase.Setup(x => x.Handle(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns(Task.FromResult("test"));
                        
            access = new Mock<IAccess>();            
        }

        [TestMethod]
        public async Task CanDoNotCalled()
        {
            var decorator = new UseCasePermissionDecorator<string, string>(useCase.Object, access.Object);

            await Assert.ThrowsExceptionAsync<Exception>(async () => 
            {
                await decorator.Handle("test", s => { });
            });

            access.Verify(x => x.CanDo(It.IsAny<Permission>()), Times.Never);
        }

        [TestMethod]
        public async Task CanDoCalled()
        {
            useCasePerm = useCase.As<IPermissionRequired>();
            useCasePerm.Setup(x => x.PermissionRequired).Returns(Permission.AddUser);

            access.Setup(x => x.CanDo(Permission.AddUser)).Returns(true);

            var decorator = new UseCasePermissionDecorator<string, string>(useCase.Object, access.Object);
            await decorator.Handle("test", (s) => {});

            access.Verify(x => x.CanDo(useCasePerm.Object.PermissionRequired), Times.Once);
        }


        [TestMethod]
        public async Task PermissionDenied()
        {
            useCasePerm = useCase.As<IPermissionRequired>();
            useCasePerm.Setup(x => x.PermissionRequired).Returns(Permission.AddUser);

            access.Setup(x => x.CanDo(useCasePerm.Object.PermissionRequired)).Returns(false);

            var decorator = new UseCasePermissionDecorator<string, string>(useCase.Object, access.Object);

            await Assert.ThrowsExceptionAsync<Errors.ErrorException>(async () => 
            {
                await decorator.Handle("test", s => { });
            });
        }
    }
}
