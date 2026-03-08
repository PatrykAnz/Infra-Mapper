import { useEffect, useState } from 'react';
import api from '../services/api';

const Dashboard = () => {
  const [items, setItems] = useState<{ id: number; name: string }[]>([]);
  const [error, setError] = useState("");

  useEffect(() => {
    // Próba pobrania danych przy starcie komponentu
    api.get('/items')
      .then(res => setItems(res.data))
      .catch(() => setError("Front-end połączony"));
  }, []);

  return (
    <div style={{ padding: '20px', textAlign: 'center' }}>
      <h1>Infra-Mapper Dashboard</h1>
      {error && <p style={{ color: 'orange', fontWeight: 'bold' }}>{error}</p>}
      <ul>
        {items.map((item) => (
          <li key={item.id}>{item.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default Dashboard;
