import React, { useEffect, useState, Fragment } from "react";
import axios from "axios";
import IotSensorDisplay from "../components/iot/IotSensorDisplay";
import DataTable from "../components/UI/DataTable";
const Iot = () => {
  const [value, setValue] = useState(0);
  const [historicalData, setHistoricaldata] = useState([]);
  useEffect(() => {
    const getTempHistoricalData = async () => {
      const response = await axios.get("https://localhost:5000/api/iot");
      setValue(response.data.value);
      let dataRequest = response.data.historicalData.map((d) => {
        return {
          id:d._id,
          publishTimestamp: d.publishTimestamp,
          displayName: d.content[0].data[0].values[0].displayName,
          value: d.content[0].data[0].values[0].value,
        };
      });
      setHistoricaldata(dataRequest);
    };
    getTempHistoricalData();
    let interval = setInterval(() => {
      getTempHistoricalData();
    }, 10000);
    return () => clearInterval(interval);
  }, []);
  return (
    <Fragment>
      <IotSensorDisplay value={value} />
      <DataTable rows={historicalData} />
    </Fragment>
  );
};

export default Iot;
