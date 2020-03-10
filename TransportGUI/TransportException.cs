using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TransportGUI {
    class TransportException {
        private string name;
        private string message;
        public TransportException (string name, string message) {
            this.name = name;
            this.message = message;
            showTransportExceptionDialog();
        }

        public void showTransportExceptionDialog () {
            MessageBox.Show(message, name, MessageBoxButton.OK);
        }
    }
}
