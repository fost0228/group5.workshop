using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmMain : Form
    {
        int index;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Populate combo box with options and set selected index
            cboSelect.Items.Add(" ------   Select Table   ------");
            cboSelect.Items.Add("               Packages        ");
            cboSelect.Items.Add("               Products        ");
            cboSelect.Items.Add("               Suppliers       ");

            cboSelect.SelectedIndex = 0;

            // Set data grid view data source to null
            dgvMain.DataSource = null;
        }


        // Member Event Handlers =================================================================

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void cboSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = cboSelect.SelectedIndex;

            // Set data grid view and buttons to default state
            dgvMain.DataSource = null; // clear data source
            DisableButtons();

            // Set selection mode to full row
            this.dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.MultiSelect = false;

            // set darker background color on alternating rows
            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Depending on which table is selected in the combo box,
            // populate data grid view with that data

            if (index == 1)
            {
                DisplayPackages();
                EnableButtons();
            } 
            else if (index == 2)
            {
                DisplayProducts();
                EnableButtons();
            }
            else if (index == 3)
            {
                DisplaySuppliers();
                EnableButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (index == 1)
            {
                frmAddModifyPackage packageForm = new frmAddModifyPackage();
                packageForm.package = null;
                packageForm.isAdd = true;

                DialogResult result = packageForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        try
                        {
                            db.Packages.Add(packageForm.package);
                            db.SaveChanges();

                            DisplayPackages();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding package", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (index == 2)
            {
                frmAddModifyProduct productForm = new frmAddModifyProduct();
                productForm.product = null;
                productForm.isAdd = true;

                DialogResult result = productForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        try
                        {
                            db.Products.Add(productForm.product);
                            db.SaveChanges();

                            DisplayProducts();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding product", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (index == 3)
            {
                frmAddModifySupplier supplierForm = new frmAddModifySupplier();
                supplierForm.supplier = null;
                supplierForm.isAdd = true;

                DialogResult result = supplierForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        // Find all supplier IDs
                        var supplierIds = db.Suppliers.Select(c => new { ID = c.SupplierId }).ToList();

                        // auto increment supplierId based on available IDs
                        foreach (var s in supplierIds)
                        {
                            if (supplierForm.supplier.SupplierId == s.ID)
                            {
                                supplierForm.supplier.SupplierId = s.ID;
                                supplierForm.supplier.SupplierId += 1;
                            }
                        }

                        try
                        {
                            db.Suppliers.Add(supplierForm.supplier);
                            db.SaveChanges();

                            DisplaySuppliers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding supplier", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (index == 1) // PACKAGES TABLE
            {
                frmAddModifyPackage packageForm = new frmAddModifyPackage();
                Packages selectedPackage;
                packageForm.isAdd = false;


                using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
                {
                    //String value for primary key column of selected row in data grid view
                    string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigns query result as the current selected product
                    selectedPackage = db.Packages.SingleOrDefault(p => p.PkgName == value);
                } // close

                packageForm.package = selectedPackage;

                DialogResult result = packageForm.ShowDialog();


                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        try
                        {
                            db.Packages.Update(packageForm.package);
                            db.SaveChanges();

                            DisplayPackages();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating Packages table", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        }
                    }
                }
            }
            else if (index == 2) // PRODUCTS TABLE
            {
                frmAddModifyProduct productForm = new frmAddModifyProduct();
                Products selectedProduct;
                productForm.isAdd = false;


                using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
                {
                    //String value for primary key column of selected row in data grid view
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigns query result as the current selected product
                    selectedProduct = db.Products.Find(value);
                } // close

                productForm.product = selectedProduct;

                DialogResult result = productForm.ShowDialog();


                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        try
                        {
                            db.Products.Update(productForm.product);
                            db.SaveChanges();

                            DisplayProducts();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating Products table", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (index == 3) // SUPPLIERS TABLE
            {
                frmAddModifySupplier supplierForm = new frmAddModifySupplier();
                Suppliers selectedSupplier;
                supplierForm.isAdd = false;

                using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
                {
                    //String value for primary key column of selected row in data grid view
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigns query result as the current selected product
                    selectedSupplier = db.Suppliers.Find(value);
                } // close

                supplierForm.supplier = selectedSupplier;

                DialogResult result = supplierForm.ShowDialog();


                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext())
                    {
                        try
                        {
                            db.Suppliers.Update(supplierForm.supplier);
                            db.SaveChanges();

                            DisplaySuppliers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating Supplier table", ex.GetType().ToString(),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            
        }


        /// <summary>
        /// 
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (index == 1) // Packages
            {
                Packages selectedPackage;

                using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
                {
                    //String value for primary key column of selected row in data grid view
                    string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigns query result as the current selected product
                    selectedPackage = db.Packages.SingleOrDefault(p => p.PkgName == value);
                } // close


                if (selectedPackage != null)
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedPackage.PkgName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {
                            using (TravelExpertsContext db = new TravelExpertsContext())
                            {
                                db.Packages.Remove(selectedPackage);
                                db.SaveChanges();

                                DisplayPackages();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Other error while deleting package: " + ex.Message,
                                ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Package needs to be selected", "Error deleting package",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (index == 2) // Products
            {
                Products selectedProduct;

                using (TravelExpertsContext db = new TravelExpertsContext())
                {
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    selectedProduct = db.Products.Find(value);
                }

                if (selectedProduct != null)
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedProduct.ProdName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {
                            using (TravelExpertsContext db = new TravelExpertsContext())
                            {
                                db.Products.Remove(selectedProduct);
                                db.SaveChanges();

                                DisplayProducts();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Other error while deleting product: " + ex.Message,
                                ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product needs to be selected", "Error deleting product",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (index == 3) // Supplier
            {
                Suppliers selectedSupplier;

                using (TravelExpertsContext db = new TravelExpertsContext())
                {
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    selectedSupplier = db.Suppliers.Find(value);
                }

                if (selectedSupplier != null)
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedSupplier.SupName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {
                            using (TravelExpertsContext db = new TravelExpertsContext())
                            {
                                db.Suppliers.Remove(selectedSupplier);
                                db.SaveChanges();

                                DisplaySuppliers();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Other error while deleting supplier: " + ex.Message,
                                ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Supplier needs to be selected", "Error deleting supplier",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Member Methods ========================================================================
        private void DisplayPackages()
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {

                var packages = db.Packages.OrderBy(p => p.PackageId)
                    .Select(p => new {
                        p.PkgName,
                        p.PkgStartDate,
                        p.PkgEndDate,
                        p.PkgDesc,
                        p.PkgBasePrice,
                        p.PkgAgencyCommission
                    })
                    .ToList();

                // Set Data Grid View data source
                dgvMain.DataSource = packages;

                // Set header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[2].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[3].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[4].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[5].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Set Column width
                dgvMain.Columns[0].Width = 225; // Name
                dgvMain.Columns[1].Width = 150; // Start Date
                dgvMain.Columns[2].Width = 150; // End Date
                dgvMain.Columns[3].Width = 375; // Description
                dgvMain.Columns[4].Width = 150; // Base Price
                dgvMain.Columns[5].Width = 240; // Agency Commission

                // Rename columns
                //dgvMain.Columns[0].HeaderText = "Package Id";
                dgvMain.Columns[0].HeaderText = "Package Name";
                dgvMain.Columns[1].HeaderText = "Start Date";
                dgvMain.Columns[2].HeaderText = "End Date";
                dgvMain.Columns[3].HeaderText = "Description";
                dgvMain.Columns[4].HeaderText = "Base Price";
                dgvMain.Columns[5].HeaderText = "Agency Commission";

                // Format prices as currency
                dgvMain.Columns[4].DefaultCellStyle.Format = "c";
                dgvMain.Columns[5].DefaultCellStyle.Format = "c";

            }
        }


        private void DisplayProducts()
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {

                var products = db.Products.OrderBy(p => p.ProductId)
                    .Select(p => new { p.ProductId, p.ProdName })
                    .ToList();

                // Set Data Grid View data source
                dgvMain.DataSource = products;

                // Set header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Set Column width
                dgvMain.Columns[0].Width = 140;
                dgvMain.Columns[1].Width = 986;

                // Rename columns
                dgvMain.Columns[0].HeaderText = "Product Id";
                dgvMain.Columns[1].HeaderText = "Product Name";
            }
        }


        private void DisplaySuppliers()
        {
            using (TravelExpertsContext db = new TravelExpertsContext())
            {

                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId)
                    .Select(s => new { s.SupplierId, s.SupName })
                    .ToList();

                // Set Data Grid View data source
                dgvMain.DataSource = suppliers;

                // Set header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Set Column width
                dgvMain.Columns[0].Width = 150;
                dgvMain.Columns[1].Width = 955;

                // Rename columns
                dgvMain.Columns[0].HeaderText = "Supplier Id";
                dgvMain.Columns[1].HeaderText = "Supplier Name";
            }
        }


        private void EnableButtons()
        {
            btnAdd.Enabled = true;
            btnModify.Enabled = true;
            btnDelete.Enabled = true;
        }


        private void DisableButtons()
        {
            btnAdd.Enabled = false;
            btnModify.Enabled = false;
            btnDelete.Enabled = false;
        }

       
    } // class
} // namespace
