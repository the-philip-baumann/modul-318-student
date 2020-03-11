using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissTransport;



namespace TransportGUI {
    class ApiSearches {

        private Transport transport;
        public ApiSearches (Transport transport) {
            this.transport = transport;
        }

        public Stations searchForStation (string searchValue) {
            if (searchValue.Length != 0) {
                // Call webservice with the users query
                string query = searchValue;
                Stations station = transport.GetStations(query);
                return station;
            } else {
                // Feedback
                new TransportException("Textbox ist leer", "Geben sie eine Station an!");
                return null;
            }
        }

        public List<Connection> searchForConnection (string start, string destination, DateTime date) {
            if (start.Length != 0 && destination.Length != 0 && date != null) {
                // Call webservice
                List<Connection> connections = transport.GetConnections(start, destination, date).ConnectionList;
                return connections;
            } else {
                // Feedback
                new TransportException("Datum, Start, oder Ziel sind leer", "Füllen sie alle Felder: Start- Ziel- und Datefeld aus");
                return null;
            }
        }

        public List<StationBoard> searchForStationBoard (string searchValue) {
            if (searchValue.Length != 0) {
                // Call webservice
                StationBoardRoot stationBoardRoot = transport.GetStationBoard(searchValue, "");
                List<StationBoard> stationBoards = stationBoardRoot.Entries.ToList<StationBoard>();
                return stationBoards;
            } else {
                // Feedback
                new TransportException("Keine Ergebnisse gefunden", "Für Ihre Suchanfrage wurde kein Ergebnis gefunden");
                return null;
            }
        }
    }
}
