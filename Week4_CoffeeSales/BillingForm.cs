/*
 * Name: Luis Acevedo
 * Date: Sept., 2014
 * Purpose: For a coffee shop, this project calculates the amount due based on the user selection.
 * It accumulates summary information that can be displayed as required.
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Week5_CoffeeSales
{
    public partial class BillingForm : Form
    {
        //module level varaiables - objects default to null and numbers default to 0
        private RadioButton selectedRadioButton = null;
        decimal subTotalDecimal = 0, totalDecimal = 0, grandTotalDecimal = 0;
        int customerCountInteger = 0;
        //module level constants
        const decimal TAX_RATE_Decimal = 0.15m;
        const decimal CAPPUCCINO_PRICE_Decimal = 2m;
        const decimal ESPRESSO_PRICE_Decimal = 2.25m;
        const decimal LATTE_PRICE_Decimal = 1.75m;
        const decimal ICED_PRICE_Decimal = 2.50m;


        public BillingForm()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            //closes the application
            this.Close();
        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            //default settings when the form loads
            quantityTextBox.Select();
            cappuccinoRadioButton.Checked = false;
            clearButton.Enabled = false;
            newOrderToolStripMenuItem.Enabled = false;
            clearItemToolStripMenuItem.Enabled = false;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            //calculate the amount due for an order
            //maintain summary information
            decimal priceDecimal, taxDecimal, itemAmountDecimal;

            //Check if user is giving the right value
            try
            {
                itemAmountDecimal = int.Parse(quantityTextBox.Text);
                
                //check if the value is greater than zero
                if (itemAmountDecimal > 0)
                {
                    //check the coffee selected
                    if (selectedRadioButton != null)
                    {
                        switch(selectedRadioButton.Name){
                            case "cappuccinoRadioButton":
                                priceDecimal = CAPPUCCINO_PRICE_Decimal;
                                break;
                            case "espressoRadioButton":
                                priceDecimal = ESPRESSO_PRICE_Decimal;
                                break;
                            case "latteRadioButton":
                                priceDecimal = LATTE_PRICE_Decimal;
                                break;
                            default:
                                priceDecimal = ICED_PRICE_Decimal;
                                break;
                        }

                        //calculate the amount
                        itemAmountDecimal = priceDecimal * decimal.Parse(quantityTextBox.Text);

                        subTotalDecimal += itemAmountDecimal;
                        if (taxCheckBox.Checked)
                        {
                            taxDecimal = subTotalDecimal * TAX_RATE_Decimal;
                        }
                        else
                        {
                            taxDecimal = 0;
                        }

                        totalDecimal = subTotalDecimal + taxDecimal;

                        //format and display results
                        itemAmountTextBox.Text = itemAmountDecimal.ToString("C");
                        taxTextBox.Text = taxDecimal.ToString("C");
                        subTotalTextBox.Text = subTotalDecimal.ToString("C");
                        totalTextBox.Text = totalDecimal.ToString("C");

                        //set the other controls
                        clearButton.Enabled = true;
                        taxCheckBox.Enabled = false;
                        newOrderToolStripMenuItem.Enabled = true;
                        clearItemToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Please select a coffee flavor", "Flavour not selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Value must the greater than Zero", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    quantityTextBox.SelectAll();
                    quantityTextBox.Focus();
                }

            }
            catch (FormatException exception)
            {
                if (quantityTextBox.Text == "")
                {
                    MessageBox.Show("Must provide a value", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Quantity MUST be a positive whole number", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                quantityTextBox.SelectAll();
                quantityTextBox.Focus();
            }
            catch (OverflowException exception)
            {
                MessageBox.Show("Out of range", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                quantityTextBox.SelectAll();
                quantityTextBox.Focus();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error:" + exception.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                quantityTextBox.SelectAll();
                quantityTextBox.Focus();
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            //This makes the selected radiobutton "selectedRadioButton" into the button selected at the form
            selectedRadioButton = (RadioButton)sender;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            //Clear the current item
            if (selectedRadioButton != null)
            {
                selectedRadioButton.Checked = false;
                selectedRadioButton = null;
            }

            itemAmountTextBox.Clear();
            quantityTextBox.Clear();
            quantityTextBox.Focus();
        }

        private void newOrderButton_Click(object sender, EventArgs e)
        {
            //clear the order after confirming from the user
            DialogResult responseDialogResult;

            //Show a dialog to confirm the selection
            responseDialogResult = MessageBox.Show("Clear the current order?", "Clear Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);

            if (responseDialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                clearButton_Click(sender, e);
                subTotalTextBox.Clear();
                taxTextBox.Clear();
                totalTextBox.Clear();
                taxCheckBox.Enabled = true;
                clearButton.Enabled = false;
                newOrderToolStripMenuItem.Enabled = false;
                clearItemToolStripMenuItem.Enabled = false;
            }

            //add the totals for summary information
            if (subTotalDecimal > 0)
            {
                grandTotalDecimal += totalDecimal;
                customerCountInteger++;

                //reset the totals for the new order
                subTotalDecimal = 0;
                totalDecimal = 0;
                taxCheckBox.Checked = false;
            }
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            //show the summary information to the user
            decimal averageDecimal;
            string messageString;

            //customer is in the middle of placing an order
            if (totalDecimal != 0)
            {
                newOrderButton_Click(sender, e);
            }

            if (customerCountInteger > 0)
            {
                averageDecimal = grandTotalDecimal / customerCountInteger;
                messageString = "Number of orders: " + customerCountInteger.ToString() + "\nTotal Sales: " + grandTotalDecimal.ToString("c") + "\nAverage Sales: " + averageDecimal.ToString("c");

                MessageBox.Show(messageString, "Sales Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No sales summary information to show", "Sales Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show a message box with application details
            String messageString;
            messageString = "R 'n R Billing application \nProgrammed by: Luis Acevedo";
            MessageBox.Show(messageString, "About Us", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Allow the user to change the font of the output
            //Save the current form
            Font currentFont = fontDialog1.Font;

            //Show the font dialog box
            fontDialog1.ShowDialog();
            currentFont = fontDialog1.Font;

            //Apply the user font selected
            subTotalTextBox.Font = currentFont;
            taxTextBox.Font = currentFont;
            totalTextBox.Font = currentFont;
        }

        private void colourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allow the user to change the color of the output
            Color currentColor = colorDialog1.Color;
            changeColor(out currentColor);

            //Assign the new color to the text
            subTotalTextBox.ForeColor = currentColor;
            taxTextBox.ForeColor = currentColor;
            totalTextBox.ForeColor = currentColor;
        }

        //Using OUT parameter
        private void changeColor(out Color color){
            colorDialog1.ShowDialog();
            color = colorDialog1.Color;
        }

        //Using RETURN parameter
        private Color changeColor(Color color)
        {
            colorDialog1.ShowDialog();
            return colorDialog1.Color;
        }

        /* Useless Bullcrap */
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            //nothing
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //nothing
        }
        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            //nothing
        }
        /* Useless Bullcrap */

    } 
}
