using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjLiveProject
{
    public partial class frmNewPurchase : Form
    {
        MySqlConnection dbConnection = ConnectionClass.dbConnection;
        string customer="", phone = "", location = "", transactionId = "", transactionType = "", quantity = "", amount = "", tax = "", item= "", total="";

        private void txtTax_TextChanged(object sender, EventArgs e)
        {
            string dummy = txtTax.Text;
            Regex pattern = new Regex(@"^[\d]+$");
            string replaceText = Regex.Replace(dummy, @"[^\d]+", "");
            txtTax.Text = replaceText;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void ResetControls()
        {
            foreach (Control item in panel1.Controls)
            {
                if (item.GetType() == new TextBox().GetType())
                {
                    item.ResetText();
                }
                if (item.GetType() == new NumericUpDown().GetType())
                {
                    item.ResetText();
                }
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            string dummy = txtAmount.Text;
            Regex pattern = new Regex(@"^[\d]+$");
            string replaceText = Regex.Replace(dummy, @"[^\d]+", "");
            txtAmount.Text = replaceText;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string dummy = txtPhone.Text;
            Regex pattern = new Regex(@"^[\d]+$");
            string replaceText = Regex.Replace(dummy, @"[^\d]+", "");
            txtPhone.Text = replaceText;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Form1().Show();
        }

        public frmNewPurchase()
        {
            InitializeComponent();
        }

        private void frmNewPurchase_Load(object sender, EventArgs e)
        { 
            cboTransactionType.Items.Insert(0, "--Please Select--");
            cboItem.Items.Insert(0, "--Please Select--");

            //PopulateItemCombo
            PopulateItemsCombo();
            PopulateTransactionType();
        }

        private void PopulateTransactionType()
        {
            string dbQuery = "SELECT * FROM transaction_type";
            MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);
            dbCommand.CommandTimeout = 60;
            List<string> listType;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {

                    listType = new List<string>();
                    string tempType = "";

                    while (myDataReader.Read())
                    {
                        tempType = myDataReader.GetString(1);
                        this.cboTransactionType.Items.Add(tempType);
                    }
                    myDataReader.Close();
                    this.cboTransactionType.AutoCompleteSource = AutoCompleteSource.ListItems;
                    dbConnection.Close();
                }
                else if (!myDataReader.HasRows)
                {
                    MessageBox.Show("No Transaction type to show");
                }
                dbConnection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
        }

        private void PopulateItemsCombo()
        {
            string dbQuery = "SELECT * FROM item";
            MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);
            dbCommand.CommandTimeout = 60;
            List<string> listItem;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {

                    listItem = new List<string>();
                    string tempItem = "";

                    while (myDataReader.Read())
                    {
                        tempItem = myDataReader.GetString(1);
                        this.cboItem.Items.Add(tempItem);
                    }
                    myDataReader.Close();
                    this.cboItem.AutoCompleteSource = AutoCompleteSource.ListItems;
                    dbConnection.Close();
                }
                else if (!myDataReader.HasRows)
                {
                    MessageBox.Show("No item to show");
                }
                dbConnection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            addnewPurchase();
        }

        private void addnewPurchase()
        {
            customer = txtCustomerName.Text.Trim();
            phone = txtPhone.Text.Trim();
            location = txtLocation.Text.Trim();
            transactionType = cboTransactionType.SelectedItem.ToString().Trim();
            quantity = numQuantity.Value.ToString().Trim();
            amount = txtAmount.Text.Trim();
            tax = txtTax.Text.Trim();
            item = cboItem.SelectedItem.ToString().Trim();
            
            
            //Check for Epty Fields
            if (customer != "" && phone != "" && transactionType != "" && quantity != "" && amount != "" && item != "")
            {
                

                total = Convert.ToDecimal((Convert.ToDecimal(amount) + Convert.ToDecimal(tax))).ToString();

            string lastInsertIDQuery = "";
            //Get ID of last row
            int tempID = 0;

            tempID = LastInsertedIDClass.GetLastID();
            if (tempID == 0)
            {
                tempID += 1;
                lastInsertIDQuery = "INSERT INTO `last_id` (`last_id`) VALUES ('" + tempID + "')";
            }
            else
            {
                tempID += 1;
                lastInsertIDQuery = "UPDATE `last_id` SET `last_id` = '" + tempID + "'";
            }

                //Trans id
                string[] arrAlph = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string finalString = "";
                Random rand = new Random();
                for (int i = 0; i < 7; i++)
                {
                    string tempStr = "";
                    int alpha = rand.Next(1, arrAlph.Length);
                    tempStr = arrAlph[alpha];
                    finalString += tempStr;
                }
                transactionId = tempID + finalString;

                MySqlCommand lastIdCommand = new MySqlCommand(lastInsertIDQuery, dbConnection);
            lastIdCommand.CommandTimeout = 60;

            string insertQuery = "INSERT INTO `purchase` (`transaction_id`,`customer`,`phone`,`location`,`transaction_type`,`item`,`quantity`,`amount`,`tax`,`total`) VALUES('"+transactionId+"','"+customer+ "','"+phone+ "','"+location+ "','"+transactionType+ "','"+item+ "','"+quantity+ "','"+amount+ "','"+tax+ "','"+total+"')";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, dbConnection);
            insertCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                //Update Last ID
                if (lastIdCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Last ID Updated");
                }
                else
                    MessageBox.Show("Error updating Last ID");

                //Save New Data
                if (insertCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Data added successfully");
                        ResetControls();
                }
                else
                    MessageBox.Show("Error adding data");
                dbConnection.Close();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
            }
            else
            {
                MessageBox.Show("Please fill all mandatory fields");
            }
        }
    }
}
