
using Auth.API.Application.Users.GetUserByGuid;
using Auth.API.Application.Users.Models;

using Auth.API.Domain;
using Auth.API.Domain.Contracts;
using Auth.API.Domain.Enums;
using Auth.API.Domain.Errors;
using Bogus;
using FluentAssertions;
using Moq;

namespace AmarEServir.UnitTests.Application.Users
{
    [TestFixture]
    public class GetUserByIdHandlerTests
    {

        private Mock<IUserRepository>? _mockUserRepository;
        private GetUserByGuidQueryHandler? _handler;
        private Faker<User>? _requestFaker;
        private static readonly Guid UserId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _handler = new GetUserByGuidQueryHandler(_mockUserRepository.Object);

            _requestFaker = new Faker<User>("pt-BR")
                .CustomInstantiator(f => new User(
                    f.Name.FullName(),
                    f.Internet.Email(),
                    f.Phone.PhoneNumber(),
                    f.Internet.Password(8),
                    new Address(
                        f.Address.StreetAddress(),
                        f.Address.SecondaryAddress(),
                        f.Address.BuildingNumber(),
                        f.Address.City(),
                        f.Address.City(),
                        f.Address.State(),
                        f.Address.Country(),
                        f.Address.ZipCode()
                    ),
                    UserRole.Admin
                ));
        }

        [Test]
        public async Task Handle_WhenUserDoesNotExist_ShouldReturnNotFound()
        {

            _mockUserRepository?.Setup(repo => repo.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            var request = new GetUserByGuidQuery(UserId);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.First().Code.Should().Be(UserErrors.Account.NotFound.Code);
            _mockUserRepository.Verify(repo => repo.GetUserByGuid(UserId), Times.Once);

        }

        [Test]
        public async Task Handle_WhenUserExist_ShouldReturnUser()
        {

            var user = _requestFaker.Generate();

            _mockUserRepository?.Setup(repo => repo.GetUserByGuid(It.IsAny<Guid>())).ReturnsAsync(user);

            var request = new GetUserByGuidQuery(UserId);

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();

            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<UserResponse>();

            result.Value.Name.Should().Be(user.Name);

            _mockUserRepository.Verify(repo => repo.GetUserByGuid(UserId), Times.Once);

        }
    }
}
