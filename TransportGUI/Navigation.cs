using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace TransportGUI {
    class Navigation {
        public void navigate(List<GroupBox> navigationItems,string content) {
            // Make Groupboxes visible and hide the rest
            foreach (GroupBox box in navigationItems) {
                if (box.Name.ToString() == "groupBox" + content) {
                    box.Visibility = Visibility.Visible;
                } else {
                    box.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
