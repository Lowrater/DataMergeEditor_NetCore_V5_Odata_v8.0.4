using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using OdataApi_V5.Contexts;
using OdataApi_V5.Models;
using OdataApi_V5.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdataApi_V5.Controllers
{
    public class UsersController : ODataController
    {
        private UsersContext db;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="context"></param>
        public UsersController(UsersContext context)
        {
            db = context;
        }


        #region Get Methods

        /// <summary>
        /// Get's user
        /// http://localhost:5000/odata/Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            // Fetches an list of all users from the DB
            var dbResponseData = db.Users;

            // Null check
            if (await dbResponseData.AnyAsync())
            {
                return Ok(dbResponseData);
            }
            else
            {
                return NotFound();
            }

        }


        ///// <summary>
        ///// Get's username based on username parameter
        /////  http://localhost:5000/odata/Users(MTS)
        ///// </summary>
        ///// <param name="username"></param>
        ///// <returns></returns>
        //[HttpGet("odata/users({username})")]
        //public async Task<IActionResult> Get([FromODataUri] string username)
        //{
        //    if (!string.IsNullOrEmpty(username))
        //    {
        //        Users dbResponseData = await db.Users.AsQueryable().FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());

        //        if (dbResponseData != null)
        //        {
        //            return Ok(dbResponseData);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}


        /// <summary>
        /// Get's username based on userid parameter
        /// http://localhost:5000/odata/Users(2)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("odata/users({userId})")]
        public async Task<IActionResult> GetById([FromODataUri] int userId)
        {
            Users dbResponseData = await db.Users.AsQueryable().FirstOrDefaultAsync(user => user.UserId == userId);

            return Ok(dbResponseData);
        }


        #endregion

        #region POST Methods
        /// <summary>
        /// Creates an user
        /// http://localhost:5000/odata/Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        //[EnableQuery]
        public async Task<IActionResult> Post([FromBody] Users user)
        {
            // Null check
            if (user == null)
                return NotFound();

            // Adds the user
            await db.AddAsync(user);

            // Set's the state
            db.Entry(user).State = EntityState.Added;

            // Saves the changes to the database
            await db.SaveChangesAsync();

            // Returns the user we just created
            return Created(user);
        }
        #endregion
    }
}
