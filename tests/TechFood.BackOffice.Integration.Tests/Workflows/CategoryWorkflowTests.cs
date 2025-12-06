using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechFood.BackOffice.Application.Categories.Commands.CreateCategory;
using TechFood.BackOffice.Application.Categories.Commands.UpdateCategory;
using TechFood.BackOffice.Application.Categories.Commands.DeleteCategory;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Integration.Tests.Fixtures;

namespace TechFood.BackOffice.Integration.Tests.Workflows;

public class CategoryWorkflowTests : IClassFixture<IntegrationTestFixture>
{
  private readonly IntegrationTestFixture _fixture;
  private readonly IMediator _mediator;
  private readonly ICategoryRepository _categoryRepository;

  public CategoryWorkflowTests(IntegrationTestFixture fixture)
  {
    _fixture = fixture;
    _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
    _categoryRepository = _fixture.ServiceProvider.GetRequiredService<ICategoryRepository>();
  }

  [Fact(DisplayName = "Should complete full category workflow from creation to deletion")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task CompleteCategoryWorkflow_ShouldCreateUpdateAndDeleteCategory()
  {
    // Act & Assert - Create Category
    var createCommand = new CreateCategoryCommand(
        "Lanches",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    categoryDto.Should().NotBeNull();
    categoryDto.Name.Should().Be("Lanches");
    categoryDto.ImageUrl.Should().NotBeNullOrEmpty();

    // Get category from repository
    var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    category.Should().NotBeNull();
    category!.Name.Should().Be("Lanches");

    // Act & Assert - Update Category
    var updateCommand = new UpdateCategoryCommand(
        categoryDto.Id,
        "Lanches Especiais",
        null,
        null);

    var updatedDto = await _mediator.Send(updateCommand);
    await _fixture.DbContext.SaveChangesAsync();

    updatedDto.Name.Should().Be("Lanches Especiais");

    // Verify update in repository
    var updatedCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    updatedCategory!.Name.Should().Be("Lanches Especiais");

    // Act & Assert - Delete Category
    var deleteCommand = new DeleteCategoryCommand(categoryDto.Id);
    await _mediator.Send(deleteCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var deletedCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    deletedCategory.Should().BeNull();
  }

  [Fact(DisplayName = "Should create multiple categories independently")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task CreateMultipleCategories_ShouldMaintainIndependence()
  {
    // Act - Create first category
    var command1 = new CreateCategoryCommand(
        "Bebidas",
        CreateMockImageStream(),
        "image/jpeg");

    var category1 = await _mediator.Send(command1);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Create second category
    var command2 = new CreateCategoryCommand(
        "Sobremesas",
        CreateMockImageStream(),
        "image/jpeg");

    var category2 = await _mediator.Send(command2);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Create third category
    var command3 = new CreateCategoryCommand(
        "Acompanhamentos",
        CreateMockImageStream(),
        "image/jpeg");

    var category3 = await _mediator.Send(command3);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    category1.Should().NotBeNull();
    category2.Should().NotBeNull();
    category3.Should().NotBeNull();

    category1.Id.Should().NotBe(category2.Id);
    category2.Id.Should().NotBe(category3.Id);
    category1.Id.Should().NotBe(category3.Id);

    category1.Name.Should().Be("Bebidas");
    category2.Name.Should().Be("Sobremesas");
    category3.Name.Should().Be("Acompanhamentos");
  }

  [Fact(DisplayName = "Should update category with new image")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task UpdateCategory_WithNewImage_ShouldUpdateSuccessfully()
  {
    // Arrange - Create Category
    var createCommand = new CreateCategoryCommand(
        "Pizzas",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var originalImageFileName = (await _categoryRepository.GetByIdAsync(categoryDto.Id))!.ImageFileName;

    // Act - Update with new image
    var updateCommand = new UpdateCategoryCommand(
        categoryDto.Id,
        "Pizzas Artesanais",
        CreateMockImageStream(),
        "image/png");

    var updatedDto = await _mediator.Send(updateCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    updatedDto.Name.Should().Be("Pizzas Artesanais");

    var updatedCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    updatedCategory!.ImageFileName.Should().NotBe(originalImageFileName);
    updatedCategory.Name.Should().Be("Pizzas Artesanais");
  }

  [Fact(DisplayName = "Should update category without changing image")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task UpdateCategory_WithoutNewImage_ShouldKeepOriginalImage()
  {
    // Arrange - Create Category
    var createCommand = new CreateCategoryCommand(
        "Cafés",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var originalImageFileName = (await _categoryRepository.GetByIdAsync(categoryDto.Id))!.ImageFileName;

    // Act - Update without new image
    var updateCommand = new UpdateCategoryCommand(
        categoryDto.Id,
        "Cafés Especiais",
        null,
        null);

    var updatedDto = await _mediator.Send(updateCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    updatedDto.Name.Should().Be("Cafés Especiais");

    var updatedCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    updatedCategory!.ImageFileName.Should().Be(originalImageFileName);
    updatedCategory.Name.Should().Be("Cafés Especiais");
  }

  [Fact(DisplayName = "Should delete category successfully")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task DeleteCategory_ShouldRemoveFromDatabase()
  {
    // Arrange - Create Category
    var createCommand = new CreateCategoryCommand(
        "Saladas",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Verify category exists
    var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    existingCategory.Should().NotBeNull();

    // Act - Delete Category
    var deleteCommand = new DeleteCategoryCommand(categoryDto.Id);
    await _mediator.Send(deleteCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert - Category no longer exists
    var deletedCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
    deletedCategory.Should().BeNull();
  }

  [Fact(DisplayName = "Should create categories with different image types")]
  [Trait("Integration", "CategoryWorkflow")]
  public async Task CreateCategories_WithDifferentImageTypes_ShouldSucceed()
  {
    // Act - Create category with JPEG
    var jpegCommand = new CreateCategoryCommand(
        "Category JPEG",
        CreateMockImageStream(),
        "image/jpeg");

    var jpegCategory = await _mediator.Send(jpegCommand);

    // Act - Create category with PNG
    var pngCommand = new CreateCategoryCommand(
        "Category PNG",
        CreateMockImageStream(),
        "image/png");

    var pngCategory = await _mediator.Send(pngCommand);

    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    jpegCategory.Should().NotBeNull();
    pngCategory.Should().NotBeNull();
    jpegCategory.Id.Should().NotBe(pngCategory.Id);
  }

  private static Stream CreateMockImageStream()
  {
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write("fake image content");
    writer.Flush();
    stream.Position = 0;
    return stream;
  }
}
