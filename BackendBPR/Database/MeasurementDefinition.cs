namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents the default measurement definitions that define a plant
    /// </summary>
    public class MeasurementDefinition
    {
        /// <summary>
        /// The measurementDefinition Id
        /// </summary>
        /// <value>Random integer that represents the Id</value>
        public int Id { get; set; }
        /// <summary>
        /// The plantId that this measurementDefinition is attached to
        /// </summary>
        /// <value>Random integer that represents the plantId</value>
        public int PlantId { get; set; }
        /// <summary>
        /// The name of the default measurementDefinition
        /// </summary>
        /// <value>Plain text</value>
        public string Name { get; set; }
        /// <summary>
        /// The preferred range of the value of the measurementDefinition
        /// </summary>
        /// <value>Plain text</value>
        public string PreferredRange {get;set;}
        /// <summary>
        /// The description of the default measurementDefinition
        /// </summary>
        /// <value>Plain text</value>
        public string Description { get; set; }        
        /// <summary>
        /// The plant that this measurementDefinition is attached to
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual Plant Plant{ get; set; }
    }
}