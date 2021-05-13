using AzureForms.Helper;
using AzureForms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureForms
{
    public partial class azureForms : Form
    {
        HttpHelper helper;
        public azureForms()
        {
            InitializeComponent();
            StyleGrid();
            ShowGrid(false);
            InitializeForDev();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }
        /// <summary>
        /// data grid view click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells. 
            if (e.RowIndex < 0 || e.ColumnIndex != dataGridView1.Columns["actionBtn"].Index
                ) return;
            //get the instance id and current status of the vm instance from row
            string instanceId = dataGridView1.Rows[e.RowIndex].Cells["Instance Id"].Value.ToString();
            string status = dataGridView1.Rows[e.RowIndex].Cells["Status"].Value.ToString();

            //show prompt
            Prompt prompt;
            //if status is running show the deallocate prompt
            if(status == "running")
            {
                string message = "Deallocate instance with Id: " + instanceId;
                
                prompt = new Prompt(message,true);
                DialogResult dr = prompt.ShowDialog();
                if(dr == DialogResult.OK)
                {
                    //if user presses ok then deallocate the computer
                    string result = helper.DeallocateComputer(instanceId).GetAwaiter().GetResult();
                    Console.WriteLine(result);
                }
            }
            else
            {
                //if status is deallocated show the start prompt
                string message = "Start machine with Id: " + instanceId;
                prompt = new Prompt(message,false);
                DialogResult dr = prompt.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string result = helper.StartInstance(instanceId).GetAwaiter().GetResult();
                    Console.WriteLine(result);
                }
            }
            //refresh the grid to show new data 
            RefreshData();
        }

        /// <summary>
        /// updates data in the grid
        /// </summary>
        private void RefreshData()
        {
            string result = helper.GetVmList().GetAwaiter().GetResult();
            dataGridView1.DataSource = GetVmData(result);
            dataGridView1.Refresh();
        }

        /// <summary>
        /// toggle grid and refresh button visibility
        /// </summary>
        /// <param name="v"></param>
        private void ShowGrid(bool v)
        {
            dataGridView1.Visible = v;
            refreshBtn.Visible = v;
        }

        private void StyleGrid()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView1.ColumnHeadersHeight = 50;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.GrayText;
            this.dataGridView1.Location = new System.Drawing.Point(54, 58);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(909, 440);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.ForeColor = Color.Black;
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = this.dataGridView1.DefaultCellStyle.BackColor;
            this.dataGridView1.DefaultCellStyle.SelectionForeColor = this.dataGridView1.DefaultCellStyle.ForeColor;


        }

        /// <summary>
        /// adds action button to datagrid view
        /// </summary>
        private void AddButtonToDataGrid()
        {
            DataGridViewButtonColumn startBtn = new DataGridViewButtonColumn();
            DataGridViewButtonColumn deallocateBtn = new DataGridViewButtonColumn();

            dataGridView1.Columns.Add(startBtn);
            startBtn.HeaderText = "Action";
            startBtn.Text = "Action";
            startBtn.Name = "actionBtn";
            startBtn.UseColumnTextForButtonValue = true;
            startBtn.FlatStyle = FlatStyle.Flat;

        }
        /// <summary>
        /// list vm button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listVmBtn_Click(object sender, EventArgs e)
        {
            using (WaitForm form = new WaitForm(InitializeValues))
                form.ShowDialog();
            helper = new HttpHelper();
            string result = helper.GetVmList().GetAwaiter().GetResult();
            dataGridView1.DataSource = GetVmData(result);
            AddButtonToDataGrid();
            ShowGrid(true);
        }

        /// <summary>
        /// Gets the datatable for the grid 
        /// </summary>
        /// <param name="inputString">Json input containing all the info about vms</param>
        /// <returns></returns>
        private DataTable GetVmData(string inputString)
        {
            try
            {
                //parse json input into vm model
                var values = VmssModel.FromJson(inputString);

                DataTable table = new DataTable();
                table.Columns.Add("Id", typeof(string));
                table.Columns.Add("Instance Id", typeof(string));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Status", typeof(string));
                table.Columns.Add("Ip", typeof(string));
                //table.Columns.Add("Status", typeof(string));
                foreach (Value vm in values.Value)
                {
                    VmInstanceModel v = GetVm(vm.InstanceId.ToString());
                    PublicIpAddressConfiguration publicIpConfig = vm.Properties.NetworkProfileConfiguration.NetworkInterfaceConfigurations[0].Properties.IpConfigurations[0].Properties.PublicIpAddressConfiguration;
                    string ipName = "";
                    if(publicIpConfig != null)
                    {
                        IpModel ip = GetIp(publicIpConfig.Name);
                        ipName = ip.Properties.IpAddress;
                    }

                    string powerState = v.Statuses[1].Code.Replace("PowerState/", string.Empty);
                    table.Rows.Add(vm.Id, vm.InstanceId, vm.Name, powerState,ipName);
                };
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IpModel GetIp(string name)
        {
            string res = helper.GetIp(name).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<IpModel>(res);
        }

        /// <summary>
        /// Get instance of vm. Used to get the state of vm
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        private VmInstanceModel GetVm(string instanceId)
        {
            string result = helper.GetVmInstance(instanceId).GetAwaiter().GetResult();
            VmInstanceModel model = JsonConvert.DeserializeObject<VmInstanceModel>(result);
            return model;
        }
        /// <summary>
        /// Initialize the api paramters required for making api calls from the input text box
        /// </summary>
        void InitializeValues()
        {
            //use these values for debug mode
            //string subscriptionId = "f1b96a0f-2775-420f-b692-3f13bcdf0060";
            //string clientId = "999d16a6-aa8a-48e7-9029-f25b35b01e53";
            //string clientSecret = "H~k7mM.ifSbhr-JPW_9l0-XL~_G1qIkhJs";
            //string tenantId = "afebbe05-f314-492a-9035-3b68dee7fea1";
            //string scaleSet = "nsVmScaleSet";
            //string resourceGroup = "DEVTEST-RG";

            //use this section for actual implementation
            string subscriptionId = string.Empty;
            string clientId = string.Empty;
            string tenantId = string.Empty;
            string clientSecret = string.Empty;
            string scaleSet = string.Empty;
            string resourceGroup = string.Empty;

            subscriptionBox.Invoke((MethodInvoker)(() => { subscriptionId = subscriptionBox.Text.Trim(); }));
            clientBox.Invoke((MethodInvoker)(() => { clientId = clientBox.Text.Trim(); }));
            tenantBox.Invoke((MethodInvoker)(() => { tenantId = tenantBox.Text.Trim(); }));
            secretBox.Invoke((MethodInvoker)(() => { clientSecret = secretBox.Text.Trim(); }));
            resourceGroupBox.Invoke((MethodInvoker)(() => { resourceGroup = resourceGroupBox.Text.Trim(); }));
            scaleSetBox.Invoke((MethodInvoker)(() => { scaleSet = scaleSetBox.Text.Trim(); }));


            if (string.IsNullOrEmpty(subscriptionId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientSecret))
            {
                MessageBox.Show("All fields must be non empty", "Invalid Field");
                return;
            }
            Vm.InitializeApiParams(subscriptionId, tenantId, clientId, clientSecret, scaleSet, resourceGroup);
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
        void InitializeForDev()
        {
            string subscriptionId = "f1b96a0f-2775-420f-b692-3f13bcdf0060";
            string clientId = "999d16a6-aa8a-48e7-9029-f25b35b01e53";
            string clientSecret = "H~k7mM.ifSbhr-JPW_9l0-XL~_G1qIkhJs";
            string tenantId = "afebbe05-f314-492a-9035-3b68dee7fea1";
            string resourceGroup = "DEVTEST-RG";
            subscriptionBox.Text = subscriptionId;
            clientBox.Text = clientId;
            secretBox.Text = clientSecret;
            tenantBox.Text = tenantId;
            resourceGroupBox.Text = resourceGroup;
        }
    }
}
