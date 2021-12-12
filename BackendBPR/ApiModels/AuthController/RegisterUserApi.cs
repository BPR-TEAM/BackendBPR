using System;
using System.Collections.Generic;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class for representing a user
    /// </summary>
    public class RegisterUserApi
    {
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
        /// The password
        /// </summary>
        /// <value>Plain text - password</value>
        public string PasswordHash {get; set;}
    }
}