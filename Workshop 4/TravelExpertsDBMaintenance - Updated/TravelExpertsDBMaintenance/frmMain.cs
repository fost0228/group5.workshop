using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TravelExpertsBLL;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmMain : Form
    {
        int index; // Index number that corresponds to ComboBox selection
        string username; // Current user logged in

        public string Username { get; set; }

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

            // ==============

            //using (TravelExpertsContext db = new TravelExpertsContext())
            //{
            //    Agents agent = db.Agents.SingleOrDefault(a => a.AgtUsername)
            //}
        }


        // Member Event Handlers ======================================================================

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Dialog box to confirm if user reall does want to exit the application
            DialogResult confirmation = MessageBox.Show("Are you sure you want to exit?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation == DialogResult.Yes) // User chose yes, so application will close
            {
                Application.Exit();
            }
            
            // User chose no, so dialog box will close and application will resume
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
                    try
                    {
                        // Add packages object from form to Packages table
                        PackageManager.Add(packageForm.package);
                           
                        DisplayPackages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding package", ex.GetType().ToString(),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    try
                    {
                        ProductManager.Add(productForm.product);
                         
                        DisplayProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding product", ex.GetType().ToString(),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // ASSIGN ID to SUPPLIER ======================================================

                    // Find all supplier IDs
                    List<int> supplierIds = SupplierManager.GetListOfIDs();

                    // Auto increment supplierId based on available IDs
                    foreach (var supId in supplierIds)
                    {
                        if (supplierForm.supplier.SupplierId == supId)
                        {
                            supplierForm.supplier.SupplierId = supId;
                            supplierForm.supplier.SupplierId += 1;

                            // Update supplierIds in Product Supplier Objects to match current
                            // SupplierId
                            foreach (ProductsSuppliers ps in supplierForm.prodsuppliers)
                            {
                                ps.SupplierId = supplierForm.supplier.SupplierId;
                            }
                        }
                    }

                    // PASS SUPPLIER ID to SUPPLIER CONTACT ========================================

                    // Add current supplier Id to supplier contact object
                    supplierForm.supplierContact.SupplierId = supplierForm.supplier.SupplierId;

                    // ASSIGN ID TO SUPPLIER CONTACT ===============================================

                    // Find all supplier contact IDs
                    List<int> supplierContactIds = SupplierContactManager.GetListOfIDs();

                    // Auto increment supplierContactId based on available IDs
                    foreach (var supConId in supplierContactIds)
                    {
                        if (supplierForm.supplierContact.SupplierContactId == supConId)
                        {
                            supplierForm.supplierContact.SupplierContactId = supConId;
                            supplierForm.supplierContact.SupplierContactId += 1;
                        }
                    }

                    // ==============================================================================

                    try
                    {
                        // Add new supplier to Suppliers table
                        SupplierManager.Add(supplierForm.supplier);

                        if (supplierForm.prodsuppliers.Count > 0) // Products were added to supplier
                        {
                            // Add all associated product supplier entries
                            foreach (ProductsSuppliers ps in supplierForm.prodsuppliers)
                            {
                                ProductSupplierManager.Add(ps);
                            }
                        }

                        // Add default supplier contact entry to SupplierContacts table
                        SupplierContactManager.Add(supplierForm.supplierContact);

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
 
                // String value for primary key column of selected row in data grid view
                string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                // Assigning query result as the current selected product
                selectedPackage = PackageManager.Find(value);              

                // Setting form package object to the current selected package
                packageForm.package = selectedPackage;


                // Display form (Dialog box)
                DialogResult result = packageForm.ShowDialog();

                if (result == DialogResult.OK)
                {              
                    try
                    {
                        PackageManager.Modify(packageForm.package);                       

                        DisplayPackages();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating Packages table", ex.GetType().ToString(),
                            MessageBoxButtons.OK, MessageBoxIcon.Error); 
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
                
                //String value for primary key column of selected row in data grid view
                int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                // Assigns query result as the current selected product
                selectedProduct = ProductManager.Find(value);

                // Setting form product object to the current selected product
                productForm.product = selectedProduct;

                // Display form (Dialog box)
                DialogResult result = productForm.ShowDialog();


                if (result == DialogResult.OK)
                {                  
                    try
                    {
                        ProductManager.Modify(productForm.product);
                        
                        DisplayProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating Products table", ex.GetType().ToString(),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                //String value for primary key column of selected row in data grid view
                int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                // Assigns query result as the current selected product
                selectedSupplier = SupplierManager.Find(value);

                // Find all product suppliers with selected id and add to list
                prodSuppliers = ProductSupplierManager.GetListBySupplierID(selectedSupplier.SupplierId);

                // Pass suppliers and product suppliers to form
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
                                            ProductManager.GetProductName((int)pstr.ProductId) == p)
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
                    } // close
                    
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

                // String value for primary key column of selected row in data grid view
                string value = (string)dgvMain.CurrentRow.Cells[0].Value;

                // Assign query result as the current selected product
                selectedPackage = PackageManager.Find(value);


                if (selectedPackage != null) // If a package is selected
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedPackage.PkgName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {                        
                            PackageManager.Delete(selectedPackage);                         

                            DisplayPackages();     
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
              
                // Integer value for primary key column of selected row in data grid view
                int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                // Find selected product using value(id)
                selectedProduct = ProductManager.Find(value); 
               

                if (selectedProduct != null) // product is selected
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedProduct.ProdName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {
                            ProductManager.Delete(selectedProduct);

                            DisplayProducts();                       
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
               
                // Integer value for primary key column of selected row in data grid view
                int value = (int)dgvMain.CurrentRow.Cells[0].Value;

                // Find selected supplier using value(id)
                selectedSupplier = SupplierManager.Find(value);


                if (selectedSupplier != null) // Supplier is selected
                {
                    DialogResult answer =
                        MessageBox.Show($"Are you sure you want to delete \"{selectedSupplier.SupName}\"?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (answer == DialogResult.Yes)
                    {
                        try
                        {  
                            SupplierManager.Delete(selectedSupplier);                               

                            DisplaySuppliers();      
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

        // Member Methods ============================================================================

        /// <summary>
        /// Void method that displays all Packages table entries in the DataGridView
        /// </summary>
        private void DisplayPackages()
        {
            // Setting Data Grid View data source
            dgvMain.DataSource = PackageManager.GetOrderedList();

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


        /// <summary>
        /// Void method that displays all Products table entries in the DataGridView
        /// </summary>
        private void DisplayProducts()
        {    
            // Settin Data Grid View data source
            dgvMain.DataSource = ProductManager.GetOrderedList();

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


        /// <summary>
        /// Void method that displays all Suppliers table entries in the DataGridView
        /// </summary>
        private void DisplaySuppliers()
        {
            // Setting Data Grid View data source
            dgvMain.DataSource = SupplierManager.GetOrderedList();

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
