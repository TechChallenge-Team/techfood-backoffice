using TechFood.BackOffice.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace TechFood.BackOffice.Domain.Tests.StepDefinitions;

[Binding]
public class ProductStepDefinitions
{
    private string _productName = string.Empty;
    private string _productDescription = string.Empty;
    private Guid _categoryId;
    private string _imageFileName = string.Empty;
    private decimal _price;
    private Product? _product;
    private Product? _product1;
    private Product? _product2;
    private Exception? _caughtException;

    [Given(@"que eu tenho um nome de produto ""(.*)""")]
    public void GivenIHaveAProductName(string name)
    {
        _productName = name;
    }

    [Given(@"eu tenho uma descrição de produto ""(.*)""")]
    public void GivenIHaveAProductDescription(string description)
    {
        _productDescription = description;
    }

    [Given(@"eu tenho um ID de categoria válido")]
    public void GivenIHaveAValidCategoryId()
    {
        _categoryId = Guid.NewGuid();
    }

    [Given(@"eu tenho um ID de categoria vazio")]
    public void GivenIHaveAnEmptyCategoryId()
    {
        _categoryId = Guid.Empty;
    }

    [Given(@"eu tenho um nome de arquivo de imagem de produto ""(.*)""")]
    public void GivenIHaveAProductImageFileName(string imageFileName)
    {
        _imageFileName = imageFileName;
    }

    [Given(@"eu tenho um preço de (.*)")]
    public void GivenIHaveAPrice(decimal price)
    {
        _price = price;
    }

    [When(@"eu criar o produto")]
    public void WhenICreateTheProduct()
    {
        _product = new Product(_productName, _productDescription, _categoryId, _imageFileName, _price);
    }

    [When(@"eu tentar criar o produto")]
    public void WhenITryToCreateTheProduct()
    {
        try
        {
            _product = new Product(_productName, _productDescription, _categoryId, _imageFileName, _price);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [Then(@"o produto deve ser criado com sucesso")]
    public void ThenTheProductShouldBeCreatedSuccessfully()
    {
        _product.Should().NotBeNull();
    }

    [Then(@"o nome do produto deve ser ""(.*)""")]
    public void ThenTheProductNameShouldBe(string expectedName)
    {
        _product.Should().NotBeNull();
        _product!.Name.Should().Be(expectedName);
    }

    [Then(@"a descrição do produto deve ser ""(.*)""")]
    public void ThenTheProductDescriptionShouldBe(string expectedDescription)
    {
        _product.Should().NotBeNull();
        _product!.Description.Should().Be(expectedDescription);
    }

    [Then(@"o ID da categoria do produto não deve ser vazio")]
    public void ThenTheProductCategoryIdShouldNotBeEmpty()
    {
        _product.Should().NotBeNull();
        _product!.CategoryId.Should().NotBe(Guid.Empty);
    }

    [Then(@"o nome do arquivo de imagem do produto deve ser ""(.*)""")]
    public void ThenTheProductImageFileNameShouldBe(string expectedImageFileName)
    {
        _product.Should().NotBeNull();
        _product!.ImageFileName.Should().Be(expectedImageFileName);
    }

    [Then(@"o preço do produto deve ser (.*)")]
    public void ThenTheProductPriceShouldBe(decimal expectedPrice)
    {
        _product.Should().NotBeNull();
        _product!.Price.Should().Be(expectedPrice);
    }

    [Then(@"o produto deve estar em estoque")]
    public void ThenTheProductShouldBeInStock()
    {
        _product.Should().NotBeNull();
        _product!.OutOfStock.Should().BeFalse();
    }

    [Given(@"que eu criei um produto chamado ""(.*)""")]
    public void GivenICreatedAProductNamed(string name)
    {
        _product1 = new Product(name, "Description 1", Guid.NewGuid(), "image1.png", 19.99m);
    }

    [Given(@"eu criei outro produto chamado ""(.*)""")]
    public void GivenICreatedAnotherProductNamed(string name)
    {
        _product2 = new Product(name, "Description 2", Guid.NewGuid(), "image2.png", 29.99m);
    }

    [Then(@"cada produto deve ter um ID único")]
    public void ThenEachProductShouldHaveAUniqueId()
    {
        _product1.Should().NotBeNull();
        _product2.Should().NotBeNull();
        _product1!.Id.Should().NotBe(_product2!.Id);
    }

    [Then(@"os IDs dos produtos não devem ser vazios")]
    public void ThenTheProductIdsShouldNotBeEmpty()
    {
        _product1.Should().NotBeNull();
        _product2.Should().NotBeNull();
        _product1!.Id.Should().NotBe(Guid.Empty);
        _product2!.Id.Should().NotBe(Guid.Empty);
    }

    [Given(@"que eu tenho um produto válido em estoque")]
    public void GivenIHaveAValidProductInStock()
    {
        _product = new Product("Test Product", "Description", Guid.NewGuid(), "image.png", 19.99m);
    }

    [When(@"eu marcar o produto como fora de estoque")]
    public void WhenIMarkTheProductAsOutOfStock()
    {
        _product!.SetOutOfStock(true);
    }

    [Then(@"o produto deve estar fora de estoque")]
    public void ThenTheProductShouldBeOutOfStock()
    {
        _product.Should().NotBeNull();
        _product!.OutOfStock.Should().BeTrue();
    }

    [Given(@"que eu tenho um produto válido fora de estoque")]
    public void GivenIHaveAValidProductOutOfStock()
    {
        _product = new Product("Test Product", "Description", Guid.NewGuid(), "image.png", 19.99m);
        _product.SetOutOfStock(true);
    }

    [When(@"eu marcar o produto como em estoque")]
    public void WhenIMarkTheProductAsInStock()
    {
        _product!.SetOutOfStock(false);
    }

    [Given(@"que eu tenho um nome de produto com (.*) caracteres")]
    public void GivenIHaveAProductNameWithCharacters(int characterCount)
    {
        _productName = new string('A', characterCount);
    }

    [Then(@"o nome do produto deve ter (.*) caracteres")]
    public void ThenTheProductNameShouldHaveCharacters(int expectedCount)
    {
        _product.Should().NotBeNull();
        _product!.Name.Length.Should().Be(expectedCount);
    }
}
