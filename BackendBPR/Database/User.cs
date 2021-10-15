using System;
using System.Collections.Generic;

namespace BackendBPR.Database
{
    /// <summary>
    /// Class for representing a user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user Id
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int Id { get; set; }
        /// <summary>
        /// The username of the user
        /// </summary>
        /// <value>Plain text</value>
        public string Username { get; set; }   
        /// <summary>
        /// The first name of the user
        /// </summary>
        /// <value>Plain text</value>
        public string FirstName { get; set; }
        /// <summary>
        /// The last name of the user
        /// </summary>
        /// <value>Plain text</value>        
        public string LastName { get; set; }
        /// <summary>
        /// The email of the user
        /// </summary>
        /// <value>Plain text including email provider</value>
        public string Email {get;set;}
        /// <summary>
        /// The birthday of the user
        /// </summary>
        /// <value>Default locale - DateTime</value>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// The country of the user
        /// </summary>
        /// <value>Plain text - full name</value>
        public string Country { get; set; }
        /// <summary>
        /// The profile picture of the user
        /// </summary>
        /// <value>BLOB format - stored as byte array</value>
        public byte[] Image { get; set;}
        /// <summary>
        /// The current token associated with this user - assigned at login and removed at logout/timeout
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual plain text</value>
        public virtual string Token {get;set;}
        /// <summary>
        /// The salt that was used to hash the password - generated when registering
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual byte array</value>
        public virtual byte[] PasswordSalt {get; set;}
        /// <summary>
        /// The password in its hashed format - hashed on registration in the controller
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Plain text hash</value>
        public virtual string PasswordHash {get; set;}
        /// <summary>
        /// The collection of personal plants the user has
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of userPlants</value>
        public virtual ICollection<UserPlant> UserPlants {get;set;}
        /// <summary>
        /// The collection of notes the user has
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of notes</value>        
        public virtual ICollection<Note> Notes {get;set;}
    }
}