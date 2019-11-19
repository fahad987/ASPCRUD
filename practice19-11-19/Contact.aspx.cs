using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace practice19_11_19
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-CO8DTB7;Initial Catalog=ASPCRUD;Integrated Security=true;");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlcon.State == ConnectionState.Closed)
                sqlcon.Open();
            SqlCommand sqlCmd = new SqlCommand("ContactCreateOrUpdate", sqlcon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ContactID",(hfContactID.Value==""?0:Convert.ToInt32(hfContactID.Value)));
            sqlCmd.Parameters.AddWithValue("Name",txtName.Text.Trim());
            sqlCmd.Parameters.AddWithValue("Mobile",txtMobile.Text.Trim());
            sqlCmd.Parameters.AddWithValue("Address",txtAddress.Text.Trim());
            sqlCmd.ExecuteNonQuery();
            sqlcon.Close();
            string contactID = hfContactID.Value;
            clear();
            if(hfContactID.Value=="")
                lblSuccessMessage.Text = "Saved Successfully";
                else
              lblSuccessMessage.Text = "Update Successfully";
            FillGridView();
            
        }
        void FillGridView()
        {
            if (sqlcon.State == ConnectionState.Closed)
                sqlcon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll",sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            gvContact.DataSource = dtbl;
            gvContact.DataBind();
        }

        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlcon.State == ConnectionState.Closed)
                sqlcon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewById", sqlcon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ContactID", contactID);
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            sqlcon.Close();
            hfContactID.Value = contactID.ToString();
            txtName.Text = dtbl.Rows[0]["Name"].ToString();
            txtMobile.Text = dtbl.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (sqlcon.State == ConnectionState.Closed)
                sqlcon.Open();
            SqlCommand sqlCmd = new SqlCommand("ContactDeleteByID",sqlcon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
            sqlCmd.ExecuteNonQuery();
            sqlcon.Close();
            clear();
            FillGridView();
            lblSuccessMessage.Text = "Deleted Successfully";
        }
    }
}