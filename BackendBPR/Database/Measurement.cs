using System;

namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a single data point for a plant measurementDefinition
    /// </summary>
    public class Measurement
    {
        /// <summary>
        /// The measurement Id
        /// </summary>
        /// <value>Random integer representing the measurementId</value>
        public int Id { get; set; }
        /// <summary>
        /// The measurementDefinitionId corresponding to this data point
        /// </summary>
        /// <value>Random integer representing the measurementDefinitionId</value>
        public int MeasurementDefinitionId { get; set; }
        /// <summary>
        /// The userPlantId corresponding to this data point
        /// </summary>
        /// <value>Random integer representing the userPlantId</value>       
        public int UserPlantId { get; set; }
        /// <summary>
        /// The value of this data point
        /// </summary>
        /// <value>Plain text</value>
        public string Value { get; set; }
        /// <summary>
        /// The timestamp for the data point
        /// </summary>
        /// <value>Default locale - DateTime in implementation terms</value>
        public DateTime Date {get;set;}     
        /// <summary>
        /// The measurementDefinition that this data point is defined by
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual MeasurementDefinition MeasurementDefinition {get;set;}
        /// <summary>
        /// The userPlant that this data point is defined by
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual UserPlant UserPlant {get;set;}
    }
}