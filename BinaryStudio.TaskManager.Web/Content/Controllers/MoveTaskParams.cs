namespace BinaryStudio.TaskManager.Web.Controllers
{
    public class MoveTaskParams
    {
        public int FromEmployeeId { get; set; }
        
        public int ToEmployeeId { get; set; }

        public int TaskId { get; set; }
    }
}