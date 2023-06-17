using System.Collections.Generic;

namespace Configs
{
    public interface IConfigProvider
    {
        public List<IConfigData> GetData();
    }
}