using AutoMapper;
using BeekeepingApi.DTOs.InvitationDTOs;

namespace BeekeepingApi.Profiles
{
    public class InvitationsProfile : Profile
    {
        public InvitationsProfile()
        {
            CreateMap<InvitationsProfile, InvitationReadDTO>();
        }
    }
}
