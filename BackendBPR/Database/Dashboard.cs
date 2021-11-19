using System.Collections.Generic;

namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a full dashboard
    /// </summary>
    public class Dashboard
    {
        /// <summary>
        /// The dashboard Id
        /// </summary>
        /// <value>Random integer representing the dashboardId</value>
        public int Id { get; set; }
        /// <summary>
        /// The userId that corresponds to this dashboard
        /// </summary>
        /// <value>Random integer representing the userId</value>
        public int UserId { get; set; }
        /// <summary>
        /// The name that was given to this dashboard
        /// </summary>
        /// <value>Plain text</value>
        public string Name { get; set; }
        /// <summary>
        /// The description that was given to this dashboard
        /// </summary>
        /// <value>Plain text</value>
        public string Description { get; set; }        
        /// <summary>
        /// The user that this dashboard is owned by
        /// </summary>
        /// <value>Virtual object</value>
        public virtual User User{ get; set; }
        /// <summary>
        /// The boards contained in this dashboard
        /// </summary>
        /// <value>Virtual list of boards</value>
        public virtual ICollection<Board> Boards{ get; set; }
        
    }
}