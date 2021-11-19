namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a custom made measurementDefinition defined by the user
    /// </summary>
    public class CustomMeasurementDefinition : MeasurementDefinition
    {
        /// <summary>
        /// The userPlantId that this measurementDefinition is attached to
        /// </summary>
        /// <value>Random integer representing the userPlantId</value>
        public int UserPlantId {get;set;}
        /// <summary>
        /// The userPlant that corresponds to this customMeasurementDefinition
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual UserPlant UserPlant {get;set;}
    }
}