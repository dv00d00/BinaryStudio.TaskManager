using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// View model for the manager tasks view
    /// </summary>
    public class ManagerTasksViewModel
    {
        /// <summary>
        /// Gets or sets the color of the manager.
        /// </summary>
        /// <value>
        /// The color of the manager.
        /// </value>
        public Color ManagerColor { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>
        /// The manager.
        /// </value>
        public User Manager { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public List<HumanTask> Tasks { get; set; }
    }
}