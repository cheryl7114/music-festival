namespace SOA_CA2.DTOs;

public record PerformerBookingDto(
    int Id, int FestivalId, int PerformerId, DateTime StartsAtUtc, DateTime EndsAtUtc, decimal Fee);
