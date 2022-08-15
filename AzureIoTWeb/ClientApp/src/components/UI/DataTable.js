import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";

const DataTable = (props) => {
  const columns = [
    { field: "id", headerName: "ID", width: "50%", align: "center" , flex:1 },
    {
      field: "publishTimestamp",
      headerAlign: "center",
      align: "center",
      headerName: "Time Stamp",
      flex: 1,
    },
    {
      field: "value",
      headerAlign: "center",
      align: "center",
      headerName: "Value",
      type: "number",
      flex: 1,
    },
  ];

  // const rows = [
  //   { id:1, publishTimestamp: 'Jon', value: 35 }
  // ];
  return (
    <div style={{ height: 400, width: "100%" }}>
      <DataGrid
        sx={{
          backgroundColor: "rgba(0,0,0,0.6)",
          color: "white",
        }}
        initialState={{
            columns: {
              columnVisibilityModel: {
                // Hide columns status and traderName, the other columns will remain visible
                id:false
              },
            },
          }}
        rows={props.rows}

        getRowId={(row) => row.id}
        columns={columns}
        pageSize={25}
        rowsPerPageOptions={[2]}
        density={"compact"}
      />
    </div>
  );
};
export default DataTable;
