using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CardapioWP7
{
    class Dia
    {
        public DateTime Data { get; set; }
        public string Desjejum { get; set; }
        public string Almoco { get; set; }
        public string Jantar { get; set; }
        private Dictionary<DayOfWeek, String> NomeDosDias;

        public Dia(DateTime _data)
        {
            this.Data = _data;
            this.Almoco = "";
            this.Jantar = "";

            PreparaDiasSemana();
        }

        private void PreparaDiasSemana()
        {
            NomeDosDias = new Dictionary<DayOfWeek, string>();

            NomeDosDias.Add(DayOfWeek.Sunday, "Domingo");
            NomeDosDias.Add(DayOfWeek.Monday, "Segunda");
            NomeDosDias.Add(DayOfWeek.Tuesday, "Terça");
            NomeDosDias.Add(DayOfWeek.Wednesday, "Quarta");
            NomeDosDias.Add(DayOfWeek.Thursday, "Quinta");
            NomeDosDias.Add(DayOfWeek.Friday, "Sexta");
            NomeDosDias.Add(DayOfWeek.Saturday, "Sábado");

        }

        public string NomeDoDia()
        {
            return NomeDosDias[this.Data.DayOfWeek];
        }

        
    }
}
