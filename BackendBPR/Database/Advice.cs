namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a piece of advice given by a user
    /// </summary>
    public class Advice
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
    }
}