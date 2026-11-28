namespace SOA_CA2.DTOs;

public record FestivalSimulationDto(
    int FestivalId,
    decimal TicketPrice,
    int ExpectedTicketsSold,
    decimal ProjectedRevenue,
    decimal TotalBookingFees,
    decimal TotalExpenses,
    decimal TotalCosts,
    decimal ProjectedProfitOrLoss);
