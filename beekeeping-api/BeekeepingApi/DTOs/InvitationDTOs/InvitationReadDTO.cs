﻿using System;

namespace BeekeepingApi.DTOs.InvitationDTOs
{
    public class InvitationReadDTO
    {
        public Guid Code { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
