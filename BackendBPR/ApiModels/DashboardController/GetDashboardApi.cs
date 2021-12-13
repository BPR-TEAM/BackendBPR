using System.Collections.Generic;
using BackendBPR.Database;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a full dashboard
    /// </summary>
    public class GetDashboardApi
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
        /// The boards contained in this dashboard
        /// </summary>
        /// <value>Virtual list of boards</value>
        public virtual ICollection<Board> Boards{ get; set; }
        /// <summary>
        /// User plants present in this dashboard
        /// </summary>
        /// <value>Virtual list of user plants</value>
        public virtual ICollection<UserPlantDashboardApi> UserPlants {get;set;}
        
    }
}