using System;
using System.Collections.Generic;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class for representing a user
    /// </summary>
    public class LoginUserApi
    {
        /// <summary>
        /// The email of the user
        /// </summary>
        /// <value>Plain text including email provider</value>
        public string Email {get;set;}
        /// <summary>
        /// The password
        /// </summary>
        /// <value>Plain text - password</value>
        public virtual string PasswordHash {get; set;}
    }
}