using System;

namespace BackendBPR.Database
{
    public class Measurement
    {
        public int Id { get; set; }
        public int MeasurementDefinitionId { get; set; }       
        public int UserPlantId { get; set; }
        public string Value { get; set; }
        public DateTime Date {get;set;}     

        public virtual MeasurementDefinition MeasurementDefinition {get;set;}

        public virtual UserPlant UserPlant {get;set;}
    }
}