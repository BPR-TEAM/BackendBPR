namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a personal note of a user
    /// </summary>
    public class Note
    {
        /// <summary>
        /// The note Id
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int Id { get; set; }
        /// <summary>
        /// The userId that this note belongs to
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int UserId { get; set;}
        /// <summary>
        /// The plantId that this note belongs to IF it was assigned, if not null and acts like a generalized note
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int? PlantId {get; set;}
        /// <summary>
        /// The text body for the note
        /// </summary>
        /// <value>Plain text</value>
        public string Text { get; set;}
        /// <summary>
        /// The user that this note was written by
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual User User { get; set; }
        /// <summary>
        /// The plant that this was written about IF APPLICABLE
        /// </summary>
        /// <value>Virtual object</value>
        public virtual Plant Plant { get; set; }
    }
}