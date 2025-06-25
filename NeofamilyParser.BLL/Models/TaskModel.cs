namespace NeofamilyParser.BLL.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string? Section { get; set; }
        public string? Theme { get; set; }
        public int Part { get; set; }
        public int Number { get; set; }
        public string? Source { get; set; }
        public string? QuestionText { get; set; }
        public IEnumerable<TaskImageModel>? QuestionImages { get; set; }
        public string? Answer { get; set; }
        public string? SolutionText { get; set; }
        public IEnumerable<TaskImageModel>? SolutionImages { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TaskImageModel
    {
        public string Name { get; set; }
        public byte[] Image {  get; set; }
    }
}
