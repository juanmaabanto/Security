using N5.Challenge.Services.Security.API.Application.Commands;
using N5.Challenge.Services.Security.API.Application.Validations;

namespace N5.Challenge.Services.Security.API.Tests.Application.Validations
{
    public class RequestPermissionValidatorTest
    {
        private RequestPermissionValidator Validator {get;}

        public RequestPermissionValidatorTest()
        {
            Validator = new RequestPermissionValidator();
        }

        [Fact]
        public void Not_allow_empty_forename()
        {
            var command = new RequestPermissionCommand(1, string.Empty, "forename");

            Assert.False(Validator.Validate(command).IsValid);
        }

        [Fact]
        public void Not_has_length_requerid()
        {
            var command = new RequestPermissionCommand(1, "p", "forename");

            Assert.False(Validator.Validate(command).IsValid);
        }
    }

}