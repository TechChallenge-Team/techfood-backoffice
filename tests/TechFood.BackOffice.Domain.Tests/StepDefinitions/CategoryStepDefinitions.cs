using TechFood.BackOffice.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;
using TechTalk.SpecFlow;

namespace TechFood.BackOffice.Domain.Tests.StepDefinitions;

[Binding]
public class CategoryStepDefinitions
{
    private string _categoryName = string.Empty;
    private string _imageFileName = string.Empty;
    private int _sortOrder;
    private Category? _category;
    private Category? _category1;
    private Category? _category2;
    private Exception? _caughtException;

    [Given(@"que eu tenho um nome de categoria ""(.*)""")]
    public void GivenIHaveACategoryName(string nome)
    {
        _categoryName = nome;
    }

    [Given(@"que eu tenho um nome de categoria vazio")]
    public void GivenIHaveAnEmptyCategoryName()
    {
        _categoryName = string.Empty;
    }

    [Given(@"que eu tenho um nome de categoria com (.*) caracteres")]
    public void GivenIHaveACategoryNameWithCharacters(int quantidade)
    {
        _categoryName = new string('A', quantidade);
    }

    [Given(@"eu tenho um nome de arquivo de imagem ""(.*)""")]
    public void GivenIHaveAnImageFileName(string arquivo)
    {
        _imageFileName = arquivo;
    }

    [Given(@"eu tenho uma ordem de classificação de (.*)")]
    public void GivenIHaveASortOrder(int ordem)
    {
        _sortOrder = ordem;
    }

    [When(@"eu criar a categoria")]
    public void WhenICreateTheCategory()
    {
        _category = new Category(_categoryName, _imageFileName, _sortOrder);
    }

    [When(@"eu tentar criar a categoria")]
    public void WhenITryToCreateTheCategory()
    {
        try
        {
            _category = new Category(_categoryName, _imageFileName, _sortOrder);
        }
        catch (Exception ex)
        {
            _caughtException = ex;
        }
    }

    [Then(@"a categoria deve ser criada com sucesso")]
    public void ThenTheCategoryShouldBeCreatedSuccessfully()
    {
        _category.Should().NotBeNull();
    }

    [Then(@"o nome da categoria deve ser ""(.*)""")]
    public void ThenTheCategoryNameShouldBe(string expectedName)
    {
        _category.Should().NotBeNull();
        _category!.Name.Should().Be(expectedName);
    }

    [Then(@"o nome do arquivo de imagem deve ser ""(.*)""")]
    public void ThenTheImageFileNameShouldBe(string expectedFileName)
    {
        _category.Should().NotBeNull();
        _category!.ImageFileName.Should().Be(expectedFileName);
    }

    [Then(@"a ordem de classificação deve ser (.*)")]
    public void ThenTheSortOrderShouldBe(int expectedSortOrder)
    {
        _category.Should().NotBeNull();
        _category!.SortOrder.Should().Be(expectedSortOrder);
    }

    [Then(@"uma exceção de domínio deve ser lançada")]
    public void ThenADomainExceptionShouldBeThrown()
    {
        _caughtException.Should().NotBeNull();
        _caughtException.Should().BeOfType<DomainException>();
    }

    [Given(@"que eu criei uma categoria chamada ""(.*)""")]
    public void GivenICreatedACategoryNamed(string nome)
    {
        _category1 = new Category(nome, "image1.png", 0);
    }

    [Given(@"eu criei outra categoria chamada ""(.*)""")]
    public void GivenICreatedAnotherCategoryNamed(string nome)
    {
        _category2 = new Category(nome, "image2.png", 0);
    }

    [Then(@"cada categoria deve ter um ID único")]
    public void ThenEachCategoryShouldHaveAUniqueId()
    {
        _category1.Should().NotBeNull();
        _category2.Should().NotBeNull();
        _category1!.Id.Should().NotBe(_category2!.Id);
    }

    [Then(@"os IDs não devem ser vazios")]
    public void ThenTheIdsShouldNotBeEmpty()
    {
        _category1.Should().NotBeNull();
        _category2.Should().NotBeNull();
        _category1!.Id.Should().NotBe(Guid.Empty);
        _category2!.Id.Should().NotBe(Guid.Empty);
    }

    [Then(@"o nome da categoria deve ter (.*) caracteres")]
    public void ThenTheCategoryNameShouldHaveCharacters(int quantidade)
    {
        _category.Should().NotBeNull();
        _category!.Name.Length.Should().Be(quantidade);
    }

    [Given(@"que eu criei uma categoria ""(.*)"" com ordem (.*)")]
    public void GivenICreatedACategoryWithSortOrder(string nome, int ordem)
    {
        if (_category1 == null)
        {
            _category1 = new Category(nome, "image1.png", ordem);
        }
        else
        {
            _category2 = new Category(nome, "image2.png", ordem);
        }
    }

    [Given(@"eu criei uma categoria ""(.*)"" com ordem (.*)")]
    public void GivenICreatedACategoryWithOrder(string nome, int ordem)
    {
        _category2 = new Category(nome, "image2.png", ordem);
    }

    [Then(@"ambas categorias devem ter a ordem de classificação (.*)")]
    public void ThenBothCategoriesShouldHaveTheSortOrder(int ordem)
    {
        _category1.Should().NotBeNull();
        _category2.Should().NotBeNull();
        _category1!.SortOrder.Should().Be(ordem);
        _category2!.SortOrder.Should().Be(ordem);
    }
}
