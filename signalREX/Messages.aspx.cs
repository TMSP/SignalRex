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
            List<cliente> lst = new List<cliente>();
            string message = string.Empty;
            string conStr = "Data Source=192.168.0.3;Initial Catalog=maderomesa; User Id=banco; Password=banco;";
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(conStr)) {
                string query = "SELECT [NOMECLI], [EMAILCLI], [TELEFONECLI] FROM [dbo].[cliente]";

                using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Notification = null;
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0) {
                        for(int i = 0; i < dt.Rows.Count; ++i) {
                            lst.Add( new cliente {
                                nomeCli     = dt.Rows[i]["NOMECLI"].ToString(),
                                emailCli    = dt.Rows[i]["EMAILCLI"].ToString(),
                                telefoneCli = dt.Rows[i]["TELEFONECLI"].ToString() });
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