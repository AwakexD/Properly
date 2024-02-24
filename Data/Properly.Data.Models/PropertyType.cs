namespace Properly.Data.Models
{
    using Properly.Data.Common.Models;

    public class PropertyType : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public virtual Property Property { get; set; }
    }
}
