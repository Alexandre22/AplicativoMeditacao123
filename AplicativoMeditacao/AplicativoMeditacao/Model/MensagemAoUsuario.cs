using SQLite;

namespace AplicativoMeditacao.Model
{
    public class MensagemAoUsuario
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        
        public string Mensagem { get; set; }

        public MensagemAoUsuario() { }

    }
}
