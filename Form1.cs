using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string connectionString = "User Id=chris;Password=1111;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)))";
            OracleConnection myConnection = new OracleConnection(connectionString);

            string commandString = "SELECT MENU_ORDER_ID, PC_SIT_ID, USER_ID, MENU_NAME, MENU_COST FROM MENU_ORDER_LOG JOIN MENU_INFO ON MENU_ORDER_LOG.MENU_ID=MENU_INFO.MENU_ID";
            OracleCommand myCommand = new OracleCommand();
            myCommand.Connection = myConnection;
            myCommand.CommandText = commandString;

            myConnection.Open();

            OracleDataReader MR;
            MR = myCommand.ExecuteReader();

            while (MR.Read())
            {
                ListViewItem item = new ListViewItem(MR[0].ToString());
                item.SubItems.Add(MR[1].ToString());
                item.SubItems.Add(MR[2].ToString());
                item.SubItems.Add(MR[3].ToString());
                item.SubItems.Add(MR[4].ToString());
                listView1.Items.Add(item);
            }

            MR.Close();
            myConnection.Close();

            listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
        }
        
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 첫 번째 리스트뷰에서 선택된 아이템이 있는지 확인
            if (listView1.SelectedItems.Count > 0)
            {
                // 첫 번째 리스트뷰에서 선택된 아이템의 데이터 가져오기
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string selectedOrderID = selectedItem.SubItems[0].Text;
                string selectedMenuName = selectedItem.SubItems[3].Text;

                // 두 번째 리스트뷰 업데이트
                UpdateSecondListView(selectedOrderID, selectedMenuName);
            }
        }

        private void UpdateSecondListView(string selectedOrderID, string selectedMenuName)
        {
            // 중복을 피하기 위해 이미 해당 데이터가 있는지 확인
            bool isAlreadyAdded = false;

            foreach (ListViewItem item in listView2.Items)
            {
                if (item.SubItems[0].Text == selectedOrderID && item.SubItems[1].Text == selectedMenuName)
                {
                    isAlreadyAdded = true;
                    break;
                }
            }

            // 중복이 아닌 경우에만 두 번째 리스트뷰에 추가
            if (!isAlreadyAdded)
            {
                ListViewItem newItem = new ListViewItem(new[] { selectedOrderID, selectedMenuName });
                listView2.Items.Add(newItem);
            }
        }

        // 데이터를 뿌려주기 위해서 조인써서 필요한 데이터만 가져오긴 했는데(기존 테이블에도 몇가지 데이터들을 무작위로 넣어서)
        // 근데 실질적으로 새 테이블을 생성하거나 기존 수정하는게 더 편할듯?
        // 사용자 PC에서 주문을 해야 넘어올 수 있는 데이터라?? 후행작업부터 한 기분
        // 일단은 리스트뷰 두개써서 리스트뷰 내에 일렬번호 클릭으로 두 리스트뷰가 상호작용 할 수 있게 생각하고 구현해봄
        // 주문 상세 내역에 음식 + 음식 + 음식 이렇게 표현하는 방법을 생각하지 못하겠음
        // 결제는 솔직히 포기... API끌어오는것도 시간 나면 하는게 맞는듯? 일단 버튼으로 구현...
    }
}
