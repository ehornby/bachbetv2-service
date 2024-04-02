using BachBetV2.Application.Exceptions;
using BachBetV2.Application.Interfaces;
using BachBetV2.Application.Services;
using FluentAssertions;
using Moq;

namespace BachBetV2.Application.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private const string PASSWORD = "testpass";

        public UserServiceTests()
        {
            _mockUserRepository = new();
            _sut = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void UserService_DoesNotThrow_WhenPasswordsMatch()
        {
            _mockUserRepository.Setup(x => x.RetrievePasswordAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(PASSWORD);

            var act = async () => await _sut.ValidateLogin("testuser", PASSWORD);

            act.Should().NotThrowAsync();
        }

        [Fact]
        public void UserService_ThrowsException_WhenPasswordDoesNotMatch()
        {
            _mockUserRepository.Setup(x => x.RetrievePasswordAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("fakepassword");

            var act = async () => await _sut.ValidateLogin("testuser", PASSWORD);

            act.Should().ThrowAsync<BachBetAuthException>();
        }

        [Fact]
        public void UserService_ThrowsException_WhenEmptyPassword()
        {
            _mockUserRepository.Setup(x => x.RetrievePasswordAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("");

            var act = async () => await _sut.ValidateLogin("testuser", PASSWORD);

            act.Should().ThrowAsync<BachBetAuthException>();
        }
    }
}