using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
/// <summary>
/// log in functionality
/// we just use customerid and customerHomePhone as userId and password
/// </summary>
namespace TravelExpertsDBMaintenance
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Log in the system
        /// </summary>
        /// author: Jiang Hou
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //get information the user is inputing
            string Name = txtUsername.Text.Trim();
            string Pwd = txtPassword.Text.Trim();

            //justify if it is null
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Please input username!", "Log In", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(Pwd))
            {
                MessageBox.Show("Please input password!", "Log In", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }

            //check in the database to see if it is right(due to we don't have a user table,we just use the cumster table to show this functionality)

            {
                //build the connection by the string
                string connString = "server=localhost\\adocoxsql;database=TravelExperts;Integrated Security=true";//windows check
                SqlConnection conn = new SqlConnection(connString);

                //query
                string sql = "select count(1) from Agents where AgtUsername='"+Name+"' and AgtPassword='"+Pwd+"'";

                //new a command object
                SqlCommand cmd = new SqlCommand(sql, conn);

                //open connection
                conn.Open();

                //execute the order
                object o = cmd.ExecuteScalar();

                //close connection
                conn.Close();

                //process the result
                if (o == null || o == DBNull.Value || ((int)o) == 0)
                {
                    MessageBox.Show("The Username or Password you have entered is incorrect. Please try again!", "Log In", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //MessageBox.Show("You have logged in successfully. Welcome!", "Success!", MessageBoxButtons.OK,
                    //MessageBoxIcon.Information);
                    
                    //go to the main form

                    frmMain mainwindow = new frmMain();

                    mainwindow.Username = Name;

                    mainwindow.Show();

                    this.Hide();

                }
            }
        }
        //
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
