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
    public partial class FrmPositionList : Form
    {


        public FrmPositionList()
        {
            InitializeComponent();
        }
        PositionDTO detail = new PositionDTO();
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPosition frm = new FrmPosition();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.ID == 0)
                MessageBox.Show("Please seelct a position from the table");
            else
            {
                FrmPosition frm = new FrmPosition();
                frm.IsUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillGrid();
                
            }

        }

        List<PositionDTO> positionList = new List<PositionDTO>();
        void FillGrid()
        {
            positionList = PositionBLL.GetPositions();
            dataGridView1.DataSource = positionList;
        }


        private void FrmPositionList_Load(object sender, EventArgs e)
        {
            FillGrid();
            dataGridView1.Columns[0].HeaderText = "Position Name";
            //dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Position ID";
            dataGridView1.Columns[3].HeaderText = "Department Name";
            dataGridView1.Columns[4].HeaderText = "Department ID";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.PositionName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            detail.DepartmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            detail.OldDepartmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Warning!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                PositionBLL.DeletePosition(detail.ID);
                MessageBox.Show("Position has been deleted.");
                FillGrid();
             
            }
        }
    }
}
