using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.Models;

public class OrderDto
{
    public string Id { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public List<OrderItemDto> Items { get; set; } = new();
    
    public decimal Subtotal { get; set; }
    
    public decimal Tax { get; set; }
    
    public decimal ShippingCost { get; set; }
    
    public decimal Total { get; set; }
    
    public string Status { get; set; } = string.Empty;
    
    public AddressDto ShippingAddress { get; set; } = new();
    
    public AddressDto BillingAddress { get; set; } = new();
    
    public PaymentInfoDto PaymentInfo { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? ShippedAt { get; set; }
    
    public DateTime? DeliveredAt { get; set; }
}

public class OrderItemDto
{
    public string ProductId { get; set; } = string.Empty;
    
    public string ProductName { get; set; } = string.Empty;
    
    public decimal UnitPrice { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal TotalPrice { get; set; }
}

public class AddressDto
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string Street { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;
    
    public string State { get; set; } = string.Empty;
    
    public string ZipCode { get; set; } = string.Empty;
    
    public string Country { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
}

public class PaymentInfoDto
{
    public string PaymentMethod { get; set; } = string.Empty;
    
    public string TransactionId { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public DateTime? ProcessedAt { get; set; }
}

public class CreateOrderRequest
{
    [Required]
    public List<CreateOrderItemRequest> Items { get; set; } = new();
    
    [Required]
    public AddressDto ShippingAddress { get; set; } = new();
    
    [Required]
    public AddressDto BillingAddress { get; set; } = new();
    
    public string? CouponCode { get; set; }
}

public class CreateOrderItemRequest
{
    [Required]
    public string ProductId { get; set; } = string.Empty;
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class UpdateOrderStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
} 