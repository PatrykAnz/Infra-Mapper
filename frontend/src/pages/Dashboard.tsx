import React, { useEffect, useState } from 'react';
import api from '../services/api';

interface CloudTask {
  id: number;
  name: string;
  isCompleted: boolean;
}

const Dashboard = () => {
  const [items, setItems] = useState<CloudTask[]>([]);
  const [error, setError] = useState('');
  const [newTaskName, setNewTaskName] = useState('');

  const fetchTasks = () => {
    api
      .get('/api/tasks')
      .then((res: any) => {
        setItems(res.data);
        setError('');
      })
      .catch((err: any) => {
        console.error('Szczegóły błędu:', err);
        setError('Błąd połączenia z API. Sprawdź, czy backend Infra-Mapper działa na porcie 8081.');
      });
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  const handleDelete = async (id: number) => {
    try {
      await api.delete(`/api/tasks/${id}`);
      setItems((prev) => prev.filter((item) => item.id !== id));
      setError('');
    } catch (err) {
      console.error(err);
      setError('Nie udało się usunąć zadania.');
    }
  };

  const handleToggle = async (item: CloudTask) => {
    try {
      const updated = { ...item, isCompleted: !item.isCompleted };
      await api.put(`/api/tasks/${item.id}`, updated);
      setItems((prev) => prev.map((t) => (t.id === item.id ? updated : t)));
      setError('');
    } catch (err) {
      console.error(err);
      setError('Nie udało się zaktualizować zadania.');
    }
  };

  const handleAddTask = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newTaskName.trim()) return;
    try {
      await api.post('/api/tasks', { name: newTaskName });
      setNewTaskName('');
      fetchTasks();
      setError('');
    } catch (err) {
      console.error('Błąd podczas dodawania:', err);
      setError('Nie udało się dodać zadania. Spróbuj ponownie.');
    }
  };

  return (
    <div style={{ padding: '20px', textAlign: 'center', fontFamily: 'Arial, sans-serif' }}>
      <h1 className="dashboard-title">☁️ Infra-Mapper Dashboard</h1>

      {error && (
        <div
          style={{
            background: '#fff3cd',
            color: '#856404',
            padding: '10px',
            borderRadius: '5px',
            margin: '20px auto',
            maxWidth: '400px',
          }}
        >
          {error}
        </div>
      )}

      <form onSubmit={handleAddTask} style={{ marginBottom: '30px' }}>
        <input
          type="text"
          placeholder="Wpisz nowe zadanie..."
          value={newTaskName}
          onChange={(e) => setNewTaskName(e.target.value)}
          style={{ padding: '10px', width: '250px', borderRadius: '4px', border: '1px solid #ccc' }}
        />
        <button
          type="submit"
          style={{
            marginLeft: '10px',
            padding: '10px 20px',
            backgroundColor: '#007bff',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
          }}
        >
          Dodaj
        </button>
      </form>

      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        {items.length === 0 && !error && <p>Brak zadań. Dodaj pierwsze zadanie.</p>}
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {items.map((item) => (
            <li
              key={item.id}
              style={{
                background: '#f8f9fa',
                margin: '10px',
                padding: '15px',
                borderRadius: '8px',
                borderLeft: item.isCompleted ? '5px solid #28a745' : '5px solid #6c757d',
                width: '400px',
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
              }}
            >
              <div>
                <input
                  type="checkbox"
                  checked={item.isCompleted}
                  onChange={() => handleToggle(item)}
                  style={{ marginRight: '10px' }}
                />
                <span
                  style={{
                    textDecoration: item.isCompleted ? 'line-through' : 'none',
                    color: '#333',
                  }}
                >
                  {item.name}
                </span>
              </div>
              <button
                type="button"
                onClick={() => handleDelete(item.id)}
                style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '18px' }}
                aria-label="Usuń zadanie"
              >
                🗑️
              </button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default Dashboard;
