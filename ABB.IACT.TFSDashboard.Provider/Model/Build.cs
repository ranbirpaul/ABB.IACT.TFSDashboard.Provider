namespace ABB.IACT.TFSDashboard.Provider.Model
{
    /// <summary>
    /// Build Status Class
    /// </summary>
    public class Build
    {
        /// <summary>
        /// TFS Project Name.
        /// </summary>
        public Project project { get; set; }
        /// <summary>
        /// Build Definition Name.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Build Number.
        /// </summary>
        public string buildNumber { get; set; }
        /// <summary>
        /// Build Status.
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// Build Result.
        /// </summary>
        public string result { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}