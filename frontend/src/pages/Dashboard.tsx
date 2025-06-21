import { useEffect, useState } from "react";
import api from "../services/api";

interface Client {
  id: number;
  name: string;
  email: string;
  balanceT: number;
}

interface Payment {
  id: number;
  clientId: number;
  amount: number;
  date: string;
}

export default function Dashboard() {
  const [clients, setClients] = useState<Client[]>([]);
  const [rate, setRate] = useState<number>(0);
  const [newRate, setNewRate] = useState<string>("");

  const [payments, setPayments] = useState<Payment[]>([]);

  const [editingClient, setEditingClient] = useState<Client | null>(null);
  const [form, setForm] = useState({ name: "", email: "", balanceT: "" });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    api.get<Client[]>("/clients").then((res) => setClients(res.data));
    api.get<{ value: number }>("/rate").then((res) => setRate(res.data.value));
    api.get<Payment[]>("/payments?take=5").then((res) => setPayments(res.data));
  };

  const updateRate = () => {
  api.post<{ value: number }>("/rate", { value: parseFloat(newRate) })
    .then((res) => {
      setRate(res.data.value);
      setNewRate("");
    });
};

  const handleEdit = (client: Client) => {
    setEditingClient(client);
    setForm({
      name: client.name,
      email: client.email,
      balanceT: client.balanceT.toString(),
    });
  };

  const handleDelete = (id: number) => {
    if (confirm("Удалить клиента?")) {
      api.delete(`/clients/${id}`).then(() => fetchData());
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const payload = {
      name: form.name,
      email: form.email,
      balanceT: parseFloat(form.balanceT),
    };

    if (editingClient) {
      api.put(`/clients/${editingClient.id}`, payload).then(() => {
        setEditingClient(null);
        setForm({ name: "", email: "", balanceT: "" });
        fetchData();
      });
    } else {
      api.post("/clients", payload).then(() => {
        setForm({ name: "", email: "", balanceT: "" });
        fetchData();
      });
    }
  };

  return (
    <div style={{ padding: "2rem", fontFamily: "Arial, sans-serif" }}>
      <h2>Клиенты</h2>
      <form onSubmit={handleSubmit} style={{ marginBottom: "1rem" }}>
        <input
          type="text"
          placeholder="Имя"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          required
        />
        <input
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={(e) => setForm({ ...form, email: e.target.value })}
          required
          style={{ marginLeft: "0.5rem" }}
        />
        <input
          type="number"
          placeholder="Баланс"
          value={form.balanceT}
          onChange={(e) => setForm({ ...form, balanceT: e.target.value })}
          required
          style={{ marginLeft: "0.5rem" }}
        />
        <button type="submit" style={{ marginLeft: "0.5rem" }}>
          {editingClient ? "Сохранить" : "Добавить"}
        </button>
        {editingClient && (
          <button
            type="button"
            onClick={() => {
              setEditingClient(null);
              setForm({ name: "", email: "", balanceT: "" });
            }}
            style={{ marginLeft: "0.5rem" }}
          >
            Отмена
          </button>
        )}
      </form>

      <table border={1} cellPadding={8} cellSpacing={0} style={{ marginBottom: "2rem", width: "100%" }}>
        <thead>
          <tr>
            <th>Имя</th>
            <th>Email</th>
            <th>Баланс токенов</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {clients.map((c) => (
            <tr key={c.id}>
              <td>{c.name}</td>
              <td>{c.email}</td>
              <td>{c.balanceT}</td>
              <td>
                <button onClick={() => handleEdit(c)}>✏️</button>
                <button onClick={() => handleDelete(c.id)} style={{ marginLeft: "0.5rem" }}>
                  🗑️
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2>Курс токенов</h2>
      <p>Текущий курс: <strong>{rate}</strong></p>
      <input
        type="number"
        placeholder="Новый курс"
        value={newRate}
        onChange={(e) => setNewRate(e.target.value)}
        style={{ marginRight: "1rem" }}
      />
      <button onClick={updateRate}>Обновить курс</button>

      <h2 style={{ marginTop: "3rem" }}>Последние 5 платежей</h2>
      <table border={1} cellPadding={8} cellSpacing={0} style={{ width: "100%" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>ID Клиента</th>
            <th>Сумма</th>
            <th>Дата</th>
          </tr>
        </thead>
        <tbody>
          {payments.map((p) => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.clientId}</td>
              <td>{p.amount}</td>
              <td>{new Date(p.date).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}


