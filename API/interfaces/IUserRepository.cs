using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetAppUserByIdAsync(int id);

        Task<AppUser> GetAppUserByUserNameAsync(string userName);

        Task<MemberDto> GetMemberAsync(string userName);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    }
}