﻿using System;
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
    public partial class FrmTaskList : Form
    {
        public FrmTaskList()
        {
            InitializeComponent();
        }

        TaskDTO dto = new TaskDTO();
        private bool combofull = false;
        TaskDetailDTO detail = new TaskDetailDTO();

        void FillAllData()
        {
            dto = TaskBLL.GetAll();
            if (!UserStatic.isAdmin)
            {
                dto.Tasks = dto.Tasks.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();

            }
            dataGridView1.DataSource = dto.Tasks;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbTaskState.DataSource = dto.TaskStates;
            cmbTaskState.DisplayMember = "StateName";
            cmbTaskState.ValueMember = "ID";
            cmbPosition.SelectedIndex = -1;
            cmbTaskState.SelectedIndex = -1;
            combofull = true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();



        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (detail.TaskID == 0)
                MessageBox.Show("Please select a task from the list");
            else
            {
                
                FrmTask frm = new FrmTask();
                frm.IsUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }

        }
 
        private void FrmTaskList_Load(object sender, EventArgs e)
        {
            FillAllData();
           // dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "User No";
            //dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Name";
            dataGridView1.Columns[4].HeaderText = "Surname";
            dataGridView1.Columns[5].HeaderText = "Start Date";
            dataGridView1.Columns[6].HeaderText = "Delivery Date";
            dataGridView1.Columns[7].HeaderText = "Task State";
            //dataGridView1.Columns[8].Visible = false;
            //dataGridView1.Columns[9].Visible = false;
            //dataGridView1.Columns[10].Visible = false;
            //dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].HeaderText = "Task ID";
            //dataGridView1.Columns[13].Visible = false;
            //dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.isAdmin)
            {
                btnNew.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                panelForAdmin.Visible = false;
                btnApprove.Text = "Delivery";
                
            }
            
            
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID ==
                Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }

        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {

                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource = list.Where(x => x.PositionID ==
                Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbDeliveryDate.Checked = false;
            rbStartDate.Checked = false;
            cmbTaskState.SelectedIndex = -1;
            dataGridView1.DataSource = dto.Tasks;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (rbStartDate.Checked)
                list = list.Where(x => x.TaskStartDate > Convert.ToDateTime(dtStart.Value) &&
                x.TaskStartDate < Convert.ToDateTime(dtFinish.Value)).ToList();
            if (rbDeliveryDate.Checked)
                list = list.Where(x => x.TaskStartDate > Convert.ToDateTime(dtStart.Value) &&
                x.TaskStartDate < Convert.ToDateTime(dtFinish.Value)).ToList();
            if (cmbTaskState.SelectedIndex != -1)
                list = list.Where(x => x.TaskStateID == (Convert.ToInt32(cmbTaskState.SelectedValue))).ToList();
            dataGridView1.DataSource = list;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.Name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.Surname = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            detail.Title = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            detail.Content = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.TaskStateID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[15].Value);
            detail.StateName = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            detail.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            detail.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[15].Value);
            detail.TaskStartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            detail.TaskDeliveryDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[6].Value);




        }

        private void cmbTaskState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {

                List<TaskDetailDTO> list = dto.Tasks;
                dataGridView1.DataSource = list.Where(x => x.TaskStateID ==
                Convert.ToInt32(cmbTaskState.SelectedValue)+1).ToList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Warning!", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                TaskBLL.DeleteTask(detail.TaskID);
                MessageBox.Show("Task has been deleted.");
                FillAllData();
                CleanFilters();
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.OnEmployee && detail.EmployeeID != UserStatic.EmployeeID)
                MessageBox.Show("Before you approve a task, employee has to deliver task.");
            else if (UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
                MessageBox.Show("This task is already approved");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Delivered)
                MessageBox.Show("This task is already delivered.");
            else if(!UserStatic.isAdmin && detail.TaskStateID == TaskStates.Approved)
            {
                MessageBox.Show("This task is already approved");

            }
            else
            {
                TaskBLL.ApproveTask(detail.TaskID, UserStatic.isAdmin);
                MessageBox.Show("Task has been updated.");
                FillAllData();
                CleanFilters();
            }
        }
    }
}
