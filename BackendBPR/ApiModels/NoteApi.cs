namespace BackendBPR.ApiModels
{
    /// <summary>
    /// Class that represents a personal note of a user
    /// </summary>
    public class NoteApi
    {
        /// <summary>
        /// The note Id
        /// </summary>
        /// <value>Random integer representing the Id</value>
        public int Id { get; set; }
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
    }
}