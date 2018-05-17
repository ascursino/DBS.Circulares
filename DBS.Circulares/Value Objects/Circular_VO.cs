using System;
using System.Collections.Generic;
using System.Text;

namespace DBS.Circulares
{
    /// <summary>
    /// Informações resumidas das circulares para exibição e sort via linq
    /// </summary>
    public class Circular_VO
    {
        //Variáveis
        private int id;
        private String titulo;
        private String mensagem;
        private bool ciente;
        private DateTime dataHoraCiente;

        
        //Propriedades        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public String Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }
        
        public String Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }
        
        public bool Ciente
        {
            get { return ciente; }
            set { ciente = value; }
        }
        
        public DateTime DataHoraCiente
        {
            get { return dataHoraCiente; }
            set { dataHoraCiente = value; }
        }

    }
}
