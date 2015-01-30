using ContentKeeperService.Attributes;

namespace ContentKeeperService.Entities
{
    [UseAllProperties]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
