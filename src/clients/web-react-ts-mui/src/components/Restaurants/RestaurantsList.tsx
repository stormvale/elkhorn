import { useState, useEffect } from 'react';
import { DataGrid } from '@mui/x-data-grid';

const columns = [
  { field: 'id', headerName: 'ID', width: 70 },
  { field: 'name', headerName: 'Name', width: 300 }
];

export default function RestaurantsList() {
  const [rows, setRows] = useState([]);

  useEffect(() => {
    fetch('https://localhost:7025/restaurants/')
        .then((res) => res.json())
        .then((data) => setRows(data))
        .catch((err) => console.error('API fetch error:', err));
  }, []);

  return (
      <div style={{ height: 700, width: '100%' }}>
        <DataGrid rows={rows} columns={columns} pageSizeOptions={[5]} />
      </div>
  );
}
