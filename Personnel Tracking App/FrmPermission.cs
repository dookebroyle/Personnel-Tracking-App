using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using BLL;
using DAL.DTO;

namespace Personnel_Tracking_App
{
    public partial class FrmPermission : Form
    {
        public FrmPermission()
        {
            InitializeComponent();
        }

        TimeSpan PermissionDay;
        public bool IsUpdate = false;
        public PermissionDetailDTO detail = new PermissionDetailDTO();




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void FrmPermission_Load(object sender, EventArgs e)
        {

            txtUserNo.Text = UserStatic.UserNo.ToString();
            if(IsUpdate)
            {
                dtStartDate.Value = detail.StartDate;
                dtEndDate.Value = detail.EndDate;
                txtDayAmount.Text = detail.PermissionDayAmount.ToString();
                txtExplanation.Text = detail.Explanation;
                txtUserNo.Text = detail.UserNo.ToString();
            }
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dtEndDate.Value.Date - dtStartDate.Value.Date;
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dtEndDate.Value.Date - dtStartDate.Value.Date;
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDayAmount.Text.Trim() == "")
                MessageBox.Show("please change end or start date");
            else if (Convert.ToInt32(txtDayAmount.Text) <= 0)
                MessageBox.Show("Permission day must be bigger than 0");
            else if (txtExplanation.Text.Trim() == "")
                MessageBox.Show("Explanation is empty");
            else
            {
                PERMISSION permission = new PERMISSION();
                if (!IsUpdate)
                {
                    permission.EmployeeID = UserStatic.EmployeeID;
                    permission.PermissionState = 1;
                    permission.PermissionStateDate = dtStartDate.Value.Date;
                    permission.PermissionEndDate = dtEndDate.Value.Date;
                    permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                    permission.PermissionExplanation = txtExplanation.Text;
                    PermissionBLL.AddPermissions(permission);
                    MessageBox.Show("Permission was Added");
                    permission = new PERMISSION();
                    dtStartDate.Value = DateTime.Today;
                    dtEndDate.Value = DateTime.Today;
                    txtDayAmount.Clear();
                    txtExplanation.Clear();
                }
                else if (IsUpdate)
                {
                    DialogResult result = MessageBox.Show("Are you sure", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        permission.ID = detail.PermissionID;
                        permission.PermissionExplanation = txtExplanation.Text;
                        permission.PermissionStateDate = dtStartDate.Value;
                        permission.PermissionEndDate = dtEndDate.Value;
                        permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                        PermissionBLL.UpdatePermission(permission);
                        MessageBox.Show("Permission was Updated");
                        this.Close();

                    }

                }


            }
        }
    }


}

