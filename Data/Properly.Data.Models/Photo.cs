namespace Properly.Data.Models
{
    using System;

    using Properly.Data.Common.Models;

    public class Photo : BaseDeletableModel<Guid>
    {
        public Photo()
        {
            this.Id = Guid.NewGuid();
        }

        public string Url { get; set; }
    }
}
