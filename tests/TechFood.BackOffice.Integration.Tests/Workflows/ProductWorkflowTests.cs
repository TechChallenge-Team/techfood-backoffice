using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechFood.BackOffice.Application.Products.Commands.CreateProduct;
using TechFood.BackOffice.Application.Products.Commands.UpdateProduct;
using TechFood.BackOffice.Application.Products.Commands.DeleteProduct;
using TechFood.BackOffice.Application.Products.Commands.SetProductOutOfStock;
using TechFood.BackOffice.Application.Categories.Commands.CreateCategory;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Integration.Tests.Fixtures;

namespace TechFood.BackOffice.Integration.Tests.Workflows;

public class ProductWorkflowTests : IClassFixture<IntegrationTestFixture>
{
  private readonly IntegrationTestFixture _fixture;
  private readonly IMediator _mediator;
  private readonly IProductRepository _productRepository;
  private readonly ICategoryRepository _categoryRepository;

  public ProductWorkflowTests(IntegrationTestFixture fixture)
  {
    _fixture = fixture;
    _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
    _productRepository = _fixture.ServiceProvider.GetRequiredService<IProductRepository>();
    _categoryRepository = _fixture.ServiceProvider.GetRequiredService<ICategoryRepository>();
  }

  [Fact(DisplayName = "Should complete full product workflow from creation to deletion")]
  [Trait("Integration", "ProductWorkflow")]
  public async Task CompleteProductWorkflow_ShouldCreateUpdateAndDeleteProduct()
  {
    // Arrange - Create Category first
    var categoryCommand = new CreateCategoryCommand(
        "Lanches",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(categoryCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Act & Assert - Create Product
    var createCommand = new CreateProductCommand(
        "X-Burger",
        "Hambúrguer com queijo",
        categoryDto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        25.00m);

    var productDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    productDto.Should().NotBeNull();
    productDto.Name.Should().Be("X-Burger");
    productDto.Description.Should().Be("Hambúrguer com queijo");
    productDto.Price.Should().Be(25.00m);
    productDto.CategoryId.Should().Be(categoryDto.Id);
    productDto.OutOfStock.Should().BeFalse();

    // Get product from repository
    var product = await _productRepository.GetByIdAsync(productDto.Id);
    product.Should().NotBeNull();
    product!.Name.Should().Be("X-Burger");

    // Act & Assert - Update Product
    var updateCommand = new UpdateProductCommand(
        productDto.Id,
        "X-Burger Premium",
        "Hambúrguer premium com queijo especial",
        categoryDto.Id,
        30.00m,
        null,
        null);

    var updatedDto = await _mediator.Send(updateCommand);
    await _fixture.DbContext.SaveChangesAsync();

    updatedDto.Name.Should().Be("X-Burger Premium");
    updatedDto.Description.Should().Be("Hambúrguer premium com queijo especial");
    updatedDto.Price.Should().Be(30.00m);

    // Act & Assert - Set Out of Stock
    var outOfStockCommand = new SetProductOutOfStockCommand(productDto.Id, true);
    await _mediator.Send(outOfStockCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var productAfterOutOfStock = await _productRepository.GetByIdAsync(productDto.Id);
    productAfterOutOfStock!.OutOfStock.Should().BeTrue();

    // Act & Assert - Delete Product
    var deleteCommand = new DeleteProductCommand(productDto.Id);
    await _mediator.Send(deleteCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var deletedProduct = await _productRepository.GetByIdAsync(productDto.Id);
    deletedProduct.Should().BeNull();
  }

  [Fact(DisplayName = "Should create multiple products for the same category")]
  [Trait("Integration", "ProductWorkflow")]
  public async Task CreateMultipleProducts_ShouldMaintainIndependence()
  {
    // Arrange - Create Category
    var categoryCommand = new CreateCategoryCommand(
        "Bebidas",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(categoryCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Create first product
    var command1 = new CreateProductCommand(
        "Coca-Cola",
        "Refrigerante 350ml",
        categoryDto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        5.00m);

    var product1 = await _mediator.Send(command1);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Create second product
    var command2 = new CreateProductCommand(
        "Guaraná",
        "Refrigerante 350ml",
        categoryDto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        5.00m);

    var product2 = await _mediator.Send(command2);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    product1.Should().NotBeNull();
    product2.Should().NotBeNull();
    product1.Id.Should().NotBe(product2.Id);
    product1.CategoryId.Should().Be(categoryDto.Id);
    product2.CategoryId.Should().Be(categoryDto.Id);
    product1.Name.Should().Be("Coca-Cola");
    product2.Name.Should().Be("Guaraná");
  }

  [Fact(DisplayName = "Should set product out of stock and back in stock")]
  [Trait("Integration", "ProductWorkflow")]
  public async Task SetProductOutOfStock_ShouldToggleStockStatus()
  {
    // Arrange - Create Category and Product
    var categoryCommand = new CreateCategoryCommand(
        "Sobremesas",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(categoryCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var productCommand = new CreateProductCommand(
        "Pudim",
        "Pudim de leite condensado",
        categoryDto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        8.00m);

    var productDto = await _mediator.Send(productCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Set out of stock
    var outOfStockCommand = new SetProductOutOfStockCommand(productDto.Id, true);
    await _mediator.Send(outOfStockCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert - Product is out of stock
    var product = await _productRepository.GetByIdAsync(productDto.Id);
    product!.OutOfStock.Should().BeTrue();

    // Act - Set back in stock
    var inStockCommand = new SetProductOutOfStockCommand(productDto.Id, false);
    await _mediator.Send(inStockCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert - Product is in stock
    product = await _productRepository.GetByIdAsync(productDto.Id);
    product!.OutOfStock.Should().BeFalse();
  }

  [Fact(DisplayName = "Should update product with new image")]
  [Trait("Integration", "ProductWorkflow")]
  public async Task UpdateProduct_WithNewImage_ShouldUpdateSuccessfully()
  {
    // Arrange - Create Category and Product
    var categoryCommand = new CreateCategoryCommand(
        "Pizzas",
        CreateMockImageStream(),
        "image/jpeg");

    var categoryDto = await _mediator.Send(categoryCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var createCommand = new CreateProductCommand(
        "Pizza Margherita",
        "Pizza com molho de tomate e mussarela",
        categoryDto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        35.00m);

    var productDto = await _mediator.Send(createCommand);
    await _fixture.DbContext.SaveChangesAsync();

    var originalImageFileName = (await _productRepository.GetByIdAsync(productDto.Id))!.ImageFileName;

    // Act - Update with new image
    var updateCommand = new UpdateProductCommand(
        productDto.Id,
        "Pizza Margherita Especial",
        "Pizza especial com molho de tomate e mussarela",
        categoryDto.Id,
        40.00m,
        CreateMockImageStream(),
        "image/png");

    var updatedDto = await _mediator.Send(updateCommand);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    updatedDto.Name.Should().Be("Pizza Margherita Especial");
    updatedDto.Price.Should().Be(40.00m);

    var updatedProduct = await _productRepository.GetByIdAsync(productDto.Id);
    updatedProduct!.ImageFileName.Should().NotBe(originalImageFileName);
  }

  [Fact(DisplayName = "Should create products in different categories")]
  [Trait("Integration", "ProductWorkflow")]
  public async Task CreateProducts_InDifferentCategories_ShouldSucceed()
  {
    // Arrange - Create two categories
    var category1Command = new CreateCategoryCommand(
        "Lanches",
        CreateMockImageStream(),
        "image/jpeg");

    var category1Dto = await _mediator.Send(category1Command);

    var category2Command = new CreateCategoryCommand(
        "Acompanhamentos",
        CreateMockImageStream(),
        "image/jpeg");

    var category2Dto = await _mediator.Send(category2Command);
    await _fixture.DbContext.SaveChangesAsync();

    // Act - Create products in different categories
    var product1Command = new CreateProductCommand(
        "X-Bacon",
        "Hambúrguer com bacon",
        category1Dto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        28.00m);

    var product1Dto = await _mediator.Send(product1Command);

    var product2Command = new CreateProductCommand(
        "Batata Frita",
        "Porção de batatas fritas",
        category2Dto.Id,
        CreateMockImageStream(),
        "image/jpeg",
        12.00m);

    var product2Dto = await _mediator.Send(product2Command);
    await _fixture.DbContext.SaveChangesAsync();

    // Assert
    product1Dto.CategoryId.Should().Be(category1Dto.Id);
    product2Dto.CategoryId.Should().Be(category2Dto.Id);

    var product1 = await _productRepository.GetByIdAsync(product1Dto.Id);
    var product2 = await _productRepository.GetByIdAsync(product2Dto.Id);

    product1!.CategoryId.Should().Be(category1Dto.Id);
    product2!.CategoryId.Should().Be(category2Dto.Id);
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
