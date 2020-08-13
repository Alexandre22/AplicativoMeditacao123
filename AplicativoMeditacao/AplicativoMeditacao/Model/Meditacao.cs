using SQLite;

namespace AplicativoMeditacao.Model
{
    public class Meditacao
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int TempoMaximoHumming { get; set; }
        public int TempoMaximoUMDP { get; set; }
        public string AudioHumming { get; set; }
        public string AudioUMP { get; set; } 
        public int TempoTotalMeditado { get; set; }

        public Meditacao()
        {

        }
    }
}
