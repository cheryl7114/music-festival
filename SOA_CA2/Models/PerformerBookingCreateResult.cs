namespace SOA_CA2.Models;

public enum PerformerBookingCreateResult
{
    Created,
    FestivalNotFound,
    PerformerNotFound,
    OutsideFestivalDates,
    OverlappingBooking
}
