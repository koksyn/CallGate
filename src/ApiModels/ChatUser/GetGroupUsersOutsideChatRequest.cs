﻿using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.ChatUser
{
    public class GetGroupUsersOutsideChatRequest : ChatRequest
    {
        [FromQuery(Name = "username")]
        [StringLength(255)]
        public string Username { get; set; }
    }
}