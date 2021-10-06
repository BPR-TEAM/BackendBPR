using System;

namespace BackendBPR.Database
{
    public class CustomMeasurementDefinition : MeasurementDefinition
    {
        public int UserPlantId {get;set;}
        public virtual UserPlant UserPlant {get;set;}
    }
}