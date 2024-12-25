

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ITableRepository TableRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IBookingRepository BookingRepository { get; }

        IBookingHeaderRepository BookingHeaderRepository { get; }
        IBookingDetailRepository BookingDetailRepository { get; }
        void Save();
    }
}
