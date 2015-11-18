using NFX.DataAccess.CRUD;

namespace NewShortener.Models
{
    public class Links : TypedRow
    {
        [Field(required: true)]
        public string Link { get; set; }

        [Field(required: true)]
        public string Short_Link { get; set; }
    }
}
