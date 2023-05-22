namespace TODOLIST.Models
{
    public class RequestData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime AddData { get; set; }
        public DateTime ChangeData { get; set; }

        public int StatusId { get; set; }
    }
}
