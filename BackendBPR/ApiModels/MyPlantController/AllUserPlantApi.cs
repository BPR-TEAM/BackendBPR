using System.Collections.Generic;
using BackendBPR.Database;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class for representing a personal plant of a user
    /// </summary>
    public class AllUserPlantApi
    {
        /// <summary>
        /// The Id of the personal plant
        /// </summary>
        /// <value>Random integer representing the plantId</value>
        public int Id {get;set;}
        /// <summary>
        /// The id of the user the plant belongs to
        /// </summary>
        /// <value>Random integer corresponding to the userId</value>
        public int UserId { get; set; }
        /// <summary>
        /// The id of the plant the personal plant represents
        /// </summary>
        /// <value>Random integer corresponding to the plantId</value>
         public int PlantId { get; set; }
        /// <summary>
        /// The personal name the user assigns this plant
        /// </summary>
        /// <value>Text that the user inputs</value>
        public string Name { get; set; } 
        /// <summary>
        /// The image the user uploads to represent this plant
        /// </summary>
        /// <value>BLOB format stored as byte array</value>
        public byte[] Image { get; set;}
        /// <summary>
        /// The plant that this userPlant is attached to - virtual so it needs to be 'included'
        /// when doing LINQ queries
        /// </summary>
        /// <value>Virtual object</value>       
        public virtual Plant Plant { get; set; }
        /// <summary>
        /// The collection of measurements that this userPlant has - virtual so it needs to be 'included'
        /// when doing LINQ queries
        /// </summary>
        /// <value>Virtual collection object</value>
        public virtual ICollection<Measurement> Measurements {get;set;}        
        /// <summary>
        /// The measurement defintion list 
        /// </summary>
        /// <value>List of string containing the name  of the measurement definitions</value>
        public List<string> MeasurementsDefinitions {get;set;}

    }
}