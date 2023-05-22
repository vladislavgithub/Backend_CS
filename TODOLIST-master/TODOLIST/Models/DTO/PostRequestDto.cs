namespace TODOLIST.Models.DTO
{
    public class PostRequestDto
    {
        public int PersonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }
}
