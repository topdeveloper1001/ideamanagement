namespace i2Nova.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Tjdeed.Data.DataContext;
    using Tjdeed.Data.Repository;

    public class i2NovaModel : DataContext
    {
        public i2NovaModel()
            : base("name=i2NovaEntities")
        {
        }

    }
}
