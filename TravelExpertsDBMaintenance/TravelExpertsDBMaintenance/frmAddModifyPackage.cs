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
    public partial class frmAddModifyPackage : Form
    {
        public bool isAdd;
        public Packages package;
        public frmAddModifyPackage()
        {
            InitializeComponent();
        }

        private void frmAddModifyPackage_Load(object sender, EventArgs e)
        {
            if (this.isAdd)
            {
                package = new Packages();

                this.Text = "Add Package";
            }
            else
            {
                this.Text = "Modify Package";

                if (package == null)
                {
                    MessageBox.Show("There is no package selected", "Error has occured",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }


                using (TravelExpertsContext db = new TravelExpertsContext())
                {
                    // populate combo box
                    List<Suppliers> suppliers = db.Suppliers.OrderBy(s => s.SupName).ToList();

                    cboSupplier.DataSource = suppliers;
                    cboSupplier.ValueMember = "SupplierId";
                    cboSupplier.DisplayMember = "SupName";


                    // populate product list
                    var query1 = db.PackagesProductsSuppliers.Join(db.ProductsSuppliers,
                                        pps => pps.ProductSupplierId,
                                        ps => ps.ProductSupplierId,
                                        (pps, ps) => new { ProdSupID = pps.ProductSupplierId, SupID = ps.SupplierId, PackID = pps.PackageId });

                    var supplierList = db.Suppliers.Join(query1,
                                           s => s.SupplierId,
                                           q => q.SupID,
                                           (s, q) => new { SupName = s.SupName, ProdSupID = q.ProdSupID, PackID = q.PackID })
                                .Where(x => x.PackID == package.PackageId);


                    foreach (var s in supplierList)
                    {
                        lstSupplierList.Items.Add(s.SupName);

                    }
                }


                txtName.Text = package.PkgName;
                dtpStartDate.Value = (DateTime)package.PkgStartDate;
                dtpEndDate.Value = (DateTime)package.PkgEndDate;
                txtDescription.Text = package.PkgDesc;
                txtBasePrice.Text = String.Format("{0:0.00}", package.PkgBasePrice);
                txtCommission.Text = String.Format("{0:0.00}",package.PkgAgencyCommission);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtName) &&
                Validator.IsNonNegativeDecimal(txtBasePrice) &&
                Validator.IsPresent(txtBasePrice) &&
                Validator.IsNonNegativeDecimal(txtCommission))
            {
                if (this.isAdd)
                {
                    package = new Packages();
                }

                package.PkgName = txtName.Text;
                package.PkgStartDate = dtpStartDate.Value.Date;
                package.PkgEndDate = dtpEndDate.Value.Date;
                package.PkgDesc = txtDescription.Text;
                package.PkgBasePrice = Decimal.Parse(txtBasePrice.Text);
                package.PkgAgencyCommission = Decimal.Parse(txtCommission.Text);

                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
