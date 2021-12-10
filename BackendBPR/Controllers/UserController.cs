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
    /// <summary>
    /// Controller responsible for users
    /// </summary>
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly OrangeBushContext _dbContext;
        /// <summary>
        /// Constructor for instantiating the controller
        /// </summary>
        /// <param name="logger">The logger to use</param>
        /// <param name="db">The database context to query</param>
        public UserController(ILogger<AuthController> logger, OrangeBushContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        /// <summary>
        /// Gets the current user by the token provided
        /// </summary>
        /// <param name="_token">The token to match the user to</param>
        /// <returns>The user whose token matches the one provided</returns>
        [HttpGet]
        [Route("/profile")]
        public ObjectResult GetCurrentUser([FromHeader] string _token)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");

            User user = new User();
            try
            {
                string trueToken = _token.Split('=')[1];
                user = _dbContext.Users.FirstOrDefault(user => user.Token == trueToken);
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
        public ObjectResult EditProfile([FromHeader] User user)
        {
            if(!ControllerUtilities.TokenVerification(user.Token, _dbContext))
                return Unauthorized("User/token mismatch");

            if(!user.Image.Equals(null))
            {
                if(!ControllerUtilities.isImage(user.Image, 5242880))
                    return BadRequest("This isn't an image");
            }

            int trueId = Convert.ToInt32(user.Token.Split('=')[1]);
            _dbContext.Users.Update(_dbContext.Users.FirstOrDefault(oldUser => oldUser.Id == trueId));
            _dbContext.SaveChanges();
            return Ok("Profile updated successfully");
        }

        /// <summary>
        /// Upload an image by passing it through a user object
        /// </summary>
        /// <param name="_token">The user token to whom the image should be attached - matched by token</param>
        /// <param name="_image">The image represented in base64 string</param>
        /// <returns>Whether or not the image was uploaded</returns>
        [HttpPost]
        [Route("/profile")]
        public ObjectResult UploadImage([FromHeader] string _token, [FromBody] string _image)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            byte[] image = Convert.FromBase64String(_image);
            if(!ControllerUtilities.isImage(image, 5242880))
                return BadRequest("This isn't an image");

            try
            {
                user.Image = image;
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return Ok("Image was uploaded");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }

        }

        /// <summary>
        /// Deletes the user profile that corresponds to the token
        /// </summary>
        /// <param name="_token">The token to which to match the user</param>
        /// <returns>Whether or not the user profile was deleted</returns>
        [HttpDelete]
        [Route("/profile")]
        public ObjectResult DeleteProfile([FromHeader] string _token)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            try
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                return Ok("Profile deleted successfully");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }

        }

        /// <summary>
        /// Gets all the notes that correspond to the provided token
        /// </summary>
        /// <param name="_token">The token to which to match the user</param>
        /// <returns>The notes that the matched user has</returns>
        [HttpGet]
        [Route("/profile/notes")]
        public ObjectResult GetNotes([FromHeader] string _token)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            List<Note> notes =  new List<Note>();
            try
            {
                notes = (List<Note>) _dbContext.Notes.Where(notes => notes.UserId == user.Id);
                return Ok(notes);
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Gets a certain note that corresponds to the parsed user token and the note id
        /// </summary>
        /// <param name="_token">The user token to match to</param>
        /// <param name="_id">The note id to match to</param>
        /// <returns>The note that was requested</returns>
        [HttpGet]
        [Route("/profile/note")]
        public ObjectResult GetNote([FromHeader] string _token, int _id)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            Note note = new Note();
            try
            {
                note = _dbContext.Notes.FirstOrDefault(note => note.Id == _id && note.UserId == user.Id);
                return Ok(note);
            } 
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }

        }

        /// <summary>
        /// Gets certain notes that correspond to the parsed user token and the plantID
        /// </summary>
        /// <param name="_token">The user token to match to</param>
        /// <param name="_plantId">The note id to match to</param>
        /// <returns>The note that was requested</returns>
        [HttpGet]
        [Route("/profile/notesbyplant")]
        public ObjectResult GetNotesByPlant([FromHeader] string _token, int _plantId)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            List<Note> notes = new List<Note>();
            try
            {
                notes = (List<Note>) _dbContext.Notes.Where(note => note.PlantId == _plantId && note.UserId == user.Id);
                return Ok(notes);
            } 
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }

        }
        
        /// <summary>
        /// Adds a note to the corresponding user that matches the token
        /// </summary>
        /// <param name="_token">The users' token to add to</param>
        /// <param name="_note">The note to add</param>
        /// <returns>Whether or not the note was added</returns>
        [HttpPost]
        [Route("/profile/note")]
        public ObjectResult AddNote([FromHeader] String _token, [FromBody] Note _note)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            string trueID = _token.Split('=')[0];
            try
            {
                _note.UserId = Convert.ToInt32(trueID);
                _dbContext.Notes.Add(_note);
                _dbContext.SaveChanges();
                return Ok("The note has been added successfully");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }
        }
        /// <summary>
        /// Updates a note by the corresponding parsed user token and the note id
        /// </summary>
        /// <param name="_token">The user token to update the note to</param>
        /// <param name="_note">The note to update</param>
        /// <returns>Whether or not the note was updated</returns>
        [HttpPut]
        [Route("/profile/note")]
        public ObjectResult EditNote([FromHeader] string _token, [FromBody] Note _note)
        {
            if(!ControllerUtilities.TokenVerification(_token, _dbContext))
                return Unauthorized("User/token mismatch");

            string trueID = _token.Split('=')[0];
            try
            {
                _note.UserId = Convert.ToInt32(trueID);
                _dbContext.Notes.Update(_note);
                _dbContext.SaveChanges();
                return Ok("The note has been edited successfully");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Deletes a note that corresponds to the parsed user token and note id
        /// </summary>
        /// <param name="_token">The user token to match to</param>
        /// <param name="_id">The note id to match to</param>
        /// <returns>Whether or not the note was deleted</returns>
        [HttpDelete]
        [Route("/profile/note")]
        public ObjectResult DeleteNote([FromHeader] string _token, int _id)
        {
            User user;
            bool isVerified;
            ControllerUtilities.TokenVerification(_token, _dbContext, out user, out isVerified);
            if(!isVerified)
                return Unauthorized("User/token mismatch");

            try
            {
                _dbContext.Notes.Remove(_dbContext.Notes.FirstOrDefault(note => note.Id == _id && note.UserId == user.Id));
                _dbContext.SaveChanges();
                return Ok("The note has been deleted successfully");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong");
            }
        }
    }
    
}