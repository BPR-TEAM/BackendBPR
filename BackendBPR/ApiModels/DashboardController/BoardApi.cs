namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a single board of a whole dashboard
    /// </summary>
    public class BoardApi
    {
        /// <summary>
        /// The board Id - Not necessary when adding a new Board
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
        /// The graph type and measurement. The format should follow this example: "barChart,CO2"
        /// </summary>
        /// <value>Plain text</value>
        public string Type { get; set; }  
    }
}