using System.Collections.Generic;

namespace Database
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public byte[] Image { get; set;}
        public string Url { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }       
        public virtual ICollection<Tag> Tags { get; set; }
    }
}