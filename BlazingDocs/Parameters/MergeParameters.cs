
using BlazingDocs.Enums;

namespace BlazingDocs.Parameters
{
    public class MergeParameters
    {
        public string DataSourceName { get; set; } = "data";
        public DataSourceType DataSourceType { get; set; } = DataSourceType.Json;
        public bool Sequence { get; set; } = false;
        public bool ParseColumns { get; set; } = false;
        public bool Strict { get; set; } = false;

        public MergeParameters() { }

        public MergeParameters(DataSourceType dataSourceType)
        {
            DataSourceType = dataSourceType;
        }
    }
}
