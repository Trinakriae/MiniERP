using FluentAssertions;

using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Mappers;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Tests.Products.Mappers
{
    public class CategoryMapperTests
    {
        private readonly CategoryMapper _mapper;

        public CategoryMapperTests()
        {
            _mapper = new CategoryMapper();
        }

        [Fact]
        public void Map_ShouldThrowArgumentException_WhenCategoryDtoIsNull()
        {
            // Act
            Action act = () => _mapper.Map(null as CategoryDto);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("CategoryDto cannot be null");
        }

        [Fact]
        public void Map_ShouldThrowArgumentException_WhenCategoryIsNull()
        {
            // Act
            Action act = () => _mapper.Map(null as Category);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Category cannot be null");
        }

        [Fact]
        public void Map_ShouldMapCategoryDtoToCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Category1"
            };

            // Act
            var category = _mapper.Map(categoryDto);

            // Assert
            category.Should().NotBeNull();
            category.Id.Should().Be(categoryDto.Id);
            category.Name.Should().Be(categoryDto.Name);
        }

        [Fact]
        public void Map_ShouldMapCategoryToCategoryDto()
        {
            // Arrange
            var category = new Category
            {
                Id = 1,
                Name = "Category1"
            };

            // Act
            var categoryDto = _mapper.Map(category);

            // Assert
            categoryDto.Should().NotBeNull();
            categoryDto.Id.Should().Be(category.Id);
            categoryDto.Name.Should().Be(category.Name);
        }
    }
}
