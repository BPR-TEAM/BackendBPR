using System.Collections.Generic;

namespace Database
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual int? UserId {get;set;}                
        public virtual ICollection<Plant> Plants {get;set;}
    }
}