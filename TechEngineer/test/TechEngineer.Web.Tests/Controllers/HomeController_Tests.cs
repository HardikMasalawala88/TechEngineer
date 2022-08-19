using System.Threading.Tasks;
using TechEngineer.Models.TokenAuth;
using TechEngineer.Web.Controllers;
using Shouldly;
using Xunit;

namespace TechEngineer.Web.Tests.Controllers
{
    public class HomeController_Tests: TechEngineerWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}