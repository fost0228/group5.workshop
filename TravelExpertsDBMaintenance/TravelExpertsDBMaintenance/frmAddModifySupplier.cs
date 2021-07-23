using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmAddModifySupplier : Form
    {
        public bool isAdd;
        public Suppliers supplier;

        public frmAddModifySupplier()
        {
            InitializeComponent();
        }

        private void frmAddModifySupplier_Load(object sender, EventArgs e)
        {
            if (this.isAdd)
            {
                supplier = new Suppliers();

                this.Text = "Add Supplier";
            }
            else
            {
                this.Text = "Modify Supplier";
                lstProductList.Items.Clear();

                if (supplier == null)
                {
                    MessageBox.Show("There is no supplier selected", "Error has occured", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Close();
                }

                using (TravelExpertsContext db = new TravelExpertsContext())
                {
                    // populate combo box

                    List<Products> products = db.Products.ToList();

                    cboSelect.DataSource = products;
                    cboSelect.ValueMember = "ProductId";
                    cboSelect.DisplayMember = "ProdName";

                   // populate product list
                    var productList = db.Products.Join(db.ProductsSuppliers,
                                        p => p.ProductId,
                                        ps => ps.ProductId,
                                        (p, ps) => new { ProdName = p.ProdName, SupID = ps.SupplierId })
                        .Where(x => x.SupID == supplier.SupplierId)
                        .ToList();

                    foreach (var p in productList)
                    {
                        lstProductList.Items.Add(p.ProdName);
                    }
                }

                txtName.Text = supplier.SupName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtName))
            {
                if (this.isAdd)
                {
                    supplier = new Suppliers();
;               }

                supplier.SupplierId = 1; // Default value
                supplier.SupName = txtName.Text;

                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
