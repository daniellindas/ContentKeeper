﻿using ContentKeeperService.Attributes;

namespace ContentKeeperService.Entities
{

    [UseAllProperties]
    public class ContentEntry
    {
        public string Id { get; set; }

        public string Content { get; set; }
    }
}
