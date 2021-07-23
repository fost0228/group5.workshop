using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmAddModifyProduct : Form
    {
        public bool isAdd;
        public Products product;

        public frmAddModifyProduct()
        {
            InitializeComponent();
        }

        private void frmAddModifyProduct_Load(object sender, EventArgs e)
        {
            if (this.isAdd)
            {
                product = new Products();

                this.Text = "Add Product";
            }
            else
            {
                this.Text = "Modify Product";

                if (product == null)
                {
                    MessageBox.Show("Product was not selected", "Error has occured",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

                txtName.Text = product.ProdName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtName))
            {
                if (this.isAdd)
                {
                    product = new Products();
                }

                product.ProdName = txtName.Text;

                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
