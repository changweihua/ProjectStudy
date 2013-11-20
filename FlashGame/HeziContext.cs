using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace FlashGame
{
    [Table]
    public class GameInfo
    {
        [Column]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string XName { get; set; }
        [Column]
        public string Cover { get; set; }
        [Column]
        public string Description { get; set; }
    }

    [Database(Name="hezi")]
    public class HeziContext : DataContext
    {
        public HeziContext(string connString)
            : base(connString)
        {
        }

        public Table<GameInfo> Games { get; set; }

    }
}
