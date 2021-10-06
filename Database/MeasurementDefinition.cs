using System;

namespace BackendBPR.Database
{
    public class MeasurementDefinition
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string Name { get; set; }
        public string PreferredRange {get;set;}
        public string Description { get; set; }        

        public virtual Plant Plant{ get; set; }
    }
}