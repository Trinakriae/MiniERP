using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Mappers
{
    public class CategoryMapper : IMapper<Category, CategoryDto>
    {
        public Category Map(CategoryDto categoryDto)
        {
            _ = categoryDto ?? throw new ArgumentException("CategoryDto cannot be null");

            return new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name
            };
        }

        public CategoryDto Map(Category category)
        {
            _ = category ?? throw new ArgumentException("Category cannot be null");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
