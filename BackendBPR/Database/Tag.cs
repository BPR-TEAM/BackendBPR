using System.Collections.Generic;

namespace BackendBPR.Database
{
    /// <summary>
    /// Class for representing a tag for a plant
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The tag Id
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int Id { get; set; }
        /// <summary>
        /// The name of the tag
        /// </summary>
        /// <value>Plain text</value>
        public string Name { get; set; }
        /// <summary>
        /// The user that corresponds to this tag IF it was assigned by a user - personal tag
        /// </summary>
        /// <value></value>
        public virtual int? UserId {get;set;}                
        public virtual ICollection<Plant> Plants {get;set;}
    }
}