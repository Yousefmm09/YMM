using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Common;
using YMM.Application.Dto.Order;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDb _appDb;
        private readonly IEmailService _emailService;
        public OrderRepo(AppDb appDb, IEmailService emailService)
        {
            _appDb = appDb;
            _emailService = emailService;
        }
        public Task<ApiResponse<OrderDto>> CancelOrderAsync(string userId, int orderId, CancelOrderDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            var cart= await _appDb.Carts.AsNoTracking().Include(x=>x.Items).ThenInclude(x=>x.Product)
                .ThenInclude(x=>x.Variants).Include(x=>x.User).FirstOrDefaultAsync(x=>x.UserId == userId);
            if (cart == null || !cart.Items.Any())
            {
                return new ApiResponse<OrderDto>
                (
                    Success: false,
                    Message: "Cart is empty",
                    Data: null,
                    Errors: new[] { "Please add items to cart before checkout" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            var shippingAddress = await _appDb.Addresses.AsNoTracking().FirstOrDefaultAsync(x=>x.UserId==userId && dto.ShippingAddressId==x.Id);
            if (shippingAddress == null)
            {
                return new ApiResponse<OrderDto>
                (
                    Success: false,
                    Message: "Shipping address not found",
                    Data: null,
                    Errors: new[] { "Invalid shipping address" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            var billingAddress = await _appDb.Addresses
               .FirstOrDefaultAsync(a => a.Id == dto.BillingAddressId && a.UserId == userId);

            if (billingAddress == null)
            {
                return new ApiResponse<OrderDto>
                (
                    Success: false,
                    Message: "Billing address not found",
                    Data: null,
                    Errors: new[] { "Invalid billing address" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            var NumberOrder = await GeneratOrderNumber();
            var order = new Order
            {
                OrderNumber = NumberOrder,
                UserId = userId,
                ShippingAddressId = dto.ShippingAddressId,
                BillingAddressId = dto.BillingAddressId,
                PaymentMethod = dto.PaymentMethod,
                Status = "Pending",
                PaymentStatus = dto.PaymentMethod == "CashOnDelivery" ? "Pending" : "Pending",
                Subtotal = cart.Subtotal,
                Discount = cart.Discount,
                Tax = cart.Tax,
                ShippingCost =50, // noted :clac shippging cost
                Total = cart.Total + 50, // noted calc
                CouponId = cart.CouponId,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            await _appDb.Orders.AddAsync(order);
            await _appDb.SaveChangesAsync();
            var ordetItem = new List<OrderItem>();
            foreach ( var cartItem in cart.Items )
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    VariantId = cartItem.VariantId,
                    ProductName = cartItem.Product.Name,
                    Size = cartItem.Variant?.Size,
                    Color = cartItem.Variant?.Color,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Total = cartItem.Total,
                    CreatedAt = DateTime.UtcNow
                };
                ordetItem.Add( orderItem );
            }
            await _appDb.OrderItems.AddRangeAsync(ordetItem);
            foreach( var cartItem in cart.Items )
            {
                cartItem.Product.SoldCount += cartItem.Quantity;
                _appDb.Products.Update(cartItem.Product);
            }
            var statusHistory = new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = "Pending",
                Notes = "Order created",
                CreatedAt = DateTime.UtcNow
            };

            await _appDb.OrderStatusHistories.AddAsync(statusHistory);
            await _emailService.SendEmail(cart.User.Email, "Order Confirmation " +
                $"Your order {order.OrderNumber} has been placed successfully.");
            // 10. CLEAR CART
            _appDb.CartItems.RemoveRange(cart.Items);
            cart.Subtotal = 0;
            cart.Discount = 0;
            cart.Tax = 0;
            cart.Total = 0;
            cart.CouponId = null;
            _appDb.Carts.Update(cart);
           await _appDb.SaveChangesAsync();

            var orderDto = new OrderDto
            { 
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                Tax = order.Tax,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt
            };
            return new ApiResponse<OrderDto>
            (
             Success: true,
             Message: "Order created successfully",
             Data: orderDto,
             Errors: null,
             TraceId: Guid.NewGuid().ToString()
             );
        }
        private async Task<string> GeneratOrderNumber()
        {
            var year = DateTime.UtcNow.Year;
            var lastOrder = await _appDb.Orders.AsNoTracking()
                .Where(o => o.OrderNumber.StartsWith($"ORD-{year}-"))
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastOrder != null)
            {
                var parts = lastOrder.OrderNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"ORD-{year}-{nextNumber:D4}";
        }
        public Task<ApiResponse<PaginatedResponse<OrderDto>>> GetAllOrdersAsync(OrderFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDto> GetOrderDetailsAsync(string userId, int orderId)
        {
            var order =  await _appDb.Orders.AsNoTrackingWithIdentityResolution()
                .Include(o => o.BillingAddress)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            var orders = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                Tax = order.Tax,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                BillingAddress = order.BillingAddress != null ? new OrderAddressDto
                {

                    PhoneNumber = order.BillingAddress.PhoneNumber,
                    Street = order.BillingAddress.Street,
                    City = order.BillingAddress.City,
                    State = order.BillingAddress.State,
                    PostalCode = order.BillingAddress.PostalCode,
                } : null!
            };
            return orders;
        }

        public async Task<PaginatedResponse<OrderDto>> GetUserOrdersAsync(string userId, PaginationParams pagination,string status)
        {
            var query = _appDb.Orders.AsNoTrackingWithIdentityResolution().Include(x=>x.Shipment).Include(x=>x.ShippingAddress).Include(x=>x.BillingAddress).Include(x=>x.User);
            var PageSizeParametr=(pagination.Page-1)*pagination.PageSize;
            var totalCount=query.Count();
            var Orders = await query.Where(x => x.UserId == userId && x.Status == status)
                .OrderBy(x=>x.Id)
                .Skip(PageSizeParametr).Take(pagination.PageSize)
                .Select(order => new PaginatedResponse<OrderDto>
                {
                    Items = new List<OrderDto>
                    {
                        new OrderDto
                        {
                            Id = order.Id,
                            OrderNumber = order.OrderNumber,
                            PaymentMethod = order.PaymentMethod,
                            Status = order.Status,
                            PaymentStatus = order.PaymentStatus,
                            Subtotal = order.Subtotal,
                            Discount = order.Discount,
                            Tax = order.Tax,
                            ShippingCost = order.ShippingCost,
                            Total = order.Total,
                            Notes = order.Notes,
                            CreatedAt = order.CreatedAt,
                            BillingAddress = order.BillingAddress != null ? new OrderAddressDto
                            {
                                Id = order.BillingAddress.Id,
                                FullName = order.BillingAddress.FullName,
                                PhoneNumber = order.BillingAddress.PhoneNumber,
                                Street = order.BillingAddress.Street,
                                Building = order.BillingAddress.Building,
                                Apartment = order.BillingAddress.Apartment,
                                City = order.BillingAddress.City,
                                State = order.BillingAddress.State,
                                PostalCode = order.BillingAddress.PostalCode,
                                Country = order.BillingAddress.Country
                            } : null!,
                        }
                    },
                    PageSize = pagination.PageSize,
                    Page = pagination.Page,
                    TotalItems = totalCount,
                    HasNextPage = PageSizeParametr + pagination.PageSize < totalCount,
                    TotalPages = (int)((Math.Ceiling((double)totalCount / pagination.PageSize))),
                    HasPreviousPage = PageSizeParametr > 0,
                }).ToListAsync();

            return  new PaginatedResponse<OrderDto>
            {
                Items = Orders.SelectMany(x=>x.Items).ToList(),
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize),
                HasNextPage = PageSizeParametr + pagination.PageSize < totalCount,
                HasPreviousPage = PageSizeParametr > 0
            };
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto, string adminUserId)
        {
            var order=  await _appDb.Orders.Include(x=>x.Items)
                .ThenInclude(x=>x.Product)
                .Include(x=>x.Items)
                .ThenInclude(x=>x.Variant).
                Include(x=>x.ShippingAddress).
                Include(x=>x.User).FirstOrDefaultAsync(x => x.Id == orderId);
            var admin = await _appDb.Users.Where(x => x.Id == adminUserId).FirstOrDefaultAsync();
            if (order == null)
                if (order == null)
                {
                    return new ApiResponse<OrderDto>
                    (
                        Success: false,
                        Message: "Order not found",
                        Data: null,
                        Errors: new[] { "Order does not exist" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }
            if (order.Status == dto.Status)
            {
                return new ApiResponse<OrderDto>
                (
                    Success: false,
                    Message: "Invalid status transition",
                    Data: null,
                    Errors: new[] {
                    $"Cannot change status from '{order.Status}' to '{dto.Status}'"
                    },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            if(dto.Status=="Shipped" && string.IsNullOrWhiteSpace(dto.TrackingNumber))
                return new ApiResponse<OrderDto>
           (
               Success: false,
               Message: "Tracking number required",
               Data: null,
               Errors: new[] { "Tracking number is required when marking order as shipped" },
               TraceId: Guid.NewGuid().ToString()
           );
            var OldStatus = order.Status;
        
            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;

         
            switch (dto.Status)
            {
                case "Confirm":
                    await HandleConfirmedStatus(order);
                    break;

                //case "Processing":
                //    await HandleProcessingStatus(order);
                //    break;

                case "Shipped":
                    await HandleShippedStatus(order, dto.TrackingNumber);
                    break;

                case "Delivered":
                    await HandleDeliveredStatus(order);
                    break;

                case "Cancelled":
                    await HandleCancelledStatus(order, dto.Notes);
                    break;
            }
            var orderStatusHistory = new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = dto.Status,
                ChangedBy = admin,
                Notes = dto.Notes ?? $"Status changed from {OldStatus} to {dto.Status}",
                CreatedAt = DateTime.UtcNow,
            };
           await _appDb.OrderStatusHistories.AddAsync(orderStatusHistory);
             _appDb.Update(order);
            await _appDb.SaveChangesAsync();
            var orderDto =  await GetOrderDetailsAsync(order.Id);
            return new ApiResponse<OrderDto>
            (
             Success: true,
             Message: "Order status updated successfully",
             Data: orderDto,
             Errors: null,
             TraceId: Guid.NewGuid().ToString()
            );
        }
        private async Task HandleConfirmedStatus(Order order)
        {
            order.PaymentStatus = "Paid";
            await _emailService.SendEmail(
        order.User.Email,
        "Order Confirmed"+
        $"Your order {order.OrderNumber} has been confirmed!");
        }
        private async Task HandleShippedStatus(Order order,string TrackingNumber)
        {
            var Shipment = _appDb.Shipments.FirstOrDefault(x => x.Id == order.Id);
            if(Shipment==null)
            {
                var newShipment = new Shipment
                {
                    OrderId = order.Id,
                    TrackingNumber = TrackingNumber,
                    Carrier = "DHL", // أو من settings
                    Status = "InTransit",
                    ShippedAt = DateTime.UtcNow,
                    EstimatedDeliveryDate = DateTime.UtcNow.AddDays(3),
                    CreatedAt = DateTime.UtcNow
                };
                await _appDb.Shipments.AddAsync(newShipment);
                await _appDb.SaveChangesAsync();
                var tracking = new ShipmentTracking
                {
                    ShipmentId = newShipment.Id,
                    Status = "InTransit",
                    Location = "Warehouse",
                    Description = "Package has been picked up by courier",
                    CreatedAt = DateTime.UtcNow
                };
                await _appDb.ShipmentTrackings.AddAsync(tracking);
                await _appDb.SaveChangesAsync();
            }
            else
            {
                Shipment.TrackingNumber = TrackingNumber;
                Shipment.Status = "InTransit";
                Shipment.ShippedAt = DateTime.UtcNow;
                 _appDb.Shipments.Update(Shipment);
                await _appDb.SaveChangesAsync();
            }
           await _emailService.SendEmail(order.User.Email,
        "Order Shipped"+
        $@"Your order {order.OrderNumber} has been shipped!
           Tracking Number: {TrackingNumber}
           Track your order: https://ymmshoes.com/track/{TrackingNumber}");
        }
        private async Task HandleDeliveredStatus(Order order)
        {
           order.DeliveredAt = DateTime.UtcNow;
            var shipment = await _appDb.Shipments.FirstOrDefaultAsync(x => x.OrderId == order.Id);
            if (shipment != null)
            {
                shipment.ShippedAt = DateTime.UtcNow;
                shipment.Status = "Delivered";
                _appDb.Shipments.Update(shipment);
                var track = new ShipmentTracking
                {
                    ShipmentId = shipment.Id,
                    Status = "Delivered",
                    Location = order.ShippingAddress.City,
                    Description = "Package delivered to recipient",
                    CreatedAt = DateTime.UtcNow
                };
                await _appDb.ShipmentTrackings.AddAsync(track);
                await _appDb.SaveChangesAsync();
            }
            await _emailService.SendEmail(order.User.Email, "Order Delivered"+
        $@"Your order {order.OrderNumber} has been delivered!
           We hope you enjoy your purchase.
           Please leave a review: https://ymmshoes.com/orders/{order.Id}/review");

        }
        private async Task HandleCancelledStatus(Order order, string reason)
        {
            order.CancellationReason = reason;
            order.CancelledAt = DateTime.UtcNow;

           
            foreach (var item in order.Items)
            {
                if (item.Variant != null)
                {
                    item.Variant.StockQuantity += item.Quantity;
                    _appDb.ProductVariants.Update(item.Variant);
                }


                item.Product.SoldCount -= item.Quantity;
                _appDb.Products.Update(item.Product);
            }

            if (order.PaymentStatus == "Paid")
            {
                order.PaymentStatus = "Refunded";
            }

            await _emailService.SendEmail(
                order.User.Email+
                "Order Cancelled",
                $@"Your order {order.OrderNumber} has been cancelled.
           Reason: {reason}
           {(order.PaymentStatus == "Paid" ? "Refund will be processed within 3-5 business days." : "")}");
        }
        private async Task<OrderDto> GetOrderDetailsAsync(int orderId)
        {
            var order = await _appDb.Orders
                .Include(o => o.BillingAddress)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            var orders = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                Tax = order.Tax,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Notes = order.Notes,
                CreatedAt = order.CreatedAt,
                BillingAddress = order.BillingAddress != null ? new OrderAddressDto
                {

                    PhoneNumber = order.BillingAddress.PhoneNumber,
                    Street = order.BillingAddress.Street,
                    City = order.BillingAddress.City,
                    State = order.BillingAddress.State,
                    PostalCode = order.BillingAddress.PostalCode,
                } : null!
            };
            return orders;
        }
    }
}
