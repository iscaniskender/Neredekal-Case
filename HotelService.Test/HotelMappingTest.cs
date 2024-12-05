using AutoMapper;
using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.ContactInfo.Command;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;
using HotelService.Application.Mapping;
using HotelService.Data.Entity;
using HotelService.Data.Enum;

namespace HotelService.Test
{
    public class HotelMappingTest
    {
        private readonly IMapper _mapper;

        public HotelMappingTest()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HotelMapping>(); // Profilinizi burada ekliyoruz.
            });

            _mapper = configuration.CreateMapper();
        }

        // Test: HotelEntity -> HotelDto
        [Fact]
        public void Should_Map_HotelEntity_To_HotelDto()
        {
            // Arrange
            var hotelEntity = new HotelEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Hotel",
                // Diğer özellikler de burada yer alabilir...
            };

            // Act
            var hotelDto = _mapper.Map<HotelDto>(hotelEntity);

            // Assert
            Assert.Equal(hotelEntity.Id, hotelDto.Id);
            Assert.Equal(hotelEntity.Name, hotelDto.Name);
        }

        // Test: HotelDto -> HotelEntity
        [Fact]
        public void Should_Map_HotelDto_To_HotelEntity()
        {
            // Arrange
            var hotelDto = new HotelDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Hotel",
                // Diğer özellikler de burada yer alabilir...
            };

            // Act
            var hotelEntity = _mapper.Map<HotelEntity>(hotelDto);

            // Assert
            Assert.Equal(hotelDto.Id, hotelEntity.Id);
            Assert.Equal(hotelDto.Name, hotelEntity.Name);
        }

        // Test: CreateHotelCommand -> HotelEntity
        [Fact]
        public void Should_Map_CreateHotelCommand_To_HotelEntity()
        {
            // Arrange
            var createHotelCommand = new CreateHotelCommand
            {
                Name = "New Hotel",
                // Diğer özellikler...
            };

            // Act
            var hotelEntity = _mapper.Map<HotelEntity>(createHotelCommand);

            // Assert
            Assert.Equal(createHotelCommand.Name, hotelEntity.Name);
        }

        // Test: UpdateHotelCommand -> HotelEntity
        [Fact]
        public void Should_Map_UpdateHotelCommand_To_HotelEntity()
        {
            // Arrange
            var updateHotelCommand = new UpdateHotelCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Hotel",
                // Diğer özellikler...
            };

            // Act
            var hotelEntity = _mapper.Map<HotelEntity>(updateHotelCommand);

            // Assert
            Assert.Equal(updateHotelCommand.Id, hotelEntity.Id);
            Assert.Equal(updateHotelCommand.Name, hotelEntity.Name);
        }

        // Test: CreateAuthorizedPersonCommand -> AuthorizedPersonEntity
        [Fact]
        public void Should_Map_CreateAuthorizedPersonCommand_To_AuthorizedPersonEntity()
        {
            // Arrange
            var createAuthorizedPersonCommand = new CreateAuthorizedPersonCommand
            {
                FirstName = "John",
                LastName = "Doe",
                // Diğer özellikler...
            };

            // Act
            var authorizedPersonEntity = _mapper.Map<AuthorizedPersonEntity>(createAuthorizedPersonCommand);

            // Assert
            Assert.Equal(createAuthorizedPersonCommand.FirstName, authorizedPersonEntity.FirstName);
            Assert.Equal(createAuthorizedPersonCommand.LastName, authorizedPersonEntity.LastName);
        }

        // Test: CreateContactInfoCommand -> ContactInfoEntity
        [Fact]
        public void Should_Map_CreateContactInfoCommand_To_ContactInfoEntity()
        {
            // Arrange
            var createContactInfoCommand = new CreateContactInfoCommand
            {
                Type = "Email",
                Content = "contact@hotel.com"
            };

            // Act
            var contactInfoEntity = _mapper.Map<ContactInfoEntity>(createContactInfoCommand);

            // Assert
            Assert.Equal(createContactInfoCommand.Type, contactInfoEntity.Type.ToString());
            Assert.Equal(createContactInfoCommand.Content, contactInfoEntity.Content);
        }

        // Test: ContactInfoDto -> ContactInfoEntity (Enum mapping included)
        [Fact]
        public void Should_Map_ContactInfoDto_To_ContactInfoEntity_WithEnum()
        {
            // Arrange
            var contactInfoDto = new ContactInfoDto
            {
                Type = "Email", // Enum'dan önce string olarak gelir
                Content = "contact@hotel.com"
            };

            // Act
            var contactInfoEntity = _mapper.Map<ContactInfoEntity>(contactInfoDto);

            // Assert
            Assert.Equal(Enum.Parse<ContactType>(contactInfoDto.Type), contactInfoEntity.Type);
            Assert.Equal(contactInfoDto.Content, contactInfoEntity.Content);
        }

        // Test: ContactInfoEntity -> ContactInfoDto (Enum mapping included)
        [Fact]
        public void Should_Map_ContactInfoEntity_To_ContactInfoDto_WithEnum()
        {
            // Arrange
            var contactInfoEntity = new ContactInfoEntity
            {
                Type = ContactType.Email,
                Content = "contact@hotel.com"
            };

            // Act
            var contactInfoDto = _mapper.Map<ContactInfoDto>(contactInfoEntity);

            // Assert
            Assert.Equal(contactInfoEntity.Type.ToString(), contactInfoDto.Type);
            Assert.Equal(contactInfoEntity.Content, contactInfoDto.Content);
        }

        // Test: AuthorizedPersonDto -> AuthorizedPersonEntity
        [Fact]
        public void Should_Map_AuthorizedPersonDto_To_AuthorizedPersonEntity()
        {
            // Arrange
            var authorizedPersonDto = new AuthorizedPersonDto
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var authorizedPersonEntity = _mapper.Map<AuthorizedPersonEntity>(authorizedPersonDto);

            // Assert
            Assert.Equal(authorizedPersonDto.FirstName, authorizedPersonEntity.FirstName);
            Assert.Equal(authorizedPersonDto.LastName, authorizedPersonEntity.LastName);
        }
    }
}
