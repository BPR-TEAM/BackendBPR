using System;
namespace BackendBPR.Database
{
    public class Board
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int DashboardId { get; set; }
        public string Type { get; set; }        
        public virtual Dashboard Dashboard {get;set;}
        public virtual Plant Plant{ get; set; }
    }
}