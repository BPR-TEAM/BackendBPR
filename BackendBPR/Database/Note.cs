using System;

namespace BackendBPR.Database
{
    public class Note
    {
        public int Id { get; set; }
        public int UserId { get; set;}
        public int? PlantId {get; set;}
        public string Text { get; set;}

        public virtual User User { get; set; }
        public virtual Plant Plant { get; set; }
    }
}