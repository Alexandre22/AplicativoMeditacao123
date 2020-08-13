using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Model
{
    public class ConteudoDaPagina
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Versao { get; set; }

        public ConteudoDaPagina()
        {

        }

    }
}
