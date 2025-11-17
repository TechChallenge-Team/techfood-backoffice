using TechFood.Shared.Domain.Exceptions;
using TechFood.Doman.Tests.Fixtures;

namespace TechFood.Doman.Tests
{
    public class CategoryTests : IClassFixture<CategoryFixture>
    {
        private readonly CategoryFixture _categoryFixture;
        public CategoryTests(CategoryFixture categoryFixture)
        { _categoryFixture = categoryFixture; }

        [Fact(DisplayName = "Validate Category Name Is Empty")]
        [Trait("Category", "Category Name Is Empty")]
        public void ShoudThrowException_WhenCategoryNameIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(_categoryFixture.CreateCategoryNameIsEmpty);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Category_ThrowNameIsEmpty, result.Message);
        }
        [Fact(DisplayName = "Validate Category Name Is Empty")]
        [Trait("Category", "Category Name Is Empty")]
        public void ShoudThrowException_WhenCategoryFileImageIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(_categoryFixture.CreateCategoryFileImageIsEmpty);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Category_ThrowFileImageIsEmpty, result.Message);
        }
        [Fact(DisplayName = "Validate Category Category Index Is Less Than Zero")]
        [Trait("Category", "Category Index Is Less ThanZero")]
        public void ShoudThrowException_WhenCategoryIndexIsLessThanZero()
        {
            // Act
            var result = Assert.Throws<DomainException>(_categoryFixture.CreateCategoryIndexIsLessThanZero);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Category_ThrowIndexIsLessThanZero, result.Message);
        }
    }
}
