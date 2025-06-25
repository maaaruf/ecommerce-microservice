namespace Shared.Contracts.Events;

public abstract class IntegrationEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    
    public string EventType { get; set; } = string.Empty;
    
    public string Source { get; set; } = string.Empty;
    
    public string CorrelationId { get; set; } = string.Empty;
}

public class OrderCreatedEvent : IntegrationEvent
{
    public string OrderId { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public decimal TotalAmount { get; set; }
    
    public List<OrderItemEvent> Items { get; set; } = new();
}

public class OrderItemEvent
{
    public string ProductId { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
}

public class PaymentProcessedEvent : IntegrationEvent
{
    public string OrderId { get; set; } = string.Empty;
    
    public string PaymentId { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public string TransactionId { get; set; } = string.Empty;
}

public class InventoryUpdatedEvent : IntegrationEvent
{
    public string ProductId { get; set; } = string.Empty;
    
    public int QuantityChange { get; set; }
    
    public int NewStockLevel { get; set; }
    
    public string Reason { get; set; } = string.Empty;
}

public class UserRegisteredEvent : IntegrationEvent
{
    public string UserId { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
} 