using ContentKeeperService.Attributes;

namespace ContentKeeperService.Entities
{
    public class ContentEntry
    {
        [UseThisProperty]
        public string Id { get; set; }

        public string Content { get; set; }
    }
}
