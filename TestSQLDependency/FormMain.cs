using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TestSQLDependency
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ALTER DATABASE SOLID_DEMO SET ENABLE_BROKER;
        /// ALTER AUTHORIZATION ON DATABASE::SOLID_DEMO TO[sa];
        /// </summary>
        private void RegisterOnDependency()
        {
            string connectionString = "Data Source=192.168.1.10;Initial Catalog=CRM_StagingDb;User ID=sa;Password=Venesa@123";
            string commandText = "select id, code_customer from dbo.service_customer";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commandText;

                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);
                    SqlDependency.Start(connectionString);
                    cmd.ExecuteReader().Dispose();
                }
            }
        }

        private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            MessageBox.Show($"Listener Event Database: { e.Info }");
            RegisterOnDependency();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            RegisterOnDependency();
            MessageBox.Show("Register Success !");
        }
    }
}
