using Domain.Entities;
using Domain.Enum;
using Infrastructure.Repositories;

public class UserService : IUserService
{
    private readonly AuthUnitOfWork _unitOfWork;

    public UserService(AuthUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> UpdatePaymentStatusAsync(int userId, PaymentStatus paymentStatus)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.PaymentStatus = paymentStatus;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}