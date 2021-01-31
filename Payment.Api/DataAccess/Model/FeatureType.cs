using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.Api.DataAccess.Model
{
    [Table("FeatureType")]
    public class FeatureType
    {
        public FeatureType()
        {
            Features = new HashSet<Feature>();
        }
        [Column("FeatureTypeID")]
        public int Id { get; set; }
        [Column("FeatureTypeName")]
        public string Name { get; set; }
        public ICollection<Feature> Features { get; set; }
    }
}
