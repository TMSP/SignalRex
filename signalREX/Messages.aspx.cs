using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace signalREX {
    
public partial class WebForm1 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            SendNotifications();
        }

        public void SendNotifications() {
            bool noshow     = false;
            bool ativo      = false;
            bool prioridade = false;
            bool chamado    = false;            
            
            List<cliente> lst = new List<cliente>();
            string message = string.Empty;
            string conStr = "Data Source=192.168.0.3;Initial Catalog=maderomesa; User Id=banco; Password=banco;";
            DataTable dt = new DataTable();
            

            using (SqlConnection connection = new SqlConnection(conStr)) {
                string query = "SELECT client.EMAILCLI, client.NOMECLI, client.TELEFONECLI, client.PRIORIDADE, " +
                    " b.PAGER, b.LUGARES, b.OBSERVACOES, b.NOSHOW, b.CHAMADO, b.ATIVO, b.TEMPOCHEGADA, b.TEMPOSAIDA "+
                    " FROM [dbo].[cliente] AS client INNER JOIN [dbo].[maderofila] AS b ON client.TELEFONECLI = b.TELEFONECLI WHERE b.ATIVO=1";
                                     

                using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Notification = null;
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);

                   

                    if (dt.Rows.Count > 0) {
                        for (int i = 0; i < dt.Rows.Count; ++i) {
                            cliente cli = new cliente();

                            cli.nomeCli      = dt.Rows[i]["NOMECLI"].ToString();
                            cli.emailCli     = dt.Rows[i]["EMAILCLI"].ToString();
                            cli.telefoneCli  = dt.Rows[i]["TELEFONECLI"].ToString();
                            cli.observacoes  = dt.Rows[i]["OBSERVACOES"].ToString();
                            bool.TryParse(dt.Rows[i]["NOSHOW"].ToString(), out cli.noshow);
                            bool.TryParse(dt.Rows[i]["ATIVO"].ToString(), out cli.ativo);
                            bool.TryParse(dt.Rows[i]["PRIORIDADE"].ToString(), out cli.prioridade);
                            bool.TryParse(dt.Rows[i]["CHAMADO"].ToString(), out cli.chamado);
                            cli.lugares      = int.Parse(dt.Rows[i]["LUGARES"].ToString());                            
                            DateTime.TryParse(dt.Rows[i]["TEMPOCHEGADA"].ToString(), out cli.tempoChegada);
                            DateTime.TryParse(dt.Rows[i]["TEMPOSAIDA"].ToString(), out cli.temposaida);                            
                            cli.pager = dt.Rows[i]["PAGER"].ToString();

                            lst.Add(cli);
                        }
                    }
                    
                }
            }
            MyHub1 nHub = new MyHub1();
            nHub.NotifyAllClients(lst);
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e) {
            if (e.Type == SqlNotificationType.Change) {
                SendNotifications();
            }
        }
    }
}