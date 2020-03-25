using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFDemo
{
    class DBListView : System.Windows.Forms.ListView
    {
        public DBListView()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw |
              System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer |
              System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
    }

}
