namespace BackendBPR.Database
{
    /// <summary>
    /// Class that represents a single board of a whole dashboard
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The board Id
        /// </summary>
        /// <value>Random integer representing the dashboardId</value>
        public int Id { get; set; }
        /// <summary>
        /// The plantId that corresponds to this board
        /// </summary>
        /// <value>Random integer representing the plantId</value>
        public int PlantId { get; set; }
        /// <summary>
        /// The dashboardId that corresponds to this board
        /// </summary>
        /// <value>Random integer representing the dashboardId</value>
        public int DashboardId { get; set; }
        /// <summary>
        /// The graphical type of this board - i.e gauge, bar, pie chart, etc
        /// </summary>
        /// <value>Plain text</value>
        public string Type { get; set; }
        /// <summary>
        /// The dashboard that this board belongs to
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>        
        public virtual Dashboard Dashboard {get;set;}
        /// <summary>
        /// The plant that this board is associated with
        /// Virtual so it needs to be 'included' when using LINQ queries
        /// </summary>
        /// <value>Virtual object</value>
        public virtual Plant Plant{ get; set; }
    }
}