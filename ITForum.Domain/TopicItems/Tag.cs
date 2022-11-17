using System.ComponentModel.DataAnnotations;

namespace ITForum.Domain.TopicItems
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
