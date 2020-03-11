using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissTransport;

namespace TransportGUI {
    class SearchSingleStation {

        private Transport transport;
        public SearchSingleStation (Transport transport) {
            this.transport = transport;
        }

        private Stations searchForStation(string searchValue) {
            if (searchValue.Length != 0) {
                // Call webservice with the users query
                string query = searchValue;
                Stations station = transport.GetStations(query);
                return station;
            } else {
                new TransportException("Textbox ist leer", "Geben sie eine Station an!");
                return null;
            }
        }
    }
}
