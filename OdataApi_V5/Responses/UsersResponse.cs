using OdataApi_V5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OdataApi_V5.Responses
{
    public class UsersResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"><see cref="Users"/></param>
        public UsersResponse(Users user)
        {
            Userid = user.UserId;
            Username = user.Username;
            Created = user.Created;
            Closed = user.Closed;
        }

        [Key]
        public int Userid { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
    }
}
