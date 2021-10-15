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
        /// The user that corresponds to this tag IF it was assigned by a user - personal tag, null otherwise
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual randomly generated userId</value>
        public virtual int? UserId {get;set;}       
        /// <summary>
        /// The collection of plants that this user has - virtual so it needs to be 'included'
        /// when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of plants</value>         
        public virtual ICollection<Plant> Plants {get;set;}
    }
}