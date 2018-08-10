using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.GroupUser
{
    public class GetUsersOutsideConnectedChatsRequest : GroupRequest
    {
        [FromQuery(Name = "chatUsersCount")]
        [Range(1, int.MaxValue)]
        public int ChatUsersCount { get; set; }   
    }
}