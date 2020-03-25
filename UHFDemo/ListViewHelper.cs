using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Windows.Forms;

namespace UHFDemo
{
    public class ListViewHelper
    {
        private List<int> sortColumn = new List<int>();
        /// <summary>
        /// Constructor
        /// </summary>
        public ListViewHelper()
        {
        }
        public void ListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            System.Windows.Forms.ListView lv = sender as System.Windows.Forms.ListView;
            if (!sortColumn.Contains(e.Column))
                return;

            if (e.Column == (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn)
            {

                if ((lv.ListViewItemSorter as ListViewColumnSorter).Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {

                (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn = e.Column;
                (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Ascending;
            }

            ((System.Windows.Forms.ListView)sender).Sort();
        }

        public void addSortColumn(int column)
        {
            sortColumn.Add(column);
        }

        public bool removeSortColumn(int column)
        {
            return sortColumn.Remove(column);
        }
    }
    /// <summary>

    /// </summary>
    public class ListViewColumnSorter : System.Collections.IComparer
    {
        /// <summary>
        /// </summary>
        private int ColumnToSort;
        /// <summary>

        /// </summary>
        private System.Windows.Forms.SortOrder OrderOfSort;
        /// <summary>

        /// </summary>
        private System.Collections.CaseInsensitiveComparer ObjectCompare;
        /// <summary>
        /// Constructor
        /// </summary>
        public ListViewColumnSorter()
        {

            ColumnToSort = 0;

            OrderOfSort = System.Windows.Forms.SortOrder.None;

            ObjectCompare = new System.Collections.CaseInsensitiveComparer();
        }
        /// <summary>

        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            System.Windows.Forms.ListViewItem listviewX, listviewY;

            listviewX = (System.Windows.Forms.ListViewItem)x;
            listviewY = (System.Windows.Forms.ListViewItem)y;
            string xText = listviewX.SubItems[ColumnToSort].Text;
            string yText = listviewY.SubItems[ColumnToSort].Text;
            int xInt, yInt;

            if (IsIP(xText) && IsIP(yText))
            {
                compareResult = CompareIp(xText, yText);
            }
            else if (int.TryParse(xText, out xInt) && int.TryParse(yText, out yInt))
            {

                compareResult = CompareInt(xInt, yInt);
            }
            else
            {

                compareResult = ObjectCompare.Compare(xText, yText);
            }

            if (OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
            {

                return compareResult;
            }
            else if (OrderOfSort == System.Windows.Forms.SortOrder.Descending)
            {

                return (-compareResult);
            }
            else
            {

                return 0;
            }
        }
        /// <summary>

        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIP(String ip)
        {
            return System.Text.RegularExpressions.Regex.Match(ip, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$").Success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipx"></param>
        /// <param name="ipy"></param>
        /// <returns></returns>
        private int CompareInt(int x, int y)
        {
            if (x > y)
            {
                return 1;
            }
            else if (x < y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>

        /// </summary>
        /// <param name="ipx"></param>
        /// <param name="ipy"></param>
        /// <returns></returns>
        private int CompareIp(string ipx, string ipy)
        {
            string[] ipxs = ipx.Split('.');
            string[] ipys = ipy.Split('.');
            for (int i = 0; i < 4; i++)
            {
                if (Convert.ToInt32(ipxs[i]) > Convert.ToInt32(ipys[i]))
                {
                    return 1;
                }
                else if (Convert.ToInt32(ipxs[i]) < Convert.ToInt32(ipys[i]))
                {
                    return -1;
                }
                else
                {
                    continue;
                }
            }
            return 0;
        }
        /// <summary>

        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }
        /// <summary>

        /// </summary>
        public System.Windows.Forms.SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
