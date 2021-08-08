using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmMain : Form
    {
        int index; // Index number that corresponds to ComboBox selection

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Populates combo box with options and sets selected index when form loads
            cboSelect.Items.Add(" ------   Select Table   ------");
            cboSelect.Items.Add("               Packages        ");
            cboSelect.Items.Add("               Products        ");
            cboSelect.Items.Add("               Suppliers       ");

            cboSelect.SelectedIndex = 0;

            // Sets DataGridView data source to null
            dgvMain.DataSource = null;
        }


        // Member Event Handlers =================================================================

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // ComboBox selection change event
        private void cboSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Setting selected index
            index = cboSelect.SelectedIndex;

            // Setting DataGridView and buttons to default state
            dgvMain.DataSource = null; // clear data source
            DisableButtons();

            // Setting selection mode to full row
            this.dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMain.MultiSelect = false;

            // Setting darker background color on alternating rows
            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Depending on which table is selected in the combo box,
            // populate data grid view with that data

            if (index == 1)
            {
                DisplayPackages(); // Packages Table
                EnableButtons();
            } 
            else if (index == 2)
            {
                DisplayProducts(); // Products Table
                EnableButtons();
            }
            else if (index == 3)
            {
                DisplaySuppliers(); // Suppliers Table
                EnableButtons();
            }
        }

        /// <summary>
        /// Depending on which option is selected in ComboBox, a corresponding form
        /// is displayed to add a new database entry
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (index == 1) // Packages 
            {
                // Adding new Packages form object and setting for an 'Add' form
                frmAddModifyPackage packageForm = new frmAddModifyPackage();
                packageForm.package = null;
                packageForm.isAdd = true;

                // Display form (Dialog box)
                DialogResult result = packageForm.ShowDialog();

                if (result == DialogResult.OK) // Once form data is confirmed
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
                    {
                        try
                        {
                            // Add packages object from form to Packages table
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
            else if (index == 2) // Products
            {
                // Adding new Products form object and setting for an 'Add' form
                frmAddModifyProduct productForm = new frmAddModifyProduct();
                productForm.product = null;
                productForm.isAdd = true;

                // Display form (Dialog box)
                DialogResult result = productForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // open database connection
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
            else if (index == 3) // Suppliers
            {
                // Adding new Suppliers form object and setting for an 'Add' form
                frmAddModifySupplier supplierForm = new frmAddModifySupplier();
                supplierForm.supplier = null;
                supplierForm.isAdd = true;

                // Display form (Dialog box)
                DialogResult result = supplierForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
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

                                // update supplierIds in Product Supplier Objects to match current
                                // supplierId
                                foreach (ProductsSuppliers ps in supplierForm.prodsuppliers)
                                {
                                    ps.SupplierId = supplierForm.supplier.SupplierId;
                                }
                            }
                        }

                        try
                        {
                            // Add new supplier to Suppliers table
                            db.Suppliers.Add(supplierForm.supplier);

                            if (supplierForm.prodsuppliers.Count > 0) // Products were added to supplier
                            {
                                // Add all associated product supplier entries
                                foreach (ProductsSuppliers ps in supplierForm.prodsuppliers)
                                {
                                    db.ProductsSuppliers.Add(ps);
                                }
                            }

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
        /// Depending on which option is selected in ComboBox, a corresponding form
        /// is displayed to modify a new database entry
        /// </summary>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (index == 1) // PACKAGES TABLE
            {
                // Adding new package form object and selected package object
                // Setting as a 'modify' form
                frmAddModifyPackage packageForm = new frmAddModifyPackage();
                Packages selectedPackage;
                packageForm.isAdd = false;


                using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
                {
                    // String value for primary key column of selected row in data grid view
                    string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigning query result as the current selected product
                    selectedPackage = db.Packages.SingleOrDefault(p => p.PkgName == value);
                } // close

                // Setting form package object to the current selected package
                packageForm.package = selectedPackage;


                // Display form (Dialog box)
                DialogResult result = packageForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
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
                // Adding new product form object and selected product object
                // Setting as a 'modify' form
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


                // Setting form product object to the current selected product
                productForm.product = selectedProduct;

                // Display form (Dialog box)
                DialogResult result = productForm.ShowDialog();


                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // Open db connection
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
                // Adding new supplier form object, selected supplier object, and product supplier list
                // Setting as a 'modify' form
                frmAddModifySupplier supplierForm = new frmAddModifySupplier();
                Suppliers selectedSupplier;
                List<ProductsSuppliers> prodSuppliers;
                supplierForm.isAdd = false;

                using (TravelExpertsContext db = new TravelExpertsContext()) // Opens database connection
                {
                    //String value for primary key column of selected row in data grid view
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    // Assigns query result as the current selected product
                    selectedSupplier = db.Suppliers.Find(value);

                    // Find all product suppliers with selected id and add to list
                    prodSuppliers = db.ProductsSuppliers.Where(ps => ps.SupplierId == selectedSupplier.SupplierId)
                        .ToList();
                } // close

                // pass suppliers and product suppliers to form
                supplierForm.supplier = selectedSupplier;
                supplierForm.prodsuppliers = prodSuppliers;

                DialogResult result = supplierForm.ShowDialog();


                if (result == DialogResult.OK)
                {
                    using (TravelExpertsContext db = new TravelExpertsContext()) // Open db connection
                    {
                        try
                        {
                            db.Suppliers.Update(supplierForm.supplier);

                            // This last part is for removing product supplier entries that 
                            // do not have a ProductId that matches an Id of any products on
                            // the current supplier list of associated product names

                            // *********************************************************************

                            // Get the list of current product suppliers entries that contain
                            // current supplierId

                            List<ProductsSuppliers> prodSuppliersToRemove = db.ProductsSuppliers
                                    .Where(ps => ps.SupplierId == supplierForm.supplierID).ToList();

                                // filter out all product supplier entries that contain an id for
                                // products that are on the product names list in the supplier form
                                // object (current record of associated products)

                                foreach (string p in supplierForm.prodNames)
                                {
                                    prodSuppliersToRemove.Remove(
                                        prodSuppliersToRemove.SingleOrDefault(pstr => 
                                            supplierForm.GetProductName((int)pstr.ProductId) == p)
                                    );
                                }

                                // If count is greater than 0, this list contains product suppliers
                                // that need to be removed from the ProductSuppliers table

                                if (prodSuppliersToRemove.Count > 0)
                                {
                                    foreach (ProductsSuppliers ps in prodSuppliersToRemove)
                                    {
                                        db.ProductsSuppliers.Remove(ps);
                                    }
                                }

                            // Save all changes to database

                            db.SaveChanges();

                            // Display updated list of Suppliers

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
        /// Event that handles delete functionality when a line is selected in the DataGridView
        /// of the current table selection
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (index == 1) // Packages
            {
                Packages selectedPackage; // Object for current selection

                using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
                {
                    // String value for primary key column of selected row in data grid view
                    string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                    // Assign query result as the current selected product
                    selectedPackage = db.Packages.SingleOrDefault(p => p.PkgName == value);
                } // close


                if (selectedPackage != null) // If a package is selected
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
                else // No package is selected
                {
                    MessageBox.Show("Package needs to be selected", "Error deleting package",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (index == 2) // Products
            {
                Products selectedProduct; // Object for current selection
                 
                using (TravelExpertsContext db = new TravelExpertsContext()) // open database connection
                {
                    // integer value for primary key column of selected row in data grid view
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    // Find selected product using value(id)
                    selectedProduct = db.Products.Find(value); 
                }

                if (selectedProduct != null) // product is selected
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
                else // No product is selected
                {
                    MessageBox.Show("Product needs to be selected", "Error deleting product",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (index == 3) // Supplier
            {
                Suppliers selectedSupplier; // Object for current selection

                using (TravelExpertsContext db = new TravelExpertsContext()) // Open database connection
                {
                    // integer value for primary key column of selected row in data grid view
                    int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                    // Find selected supplier using value(id)
                    selectedSupplier = db.Suppliers.Find(value);
                }

                if (selectedSupplier != null) // Supplier is selected
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
                else // No supplier is selected
                {
                    MessageBox.Show("Supplier needs to be selected", "Error deleting supplier",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Member Methods ========================================================================

        /// <summary>
        /// Void method that displays all Packages table entries in the DataGridView
        /// </summary>
        private void DisplayPackages()
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // open db connection
            {
                // Getting ordered list of packages from Packages table
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

                // Setting Data Grid View data source
                dgvMain.DataSource = packages;

                // Setting header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[2].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[3].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[4].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[5].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Setting Column width
                dgvMain.Columns[0].Width = 225; // Name
                dgvMain.Columns[1].Width = 150; // Start Date
                dgvMain.Columns[2].Width = 150; // End Date
                dgvMain.Columns[3].Width = 375; // Description
                dgvMain.Columns[4].Width = 150; // Base Price
                dgvMain.Columns[5].Width = 240; // Agency Commission

                // Renaming columns           
                dgvMain.Columns[0].HeaderText = "Package Name";
                dgvMain.Columns[1].HeaderText = "Start Date";
                dgvMain.Columns[2].HeaderText = "End Date";
                dgvMain.Columns[3].HeaderText = "Description";
                dgvMain.Columns[4].HeaderText = "Base Price";
                dgvMain.Columns[5].HeaderText = "Agency Commission";

                // Formatting prices as currency
                dgvMain.Columns[4].DefaultCellStyle.Format = "c";
                dgvMain.Columns[5].DefaultCellStyle.Format = "c";

            }
        }


        /// <summary>
        /// Void method that displays all Products table entries in the DataGridView
        /// </summary>
        private void DisplayProducts()
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // open database connection
            {
                // Getting ordered list of products from Packages table
                var products = db.Products.OrderBy(p => p.ProductId)
                    .Select(p => new { p.ProductId, p.ProdName })
                    .ToList();

                // Settin Data Grid View data source
                dgvMain.DataSource = products;

                // Setting header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Setting Column width
                dgvMain.Columns[0].Width = 140;
                dgvMain.Columns[1].Width = 986;

                // Renaming columns
                dgvMain.Columns[0].HeaderText = "Product Id";
                dgvMain.Columns[1].HeaderText = "Product Name";
            }
        }


        /// <summary>
        /// Void method that displays all Suppliers table entries in the DataGridView
        /// </summary>
        private void DisplaySuppliers()
        {
            using (TravelExpertsContext db = new TravelExpertsContext()) // open db connection
            {
                // Getting ordered list of suppliers from Suppliers table
                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId)
                    .Select(s => new { s.SupplierId, s.SupName })
                    .ToList();

                // Setting Data Grid View data source
                dgvMain.DataSource = suppliers;

                // Setting header and individual column font family and font size
                dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe", 12, FontStyle.Bold);

                dgvMain.Columns[0].DefaultCellStyle.Font = new Font("Segoe", 12);
                dgvMain.Columns[1].DefaultCellStyle.Font = new Font("Segoe", 12);

                // Setting Column width
                dgvMain.Columns[0].Width = 150;
                dgvMain.Columns[1].Width = 955;

                // Renaming columns
                dgvMain.Columns[0].HeaderText = "Supplier Id";
                dgvMain.Columns[1].HeaderText = "Supplier Name";
            }
        }


        /// <summary>
        /// Enables form buttons
        /// </summary>
        private void EnableButtons()
        {
            btnAdd.Enabled = true;
            btnModify.Enabled = true;
            btnDelete.Enabled = true;
        }


        /// <summary>
        /// Disables form buttons
        /// </summary>
        private void DisableButtons()
        {
            btnAdd.Enabled = false;
            btnModify.Enabled = false;
            btnDelete.Enabled = false;
        }

       
    } // class
} // namespace
