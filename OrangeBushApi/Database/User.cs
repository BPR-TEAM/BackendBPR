using System;

namespace OrangeBushApi.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }   

        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }

        public virtual byte[] PasswordSalt {get; set;}
        public virtual byte[] PasswordHash {get; set;}
        public virtual ICollection<Plant> Plants {get;set;}
    }
}