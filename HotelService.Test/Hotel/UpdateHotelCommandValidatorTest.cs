using FluentValidation.TestHelper;
using HotelService.Application.Hotel.Command;
using HotelService.Application.Hotel.Validator;
using HotelService.Application.Dto;
using Xunit;
using System;
using System.Collections.Generic;

namespace HotelService.Test.Hotel
{
    public class UpdateHotelCommandValidatorTest
    {
        private readonly UpdateHotelCommandValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Id_IsEmpty()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.Empty,
                Name = "Valid Hotel Name",
                ContactInfos = new List<ContactInfoDto> { new ContactInfoDto { Type = "Email", Content = "contact@hotel.com" } },
                AuthorizedPersons = new List<AuthorizedPersonDto> { new AuthorizedPersonDto { FirstName = "John", LastName = "Doe" } }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Hotel Id cannot be an empty GUID.");
        }

        [Fact]
        public void Should_HaveError_When_Id_IsNotProvided()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.Empty,
                Name = "Hotel Name",
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Hotel Id is required.");
        }

        [Fact]
        public void Should_HaveError_When_Name_IsEmpty()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Hotel name is required.");
        }

        [Fact]
        public void Should_HaveError_When_Name_IsTooLong()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = new string('A', 201),
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Hotel name must not exceed 200 characters.");
        }

        [Fact]
        public void Should_HaveError_When_ContactInfos_IsEmpty()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Hotel Name",
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactInfos)
                .WithErrorMessage("At least one contact info is required.");
        }

        [Fact]
        public void Should_HaveError_When_ContactInfos_AreInvalid()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Hotel Name",
                ContactInfos = new List<ContactInfoDto> { new ContactInfoDto { Type = string.Empty, Content = string.Empty } },
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactInfos)
                .WithErrorMessage("Invalid contact info.");
        }

        [Fact]
        public void Should_HaveError_When_AuthorizedPersons_IsEmpty()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Hotel Name",
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AuthorizedPersons)
                .WithErrorMessage("At least one authorized person is required.");
        }

        [Fact]
        public void Should_HaveError_When_AuthorizedPersons_AreInvalid()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Hotel Name",
                ContactInfos = new List<ContactInfoDto>(),
                AuthorizedPersons = new List<AuthorizedPersonDto> { new AuthorizedPersonDto { FirstName = string.Empty, LastName = string.Empty } }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AuthorizedPersons)
                .WithErrorMessage("Invalid authorized person.");
        }

        [Fact]
        public void Should_NotHaveError_When_AllFieldsAreValid()
        {
            // Arrange
            var command = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Valid Hotel Name",
                ContactInfos = new List<ContactInfoDto> { new ContactInfoDto { Type = "Email", Content = "contact@hotel.com" } },
                AuthorizedPersons = new List<AuthorizedPersonDto> { new AuthorizedPersonDto { FirstName = "John", LastName = "Doe" } }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.ContactInfos);
            result.ShouldNotHaveValidationErrorFor(x => x.AuthorizedPersons);
        }
    }
}
