

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using Restaurant.Services;
using RestaurantViewModels;
using FluentAssertions;
using System.Linq.Expressions;


public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        _categoryService = new CategoryService(_mockUnitOfWork.Object, _mockWebHostEnvironment.Object);
    }

    [Fact]
    public async Task CreateCategory_WithValidData_ShouldCreateCategory()
    {
        // Arrange
        var categoryVM = new CategoryVM
        {
            Name = "Test Category",
            DisplayOrder = 1
        };

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("test.jpg");
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

        _mockWebHostEnvironment.Setup(w => w.WebRootPath).Returns("wwwroot");
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
         It.IsAny<Expression<Func<Category, bool>>>(),
         null, 
         false 
     )).Returns((Category)null);

        // Act
        await _categoryService.CreateCategory(categoryVM, mockFile.Object);

        // Assert
        _mockUnitOfWork.Verify(u => u.CategoryRepository.Add(It.IsAny<Category>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCategory_WithExistingCategory_ShouldThrowException()
    {
        // Arrange
        var categoryVM = new CategoryVM
        {
            Name = "Existing Category",
            DisplayOrder = 1
        };

        var existingCategory = new Category { Id = 1, Name = "Existing Category" };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
        It.IsAny<Expression<Func<Category, bool>>>(),
        null, 
        false 
    )).Returns(existingCategory);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _categoryService.CreateCategory(categoryVM, null));
    }
    [Fact]
    public void DeleteCategory_WithValidId_ShouldDeleteCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category", ImageUrl = "\\images\\category\\test.jpg" };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
            It.IsAny<Expression<Func<Category, bool>>>(),
            null, 
            false 
        )).Returns(category);

        _mockWebHostEnvironment.Setup(w => w.WebRootPath).Returns("wwwroot");

        // Act
        var result = _categoryService.DeleteCategory(category);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockUnitOfWork.Verify(u => u.CategoryRepository.Remove(It.IsAny<Category>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public void DeleteCategory_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var category = new Category { Id = 999 };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
            It.IsAny<Expression<Func<Category, bool>>>(),
            null, 
            false 
        )).Returns((Category)null);

        // Act & Assert
        Assert.Throws<Exception>(() => _categoryService.DeleteCategory(category));
    }
    [Fact]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Category 1", DisplayOrder = 1 },
        new Category { Id = 2, Name = "Category 2", DisplayOrder = 2 }
    };

        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetAll(
           It.IsAny<Expression<Func<Category, bool>>>(),
           null
         
        )).Returns(categories.AsQueryable);

        // Act
        var result = _categoryService.GetAll().Result;

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "Category 1");
        result.Should().Contain(c => c.Name == "Category 2");
    }

    [Fact]
    public void GetById_WithValidId_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
            It.IsAny<Expression<Func<Category, bool>>>(),
            null, 
            false 
        )).Returns(category);

        // Act
        var result = _categoryService.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Category");
    }

    [Fact]
    public void GetById_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
            It.IsAny<Expression<Func<Category, bool>>>(),
            null, 
            false 
        )).Returns((Category)null);

        // Act
        var result = _categoryService.GetById(999);

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public void Update_WithValidData_ShouldUpdateCategory()
    {
        // Arrange
        var categoryVM = new CategoryVM
        {
            Id = 1,
            Name = "Updated Category",
            DisplayOrder = 2,
            ImageUrl = "\\images\\category\\updated.jpg"
        };

        var existingCategory = new Category { Id = 1, Name = "Old Category", DisplayOrder = 1 };
        _mockUnitOfWork.Setup(u => u.CategoryRepository.GetFirstOrDefault(
            It.IsAny<Expression<Func<Category, bool>>>(),
            null, 
            false 
        )).Returns(existingCategory);

        // Act
        _categoryService.Update(categoryVM, null);

        // Assert
        existingCategory.Name.Should().Be("Updated Category");
        existingCategory.DisplayOrder.Should().Be(2);
        existingCategory.ImageUrl.Should().Be("\\images\\category\\updated.jpg");
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}