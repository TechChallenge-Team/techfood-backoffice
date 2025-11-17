using TechFood.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;
using TechFood.Doman.Tests.Fixtures;

namespace TechFood.Doman.Tests
{
    public class OrderTests :
        IClassFixture<CustomerFixture>,
        IClassFixture<OrderFixture>
    {
        private readonly OrderFixture _orderFixture;
        private readonly CustomerFixture _customerFixture;

        public OrderTests(
            OrderFixture orderFixture,
            CustomerFixture customerFixture)
        {
            _orderFixture = orderFixture;
            _customerFixture = customerFixture;
        }

        [Fact(DisplayName = "Cannot add item to an order that is not in the Pending status.")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenAddingItemToOrderThatIsNotPendingStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            var item = new OrderItem(
                productId: Guid.NewGuid(),
                quantity: 1,
                unitPrice: 10);

            // Act
            var result = Assert.Throws<DomainException>(() => order.AddItem(item));

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotAddItemToNonPendingStatus, result.Message);
        }

        [Fact(DisplayName = "Cannot remove item to an order that is not in the Pending status.")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenRemovingItemToOrderThatIsNotPendingStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            var item = new OrderItem(
                productId: Guid.NewGuid(),
                quantity: 1,
                unitPrice: 10);

            order.AddItem(item);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(() => order.RemoveItem(item.Id));

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotRemoveItemToNonPendingStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Receive when Order is Pending")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenReceiveToOrderThatIsNotPendingStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(order.Receive);

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotReceiveToNonPendingStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Discount Application when Order is Pending")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenApplyingDiscountToOrderThatIsNotPendingStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(() => order.ApplyDiscount(10));

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotApplyDiscountToNonPendingStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Prepare when Order is Received")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenPrepareOrderThatIsNotReceivedStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(order.Receive);

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotReceiveToNonPendingStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Ready when Order is InPreparation")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenReadyOrderThatIsNotInPreparationStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(order.Prepare);

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotPrepareToNonReceivedStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Deliver when Order is Ready")]
        [Trait("Order", "Order Status")]
        public void ShoudThrowException_WhenDeliverOrderThatIsNotReadyStatus()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            order.Cancel();

            // Act
            var result = Assert.Throws<DomainException>(order.Deliver);

            // Assert
            Assert.Equal(Domain.Resources.Exceptions.Order_CannotDeliverToNonReadyStatus, result.Message);
        }

        [Fact(DisplayName = "Validate Amount Calculation in Order")]
        [Trait("Order", "Calculation")]
        public void ShoudThrowException_WhenCalculatingAmountIsNotCorrect()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            var item = new OrderItem(
                productId: Guid.NewGuid(),
                quantity: 7,
                unitPrice: 10.42m);

            order.AddItem(item);
            order.AddItem(item);
            order.AddItem(item);
            order.AddItem(item);

            // Act

            // Assert
            Assert.Equal(7 * 10.42m * 4, order.Amount);
        }

        [Fact(DisplayName = "Validate Discount Calculation in Order")]
        [Trait("Order", "Calculation")]
        public void ShoudThrowException_WhenCalculatingDiscountIsNotCorrect()
        {
            // Arrange
            var customer = _customerFixture.CreateValidCustomer();
            var order = _orderFixture.CreateValidOrder(customer.Id);

            var item = new OrderItem(
                productId: Guid.NewGuid(),
                quantity: 7,
                unitPrice: 10.42m);

            order.AddItem(item);
            order.ApplyDiscount(9.76m);

            // Act

            // Assert
            Assert.Equal((7 * 10.42m) - 9.76m, order.Amount);
        }
    }
}
