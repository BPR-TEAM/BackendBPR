using System.Collections.Generic;
namespace BackendBPR.Database
{
    /// <summary>
    /// Class for representing a creation, like, etc action for an advice
    /// </summary>
    public class UserAdvice
    {
        /// <summary>
        /// The id of the user that took this action
        /// </summary>
        /// <value>Random integer representing the userId</value>
        public int UserId {get;set;}
        /// <summary>
        /// The id of the advice that it is in question
        /// </summary>
        /// <value>Random integer representing the adviceId</value>
        public int AdviceId {get;set;}
        /// <summary>
        /// The user that this userAdvice action is attached to - virtual so it needs to be 'included'
        /// when doing LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual User User {get;set;}
        /// <summary>
        /// The advice that this userAdvice action is about - virtual so it needs to 'included'
        /// when doing LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual Advice Advice {get;set;}
        /// <summary>
        /// The action that was taken
        /// </summary>
        /// <value>Enumerator describing the action</value>
        public AdviceRole Type {get;set;}        
    }
}

/// <summary>
/// Enumerator for describing the taken action
/// </summary>
public enum AdviceRole
{
    /// <summary> Represents a creator </summary>
    Creator = 2,
    /// <summary> Represents a like </summary>
    Like = 1,
    /// <summary> Represents a dislike </summary>
    Dislike = 0
}