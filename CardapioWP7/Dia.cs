using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardapioWP7
{
    class Dia
    {
        public DateTime Data { get; set; }
        public string Desjejum { get; set; }
        public string Almoco { get; set; }
        public string Jantar { get; set; }

        public Dia(DateTime _data)
        {
            this.Data = _data;
            this.Almoco = "";
            this.Jantar = "";
        }
    }
}
