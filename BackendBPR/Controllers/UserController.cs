using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendBPR.Database;
using BackendBPR.Utils;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace BackendBPR.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;

        public UserController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// Gets the current user by the token provided
        /// </summary>
        /// <param name="token">The token to match the user to</param>
        /// <returns>The user whose token matches the one provided</returns>
        [HttpGet]
        [Route("/profile")]
        public ObjectResult GetCurrentUser([FromBody] string token)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            User user = new User();
            try
            {
                user = _dbContext.Users.FirstOrDefault(user => user.Token == token);
                return Ok(user);
            }
            catch(Exception)
            {
                return NotFound("How'd you spoof a token broski?");
            }
        }

        /// <summary>
        /// Updates the user profile with the provided changes by the token on the passed object
        /// </summary>
        /// <param name="user">The user and their changes to update</param>
        /// <returns>Whether or not the update went through</returns>
        [HttpPut]
        [Route("/profile")]
        public ObjectResult EditProfile([FromBody] User user)
        {
            if(!ControllerUtilities.TokenVerification(user.Token, _dbContext))
                return Unauthorized("User/token mismatch");

                _dbContext.Users.Update(_dbContext.Users.FirstOrDefault(oldUser => oldUser.Token == user.Token));
                _dbContext.SaveChanges();
                return Ok("Profile updated succesfully");
        }

        /// <summary>
        /// Upload an image by passing it through a user object
        /// </summary>
        /// <param name="user">The user to whom the image should be attached - matched by token</param>
        /// <returns>Whether or not the image was uploaded</returns>
        [HttpPost]
        [Route("/profile")]
        public ObjectResult UploadImage([FromBody] User user)
        {
            if(!ControllerUtilities.TokenVerification(user.Token, _dbContext))
                return Unauthorized("User/token mismatch");

            User currentUser = _dbContext.Users.FirstOrDefault(currentUser => currentUser.Token == user.Token);
            currentUser.Image = user.Image;
            _dbContext.Users.Update(currentUser);
            _dbContext.SaveChanges();
            return Ok("Image was uploaded");
        }

        /// <summary>
        /// Deletes the user profile that corresponds to the token
        /// </summary>
        /// <param name="token">The token to which to match the user</param>
        /// <returns>Whether or not the user profile was deleted</returns>
        [HttpDelete]
        [Route("/profile")]
        public ObjectResult DeleteProfile([FromBody] string token)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            _dbContext.Users.Remove(_dbContext.Users.FirstOrDefault(user => user.Token == token));
            _dbContext.SaveChanges();
            return Ok("Profile deleted succesfully");
        }

        /// <summary>
        /// Gets all the notes that correspond to the provided token
        /// </summary>
        /// <param name="token">The token to which to match the user</param>
        /// <returns>The notes that the matched user has</returns>
        [HttpGet]
        [Route("/profile/notes")]
        public ObjectResult GetNotes([FromBody] string token)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            //This LINQ makes sense but I feel like it's going to want me to use "Include" somewhere in it
            List<Note> notes =  new List<Note>();
            notes = (List<Note>) _dbContext.Notes.Where(notes => notes.UserId == _dbContext.Users
            .FirstOrDefault(user => user.Token == token).Id);
            return Ok(notes);
        }

        /// <summary>
        /// Gets a certain note that corresponds to the parsed user token and the note id
        /// </summary>
        /// <param name="token">The user token to match to</param>
        /// <param name="id">The note id to match to</param>
        /// <returns>The note that was requested</returns>
        [HttpGet]
        [Route("/profile/note")]
        public ObjectResult GetNote([FromBody] string token, [FromHeader] int id)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            Note note = new Note(); 
            note = _dbContext.Notes.FirstOrDefault(note => note.Id == id && note.UserId == _dbContext.Users
            .FirstOrDefault(user => user.Token == token).Id);
            return Ok(note);
        }

        /// <summary>
        /// Adds a note to the corresponding user that matches the token parsed in the object
        /// </summary>
        /// <param name="user">The user to add the note to</param>
        /// <returns>Whether or not the note was added</returns>
        [HttpPost]
        [Route("/profile/note")]
        public ObjectResult AddNote([FromBody] User user)
        {
            if(!ControllerUtilities.TokenVerification(user.Token, _dbContext))
                return Unauthorized("User/token mismatch");

            //This LINQ makes sense but I feel like it's going to want me to use "Include" somewhere in it
            _dbContext.Users.Include(currentUser => currentUser.Notes).FirstOrDefault(currentUser => currentUser.Token == user.Token).Notes.Add(user.Notes.First());
            _dbContext.SaveChanges();
            return Ok("The note has been added successfully");
        }

        /// <summary>
        /// Updates a note by the corresponding parsed user token and the note id
        /// </summary>
        /// <param name="user">The user to update the note to</param>
        /// <param name="id">The note id to update</param>
        /// <returns>Whether or not the note was updated</returns>
        [HttpPut]
        [Route("/profile/note")]
        public ObjectResult EditNote([FromBody] User user, [FromHeader] int id)
        {
            if(!ControllerUtilities.TokenVerification(user.Token, _dbContext))
                return Unauthorized("User/token mismatch");

            //This LINQ makes sense but I feel like it's going to want me to use "Include" somewhere in it
            _dbContext.Users.Include(currentUser => currentUser.Notes).FirstOrDefault(currentUser => currentUser.Token == user.Token).Notes.FirstOrDefault(note => note.Id == id)
            .Text = user.Notes.First().Text;
            _dbContext.SaveChanges();
            return Ok("The note has been edited succesfully");
        }

        /// <summary>
        /// Deletes a note that corresponds to the parsed user token and note id
        /// </summary>
        /// <param name="token">The user token to match to</param>
        /// <param name="id">The note id to match to</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/profile/note")]
        public ObjectResult DeleteNote([FromBody] string token, [FromHeader] int id)
        {
            if(!ControllerUtilities.TokenVerification(token, _dbContext))
                return Unauthorized("User/token mismatch");

            _dbContext.Notes.Remove(_dbContext.Notes.FirstOrDefault(note => note.Id == id && note.UserId == _dbContext.Users
            .FirstOrDefault(user => user.Token == token).Id));
            _dbContext.SaveChanges();
            return Ok("The note has been deleted succesfully");
        }
    }
    
}