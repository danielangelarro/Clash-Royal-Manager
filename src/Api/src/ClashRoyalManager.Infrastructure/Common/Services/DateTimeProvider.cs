using ClashRoyalManager.Application.Common.Interfaces.Services;

namespace ClashRoyalManager.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}