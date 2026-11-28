using SOA_CA2.DTOs;
using SOA_CA2.Services;
using Xunit;

namespace SOA_CA2.Tests;

public class FestivalSimulationCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsProfitAndCostBreakdown()
    {
        var result = FestivalSimulationCalculator.Calculate(7, 30.00m, 500, 250.25m, 100.10m);

        Assert.Equal(
            new FestivalSimulationDto(7, 30.00m, 500, 15000m, 250.25m, 100.10m, 350.35m, 14649.65m),
            result);
    }

    [Fact]
    public void Calculate_ReturnsLossWhenNoTicketsSell()
    {
        var result = FestivalSimulationCalculator.Calculate(7, 30.00m, 0, 250.25m, 100.10m);

        Assert.Equal(0m, result.ProjectedRevenue);
        Assert.Equal(-350.35m, result.ProjectedProfitOrLoss);
    }

    [Fact]
    public void Calculate_ReturnsZeroProfitAtBreakEven()
    {
        var result = FestivalSimulationCalculator.Calculate(7, 30.00m, 10, 250m, 50m);

        Assert.Equal(0m, result.ProjectedProfitOrLoss);
    }
}
