using System.Collections.Generic;
using BackendBPR.Database;

namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a full dashboard
    /// </summary>
    public class CreateDashboardApi
    {
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
        /// User plants present in this dashboard
        /// </summary>
        /// <value>Virtual list of user plants</value>
        public virtual ICollection<UserPlant> UserPlants {get;set;}
        
    }
}