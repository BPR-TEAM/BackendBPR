using System.Collections.Generic;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class for representing a personal plant of a user
    /// </summary>
    public class UserPlantApi
    {
        /// <summary>
        /// The Id of the personal plant
        /// </summary>
        /// <value>Random integer representing the plantId</value>
        public int Id {get;set;}
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
    }
}