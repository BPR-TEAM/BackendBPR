using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendBPR.Database
{
    public class UserAdvice
    {
        public int UserId {get;set;}
        public int AdviceId {get;set;}

        public virtual User User {get;set;}
        public virtual Advice Advice {get;set;}
        public AdviceRole Type {get;set;}
        
    }
}


public enum AdviceRole
{
    Creator = 2,
    Like = 1,
    Dislike = 0
}