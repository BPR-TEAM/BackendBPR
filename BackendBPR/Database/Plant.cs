using System.Collections.Generic;

namespace BackendBPR.Database
{
    /// <summary>
    /// Class for representing a plant
    /// </summary>
    public class Plant
    {
        /// <summary>
        /// The Id of the plant
        /// </summary>
        /// <value>Random integer that represents the Id</value>
        public int Id { get; set; }
        /// <summary>
        /// The common name of the plant - example Common Daisy
        /// </summary>
        /// <value>Plain text</value>
        public string CommonName { get; set; }
        /// <summary>
        /// The scientific name of the plant - example Bellis perennis
        /// </summary>
        /// <value>Plain text</value>  
        public string ScientificName { get; set; }
        ///<summary>
        /// The image that represents this plant
        /// </summary>
        /// <value>BLOB format - stored as byte array</value>  
        public byte[] Image { get; set;}
        /// <summary>
        /// The URL to the wikipedia page of the plant
        /// </summary>
        /// <value>Plain text URL</value>
        public string Url { get; set; }
        /// <summary>
        /// The description of the plant with helpful information
        /// </summary>
        /// <value>Plain text</value>
        public string Description { get; set; }
        /// <summary>
        /// The collection of userPlants that this plant acts as a base to
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of userPlants</value>
        public virtual ICollection<UserPlant> UserPlants { get; set; }  
        /// <summary>
        /// The collection of default tags that this plant has
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of tags</value>     
        public virtual ICollection<Tag> Tags { get; set; }
        /// <summary>
        /// The collection of default measurement definitions that define this plant
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual collection of measurementDefinitions</value>
        public virtual ICollection<MeasurementDefinition> MeasurementDefinitions {get;set;}        
    }
}