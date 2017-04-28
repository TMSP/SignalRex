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
            List<string> lst = new List<string>();
            string message = string.Empty;
            string conStr = "Data Source=192.168.0.3;Initial Catalog=Test; User Id=banco; Password=banco;";
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(conStr)) {
                string query = "SELECT [Message] FROM [dbo].[DummyData]";

                using (SqlCommand command = new SqlCommand(query, connection)) {
                    command.Notification = null;
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);

                    if (dt.Rows.Count > 0) {
                        for(int i = 0; i < dt.Rows.Count; ++i) {
                            lst.Add(dt.Rows[i]["Message"].ToString());
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