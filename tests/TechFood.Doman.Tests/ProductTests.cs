using TechFood.Shared.Domain.Exceptions;
using TechFood.Doman.Tests.Fixtures;

namespace TechFood.Doman.Tests
{
    public class ProductTests : IClassFixture<ProductFixture>
    {
        private readonly ProductFixture _productFixture;
        public ProductTests(ProductFixture productFixture)
        { _productFixture = productFixture; }

        [Fact(DisplayName = "Validate Product Name Is Empty")]
        [Trait("Product", "Product Name Is Empty")]
        public void ShoudThrowException_WhenProductNameIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(_productFixture.CreateProductNameIsEmpty);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Product_ThrowNameIsEmpty, result.Message);
        }
        [Fact(DisplayName = "Validate Product Description Is Empty")]
        [Trait("Product", "Product Description Is Empty")]
        public void ShoudThrowException_WhenProductDescriptionIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(_productFixture.CreateProductDescriptionIsEmpty);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Product_ThrowDescriptionIsEmpty, result.Message);
        }
        [Fact(DisplayName = "Validate Product Category Id Invalid")]
        [Trait("Product", "Product Category Id Invalid")]
        public void ShoudThrowException_WhenProductCategoryIdInvalid()
        {
            // Act
            var result = Assert.Throws<DomainException>(_productFixture.CreateProductCategoryIdInvalid);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Product_ThrowCategoryIdInvalid, result.Message);
        }
        [Fact(DisplayName = "Validate Product Image File Is Empty")]
        [Trait("Product", "Product Image File Is Empty")]
        public void ShoudThrowException_WhenProductImageFileIsEmpty()
        {
            // Act
            var result = Assert.Throws<DomainException>(_productFixture.CreateProductImageFileIsEmpty);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Product_ThrowCategoryImageFileIsEmpty, result.Message);
        }
        [Fact(DisplayName = "Validate Product Price Is Greater Than Zero")]
        [Trait("Product", "Product Price Is Greater Than Zero")]
        public void ShoudThrowException_WhenProductPriceIsGreaterThanZero()
        {
            // Act
            var result = Assert.Throws<DomainException>(_productFixture.CreateProductPriceIsGreaterThanZero);
            //// Assert
            Assert.Equal(Domain.Resources.Exceptions.Product_ThrowPriceIsGreaterThanZero, result.Message);
        }
    }
}
