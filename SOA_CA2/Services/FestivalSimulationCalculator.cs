using SOA_CA2.DTOs;

namespace SOA_CA2.Services;

public static class FestivalSimulationCalculator
{
    public static FestivalSimulationDto Calculate(
        int festivalId, decimal ticketPrice, int expectedTicketsSold, decimal bookingFees, decimal expenses)
    {
        var projectedRevenue = ticketPrice * expectedTicketsSold;
        var totalCosts = bookingFees + expenses;

        return new FestivalSimulationDto(festivalId, ticketPrice, expectedTicketsSold, projectedRevenue,
            bookingFees, expenses, totalCosts, projectedRevenue - totalCosts);
    }
}
