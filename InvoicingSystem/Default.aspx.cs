using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using InvoicingSystem.Models;

namespace InvoicingSystem
{
    public partial class Default : System.Web.UI.Page
    {
        public List<Invoice> InvoiceList = new List<Invoice>();

        //Initial Page Load method
        protected void Page_Load(object sender, EventArgs e)
        {
            InvoicingSystemContext context = new InvoicingSystemContext();
            try
            {
                if (!IsPostBack)
                {
                    InvoiceList = context.Invoices.ToList();
                }
                dataSection.InnerHtml = BuildHTMLTable(InvoiceList);
            }
            catch (Exception)
            {
                Response.Redirect("~/Error.aspx");
            }

        }

        //Builds the HTML Table from the Invoice List to display
        private string BuildHTMLTable(List<Invoice> InvoiceList)
        {
            string htmltable = "";
            htmltable += "<table id='invoicetable' class='table cell-border'>" + 
                "<thead>" + 
                "<tr>" + 
                "<th></th>" + 
                "<th>Invoice ID</th>" + 
                "<th>Client</th>" + 
                "<th>Invoice Amount</th>" + 
                "<th>Invoice Amount with VAT</th>" + 
                "<th>VAT Rate</th>" + 
                "<th>Invoice Status</th>" + 
                "<th>Invoice Date</th>" + 
                "<th>Pay</th>" + 
                "</tr>" + 
                "</thead>" + 
                "<tbody>";
            foreach (Invoice inv in InvoiceList)
            {
                htmltable +=
                    "<tr>" + 
                    "<td><input type='submit' value='Details' onclick = 'Details(this);return false;' data-value ='" + inv.Id + "' /></td>" + 
                    "<td>" + inv.Id + "</td>" + 
                    "<td>" + inv.Client + "</td>" + 
                    "<td>" + inv.Invoice_Amount + "</td>" + 
                    "<td>" + inv.Invoice_Amount_Plus_VAT + "</td>" + 
                    "<td>" + inv.VAT_Rate + "</td>" +
                    "<td>" + inv.Invoice_Status.ToUpper() + "</td>" +
                    "<td>" + inv.Invoice_Date?.Date.ToShortDateString() + "</td>";
                if (inv.Invoice_Status == Invoice_Status_Enum.unpaid.ToString())
                {
                    htmltable += "<td><input type='submit' value='Pay Invoice' onclick = 'PayInvoice(this);return false;' data-value ='" + inv.Id + "' /></td>";
                }
                else
                {
                    htmltable += "<td><input type='submit' value='Unpay Invoice' onclick = 'PayInvoice(this);return false;' data-value ='" + inv.Id + "' /></td>";
                }
                htmltable += "</tr>";

            }
            htmltable += "</tbody>" + 
                "</table>";
            return htmltable;
        }

        //Builds the HTML Table from the InvoiceItem List to display
        private string BuildHTMLTable(List<Invoice_Item> InvoiceItemList)
        {
            string htmltable = "";
            htmltable += "<table class='table table-sm'>" + 
                "<thead>" + 
                "<tr>" + 
                "<th>Item Name</th>" + 
                "<th>Amount</th>" + 
                "</tr>" +
                "</thead>" + 
                "<tbody>";
            foreach (Invoice_Item invitem in InvoiceItemList)
            {
                htmltable +=
                    "<tr>" +
                    "<td>" + invitem.Name + "</td>"+
                    "<td>" + invitem.Amount + "</td>" +
                    "</tr>";

            }
            htmltable += "</tbody>" +
                "</table>";
            return htmltable;
        }

        //Let user Download the Invoice Report as CSV
        protected void DownloadCSV_Click(object sender, EventArgs e)
        {
            InvoicingSystemContext context = new InvoicingSystemContext();
            InvoiceList = context.Invoices.ToList();
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("Invoice ID,Company Name,Invoice Amount");
                    writer.WriteLine();
                    if (InvoiceList.Count > 0)
                    {
                        for (int i = 0; i < InvoiceList.Count(); i++)
                        {
                            writer.Write(InvoiceList[i].Id + ",");
                            writer.Write("\"" + InvoiceList[i].Client + "\"" + ",");
                            writer.Write(InvoiceList[i].Invoice_Amount.ToString() + ",");
                            writer.WriteLine();
                        }
                    }
                }
                string saveAsFileName = "Invoice.csv";
                Response.ContentType = "text/comma-seperated-values";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                Response.Clear();
                Response.BinaryWrite(stream.GetBuffer());
                Response.End();
            }
        }

        //Let user Download the Customer Report as CSV
        protected void DownloadCustomerCSV_Click(object sender, EventArgs e)
        {
            InvoicingSystemContext context = new InvoicingSystemContext();
            InvoiceList = context.Invoices.ToList();
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write("Company Name,Invoice Amount Invoiced,Total Amount Paid,Total Amount Outstanding");
                        writer.WriteLine();
                        if (InvoiceList.Count > 0)
                        {
                            for (int i = 0; i < InvoiceList.Count(); i++)
                            {
                                writer.Write("\"" + InvoiceList[i].Client + "\"" + ",");
                                writer.Write(InvoiceList[i].Invoice_Amount.ToString() + ",");
                                if (InvoiceList[i].Invoice_Status == Invoice_Status_Enum.paid.ToString())
                                {
                                    writer.Write(InvoiceList[i].Invoice_Amount_Plus_VAT.ToString() + ",");
                                }
                                else
                                {
                                    writer.Write(String.Format("{0:0.00000}", 0) + ",");
                                }
                                if (InvoiceList[i].Invoice_Status == Invoice_Status_Enum.unpaid.ToString())
                                {
                                    writer.Write(InvoiceList[i].Invoice_Amount_Plus_VAT.ToString() + ",");
                                }
                                else
                                {
                                    writer.Write(String.Format("{0:0.00000}", 0) + ",");
                                }
                                writer.WriteLine();
                            }
                        }
                    }
                    string saveAsFileName = "CustomerReport.csv";
                    Response.ContentType = "text/comma-seperated-values";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", saveAsFileName));
                    Response.Clear();
                    Response.BinaryWrite(stream.GetBuffer());
                    Response.End();
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Error.aspx");
            }
            
        }

        //Web Method to set the payment status of each invoice via the interface to paid / unpaid
        //If the invoice is paid, set to unpaid
        //If the invoice is unpaid, set to paid
        [WebMethod]
        public static string PayInvoice(int invoice_id)
        {
            InvoicingSystemContext context = new InvoicingSystemContext();
            int id = invoice_id;
            Invoice invoice = context.Invoices.FirstOrDefault(i => i.Id == id);
            if (invoice != null)
            {
                if(invoice.Invoice_Status == Invoice_Status_Enum.unpaid.ToString())
                {
                    invoice.Invoice_Status = Invoice_Status_Enum.paid.ToString();
                }
                else if(invoice.Invoice_Status == Invoice_Status_Enum.paid.ToString())
                {
                    invoice.Invoice_Status = Invoice_Status_Enum.unpaid.ToString();
                }
                context.SaveChanges();
            }

            List<Invoice> InvoiceList = context.Invoices.ToList();
            return new Default().BuildHTMLTable(InvoiceList);
        }

        //Web Method to let user see the detail items of the invoice
        [WebMethod]
        public static string Details(int invoice_id)
        {
            InvoicingSystemContext context = new InvoicingSystemContext();
            int id = invoice_id;
            List<Invoice_Item> InvocieItemList = context.Invoice_Items.Where(i => i.Invoice_Id == id).ToList();
            return new Default().BuildHTMLTable(InvocieItemList);
        }
    }
}