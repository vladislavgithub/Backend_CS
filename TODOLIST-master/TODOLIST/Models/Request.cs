namespace TODOLIST.Models
{
    public class Request
    {
        public int id { get; set; }
       
        public int PersonId { get; set; }

        public RequestData RequestData { get; set; }

        public Request ()
        {

        }

        public Request(string Title, string Description, int PersonId, int StatusId)
        {
            this.RequestData = new RequestData
            {
                Title = Title,
                Description = Description,
                StatusId = StatusId,
                AddData = DateTime.Now,
                ChangeData = DateTime.Now
            };
            this.PersonId = PersonId;
        }
    }
}
