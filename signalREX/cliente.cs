using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace signalREX {
    public class cliente {
        public string emailCli;
        public string nomeCli;
        public string telefoneCli;
        public string observacoes;
        public bool   noshow;
        public bool   ativo;
        public bool   prioridade;
        public bool   chamado;
        public int    lugares;
        public string pager;
        public DateTime tempoChegada;
        public DateTime temposaida;
    }
}