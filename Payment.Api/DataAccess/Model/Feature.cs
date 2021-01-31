using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("Feature")]
    public class Feature
    {

        [Column("FeatureID")] public int Id { get; set; }
        public string Key { get; set; }
        public string DataType { get; set; }
        public bool ShowInSummery { get; set; }
        public int TypeId { get; set; }
        public FeatureType Type { get; set; }
    }
}
