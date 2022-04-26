using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using DAL.DTO;



namespace Personnel_Tracking_App
{
    public partial class FrmPosition : Form
    {
        public FrmPosition()
        {
            InitializeComponent();
        }

        List<DEPARTMENT> departmentList = new List<DEPARTMENT>();
        public PositionDTO detail = new PositionDTO();
        public bool IsUpdate = false;


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void FrmPosition_Load(object sender, EventArgs e)
        {
            departmentList = DepartmentBLL.GetDepartments();
            cmbDepartment.DataSource = departmentList;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            if(IsUpdate)
            {
                txtPosition.Text = detail.PositionName;
                cmbDepartment.SelectedValue = detail.DepartmentID;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPosition.Text.Trim()=="")
            {
                MessageBox.Show("Please fill the position name.");
            }
            else if (cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a department");
            }
            else
            {
                if (!IsUpdate)
                {
                    POSITION position = new POSITION();
                    position.PositionName = txtPosition.Text;
                    position.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    BLL.PositionBLL.AddPosition(position);
                    MessageBox.Show("Position was added.");
                    txtPosition.Clear();
                    cmbDepartment.SelectedIndex = -1;
                }
                else
                {
                    POSITION position = new POSITION();
                    position.ID = detail.ID;
                    position.PositionName = txtPosition.Text;
                    position.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    bool control = false;
                    if (Convert.ToInt32(cmbDepartment.SelectedValue) != detail.OldDepartmentID)
                        control = true;
                    PositionBLL.UpdatePosition(position, control);
                    MessageBox.Show("Position has been updated.");
                    this.Close();
                }
            }
        }

        private void txtDepartment_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
