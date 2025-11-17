using Bogus;
using TechFood.Domain.Entities;

namespace TechFood.Doman.Tests.Fixtures
{
    public class ProductFixture
    {
        private readonly Faker _faker;
        public ProductFixture()
        {
            _faker = new Faker("pt_BR");
        }

        private readonly string[] _productName = { "coca", "X-Bacon", "Milk Shake de Baunilha", "Fanta", "Batata Frita" };
        private readonly string[] _productDescription = { "Coca-Cola", "Delicioso X-Bacon", "Milk Shake de Baunilha", "Fanta", "Batata Frita" };
        private readonly string[] _productImageFileName = { "coca-cola.png", "x-bacon.png", "milk-shake-baunilha.png", "milk-shake-chocolate.png", "fanta.png" };

        public Product CreateProductNameIsEmpty()
            => new(string.Empty,
                _faker.PickRandom(_productDescription),
                new Guid(),
                _faker.PickRandom(_productImageFileName),
                _faker.Random.Number(20, 40));

        public Product CreateProductDescriptionIsEmpty()
            => new(_faker.PickRandom(_productName),
           string.Empty, new Guid(),
                _faker.PickRandom(_productImageFileName),
                _faker.Random.Number(20, 40));

        public Product CreateProductCategoryIdInvalid()
            => new(_faker.PickRandom(_productName),
                _faker.PickRandom(_productDescription), new Guid(),
                _faker.PickRandom(_productImageFileName),
                _faker.Random.Number(20, 40));
        public Product CreateProductImageFileIsEmpty()
            => new(_faker.PickRandom(_productName),
                _faker.PickRandom(_productDescription),
                new Guid("C3A70938-9E88-437D-A801-C166D2716341"),
                string.Empty,
                _faker.Random.Number(20, 40));

        public Product CreateProductPriceIsGreaterThanZero()
            => new(_faker.PickRandom(_productName),
                _faker.PickRandom(_productDescription),
                new Guid("C3A70938-9E88-437D-A801-C166D2716341"),
                _faker.PickRandom(_productImageFileName),
                _faker.Random.Number(-40, -1));
    }
}
