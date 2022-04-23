using AutoMapper;
using BeekeepingApi.DTOs.InvitationDTOs;
using BeekeepingApi.Models;

namespace BeekeepingApi.Profiles
{
    public class InvitationsProfile : Profile
    {
        public InvitationsProfile()
        {
            CreateMap<Invitation, InvitationReadDTO>();
        }
    }
}
