namespace SOA_CA2.Models;

public enum PerformerBookingSaveResult
{
    Saved,
    BookingNotFound,
    FestivalNotFound,
    PerformerNotFound,
    OutsideFestivalDates,
    OverlappingBooking
}
