using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using TechFood.Domain.Entities;

namespace TechFood.Doman.Tests.Fixtures
{
    public class CategoryFixture
    {
        private readonly Faker _faker;

        public CategoryFixture()
        {
            _faker = new Faker("pt_BR");
        }
        private readonly string[] _categoryName = { "Sobremesa", "Acompanhamento", "Lanche", "Bebida" };
        private readonly string[] _categoryImageFileName = { "sobremesa.jpg", "acompanhamento.png", "lanche.jpg", "bebida.jpg" };
        public Category CreateCategoryNameIsEmpty()
            => new(string.Empty,
                _faker.PickRandom(_categoryImageFileName),
                _faker.Random.Number(0, 3));
        public Category CreateCategoryFileImageIsEmpty()
           => new(_faker.PickRandom(_categoryName),
               string.Empty,
               _faker.Random.Number(0, 3));
        public Category CreateCategoryIndexIsLessThanZero()
           => new(_faker.PickRandom(_categoryName),
               _faker.PickRandom(_categoryImageFileName),
               _faker.Random.Number(-4, -1));
    }
}
