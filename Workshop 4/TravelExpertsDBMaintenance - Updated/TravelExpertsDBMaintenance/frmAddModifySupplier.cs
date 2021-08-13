using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TravelExpertsBLL;
using TravelExpertsData.Models;

namespace TravelExpertsDBMaintenance
{
    public partial class frmAddModifySupplier : Form
    {
        public bool isAdd;
        public Suppliers supplier;
        public SupplierContacts supplierContact;
        public List<ProductsSuppliers> prodsuppliers;

        public int supplierID; // current supplier id if modify (This was used for an experiment)
                               // Will likely remove

        // Record of product names that are being displayed in the listbox of this form
        public List<string> prodNames = new List<string>(); 

        public frmAddModifySupplier()
        {
            InitializeComponent();
        }

        private void frmAddModifySupplier_Load(object sender, EventArgs e)
        {
            FillProductsComboBox();

            if (this.isAdd)
            {
                supplier = new Suppliers();
                prodsuppliers = new List<ProductsSuppliers>();

                this.Text = "Add Supplier";               
            }
            else
            {
                this.Text = "Modify Supplier";
                lstProductList.Items.Clear();

                // Loop through product supplier object that was passed to this form
                // Retrieve the name of product(s) from that object (all table entries)
                // Add them to record of product names

                foreach (ProductsSuppliers ps in prodsuppliers)
                {
                    string name = ProductManager.GetProductName((int)ps.ProductId);
                    prodNames.Add(name);
                }

                if (supplier == null) // Supplier is selected to modify
                {
                    MessageBox.Show("There is no supplier selected", "Error has occured",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Close();
                }

                // Add associated product names to list box for display
                foreach (string name in prodNames)
                {
                    lstProductList.Items.Add(name);
                }

                //using (TravelExpertsContext db = new TravelExpertsContext()) // open database connection
                //{
                //    // populate product list with associated products
                //    var productList = db.Products.Join(db.ProductsSuppliers,
                //                        p => p.ProductId,
                //                        ps => ps.ProductId,
                //                        (p, ps) => new { ProdName = p.ProdName, SupID = ps.SupplierId })
                //        .Where(x => x.SupID == supplier.SupplierId)
                //        .ToList();

                //    // add associated product names to list box for display
                //    foreach (var p in productList)
                //    {
                //        lstProductList.Items.Add(p.ProdName);
                //    }
                //}

                // Populate supplier name field with selected supplier name
                txtName.Text = supplier.SupName;

                // *** As mentioned above, this step will likely be removed ***
                this.supplierID = supplier.SupplierId;

                // If associated product list is empty, disable 'Remove Product' button
                // Otherwise, enable button
                EnableOrDisableRemoveProduct();
            }
        }


        // Member Event Handlers ===============================================================

        /// <summary>
        /// Event handler for saving new supplier or updating selected supplier
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtName)) // Supplier name field is not blank
            {
                List<string> currentProds; // Being used in modify part of this method
                                           // **May remove from code depending on how it's refactored**


                if (this.isAdd) // Adding supplier
                {
                    supplier = new Suppliers(); // Creating new instance of supplier 
                    supplier.SupplierId = 1; // Default value

                    if (prodNames.Count > 0) // If product list is populated
                    {
                        foreach (string p in prodNames) // For each product name in the list box...
                        {
                            // Create a product supplier object
                            ProductsSuppliers ps = new ProductsSuppliers();

                            // Get product id based on product name
                            int prodID = ProductManager.GetProductId(p);

                            // Add productId and current supplierId to product supplier object
                            ps.ProductId = prodID;
                            ps.SupplierId = supplier.SupplierId;
                                            
                            prodsuppliers.Add(ps);
                        }
                    }

                    // Creating default supplier contact entry, which is set to be blank until
                    // user updates it

                    supplierContact = new SupplierContacts
                    {
                        SupplierContactId = 1,
                        SupConFirstName = null,
                        SupConLastName = null,
                        SupConCompany = null,
                        SupConAddress = null,
                        SupConCity = null,
                        SupConProv = null,
                        SupConPostal = null,
                        SupConCountry = null,
                        SupConBusPhone = null,
                        SupConFax = null,
                        SupConEmail = null,
                        SupConUrl = null,
                        AffiliationId = null,
                        SupplierId = null
                    };
                    
                    // *** Items are added to database in main form code ***
;               }
                else // Modifying supplier
                {                
                    currentProds = new List<string>(); 

                    foreach (string p in prodNames)
                    {
                        foreach (ProductsSuppliers ps in prodsuppliers)
                        {
                            if (p == ProductManager.GetProductName((int)ps.ProductId))
                            {
                                currentProds.Add(p);
                            }
                        }
                    }

                    // removing prodNames that haven't been added to or deleted from
                    // the list box that represent the current product suppliers entries
                    // in the Product Suppliers table

                    foreach (string c in currentProds)
                    {
                        prodNames.Remove(c);
                    }

                    // Add products that don't current exist in Product Suppliers table
                    
                    foreach (string p in prodNames)
                    {
                        int prodID = ProductManager.GetProductId(p);

                        ProductsSuppliers newProdSuppliers = new ProductsSuppliers
                        {
                            ProductId = prodID,
                            SupplierId = this.supplierID
                        };

                        ProductSupplierManager.Add(newProdSuppliers);
                    }

                    // Rejoin all product names to prodName List
                    foreach (string c in currentProds)
                    {
                        prodNames.Add(c);
                    }
                }              
                
                supplier.SupName = txtName.Text;
               
                // Create product supplier object entries and add to list             

                this.DialogResult = DialogResult.OK;
            }
        }


        /// <summary>
        /// Event handler for button that adds associated products to supplier
        /// </summary>
        private void btnAddProd_Click(object sender, EventArgs e)
        {
            string prodName = cboSelect.GetItemText(cboSelect.SelectedItem);

            foreach (string p in prodNames) // check if same product is added
            {
                if (p == prodName)
                {
                    MessageBox.Show("Product already added.", "Error adding product", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // ends execution of code
                }
            }

            // Add name to product name record
            prodNames.Add(prodName); 

            // Clear current list contents
            lstProductList.Items.Clear(); 

            // Update with current list of product names
            foreach (string p in prodNames)
            {
                lstProductList.Items.Add(p);
            }

            EnableOrDisableRemoveProduct();         
        }


        /// <summary>
        /// Event handler for button that removes associated products from supplier
        /// </summary>
        private void btnRemoveProd_Click(object sender, EventArgs e)
        {
            string prodName = lstProductList.SelectedItem.ToString(); // get selected product to remove
            string prodToRemove = null; 

            // Finding matching product name to remove within products name record
            // and assign to its own variable

            foreach (string p in prodNames)
            {
                if (p == prodName)
                {
                    prodToRemove = p;
                }
            }

            //*** Perhaps the about code could be improved as there is likely some redundancy ***
            // However, my approach was to ensure I got an exact reference of the name in the
            // prodNames list

            // remove name from list
            prodNames.Remove(prodToRemove);

            lstProductList.Items.Clear();

            // update list box to reflect current record of product names
            foreach (string p in prodNames)
            {
                lstProductList.Items.Add(p);
            }

            EnableOrDisableRemoveProduct();
        }


        // Member Methods ========================================================================

        private void FillProductsComboBox()
        {
            List<Products> products = ProductManager.GetAll();

            cboSelect.DataSource = products;
            cboSelect.ValueMember = "ProductId";
            cboSelect.DisplayMember = "ProdName";
          
        }


        /// <summary>
        /// Disables 'Remove Product' button if there are no products in the form listbox, otherwise
        /// it is enabled
        /// </summary>
        private void EnableOrDisableRemoveProduct()
        {
            if (lstProductList.Items.Count > 0)
            {
                btnRemoveProd.Enabled = true;
            }
            else
            {
                btnRemoveProd.Enabled = false;
            }
        }
    }
}
