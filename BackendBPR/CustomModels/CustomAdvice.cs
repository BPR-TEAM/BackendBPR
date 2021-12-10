namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a piece of advice given by a user 
    /// </summary>
    public class CustomAdvice
    {
        /// <summary>
        /// The advice Id
        /// </summary>
        /// <value>Random integer representing the adviceId</value>
        public int Id { get; set; }
        /// <summary>
        /// The tagId that corresponds to this piece of advice IF APPLICABLE else NULL
        /// </summary>
        /// <value>Random integer representing the tagId</value>
        public int? TagId {get; set;}
        /// <summary>
        /// The tag that corresponds to this piece of advice
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual Tag Tag {get;set;}
        /// <summary>
        /// The description for this advice
        /// </summary>
        /// <value>Plain text</value>
        public string Description {get; set;}
        /// <summary>
        /// Count of likes in the current advice
        /// </summary>
        /// <value>Integer</value>
        public int Likes {get;set;}
        /// <summary>
        /// Count of dislikes in the current advice
        /// </summary>
        /// <value>Integer</value>
        public int Dislikes {get;set;}
        /// <summary>
        /// Id of the user that crated this advice
        /// </summary>
        /// <value>Integer</value>
        public int CreatorId {get;set;}
        /// <summary>
        /// Current user's role
        /// </summary>
        /// <value>AdviceRole enumerator</value>
        public AdviceRole CurrentUserRole {get;set;}
    }
}