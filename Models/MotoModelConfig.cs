using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationTestBench.Models
{
   public class MotoModelConfig
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        public string motoName { get; set; }
        public int motoId { get; set; }

    }
}
