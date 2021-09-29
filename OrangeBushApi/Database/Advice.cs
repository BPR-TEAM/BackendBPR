using System;

namespace OrangeBushApi.Database
{
    public class Advice
    {
        public int Id { get; set; }
        public string Description {get; set;}
        public int Likes {get;set;}
        public List<string> WhoVoted {get;set;}

        public User User { get; set; }
    }
}