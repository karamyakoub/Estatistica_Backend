using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.DataAccessLayer.Entities;

namespace Estatistica.WebAPI.ServiceContracts
{
    public interface ICurrentUserService
    {
        //Task<Usuario?> GetCurrentUser();
        string? GetCurrentUserId();
    }
}