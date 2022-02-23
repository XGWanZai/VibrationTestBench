using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationTestBench.Models
{
    class MotoModelDetails
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        public string sudushangxianzhi { get; set; }

        public string suduxiaxianzhi { get; set; }

        public string jiasuduzhishangxian { get; set; }

        public string jiasuduzhixiaxian { get; set; }

        public int motoId { get; set; }


    }
}
