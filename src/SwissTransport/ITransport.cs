﻿namespace SwissTransport
{
    using System;
    public interface ITransport
    {
        Stations GetStations(string query);
        StationBoardRoot GetStationBoard(string station, string id);
        Connections GetConnections(string fromStation, string toStattion, DateTime date);
    }
}