using BackendBPR.Database;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a piece of advice given by a user 
    /// </summary>
    public class AdviceExtendedApi
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
        /// Name of the user that created this advice
        /// </summary>
        /// <value>String</value>
        public string CreatorName {get;set;}
        /// <summary>
        /// Image of the creator
        /// </summary>
        /// <value>Byte array with the image</value>
        public byte[] CreatorImage {get;set;}
        /// <summary>
        /// Current user's role
        /// </summary>
        /// <value>AdviceRole enumerator</value>
        public AdviceRole CurrentUserRole {get;set;}
    }
}