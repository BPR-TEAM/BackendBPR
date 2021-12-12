using BackendBPR.Database;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a piece of advice given by a user 
    /// </summary>
    public class GiveAdviceApi
    {
        /// <summary>
        /// The tagId that corresponds to this piece of advice IF APPLICABLE else NULL
        /// </summary>
        /// <value>Random integer representing the tagId</value>
        public int TagId {get; set;}
        /// <summary>
        /// Description of the 
        /// </summary>
        /// <value></value>
        public string Description {get; set;}
    }
}